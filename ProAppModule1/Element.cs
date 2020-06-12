using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Input;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace ProAppModule1
{
    public abstract class Element:PropertyChangedBase {

        // Fields
        public const string serviceURL = "https://services.arcgis.com/F7DSX1DSNSiWmOqh/arcgis/rest/services/GeodatabaseTNC/FeatureServer"; // Production
        //public const string serviceURL = "https://services.arcgis.com/F7DSX1DSNSiWmOqh/arcgis/rest/services/GeodatabaseTNC_Pruebas/FeatureServer"; // Testing
        public JavaScriptSerializer serializer = new JavaScriptSerializer();
        public string ElementName = "Elemento";
        public string ElementType = "Tipo de elemento";
        public string FilterType = ItemFilters.tables_all;
        public string OidField = "OBJECTID";
        public List<string> Columns;

        // Contructor
        protected Element()
        {
            Service = $"{serviceURL}/{Index}";

            var loader = new DataUploader(new FieldValidator(), new Geoprocessor());
            BrowseFileCommand = new RelayCommand(() => BrowseFile(), () => true);
            UploadCommand = new RelayCommand(() => { loader.UploadData(this); LoadData();}, () => true);

        }

        public ICommand BrowseFileCommand { get; }
        public ICommand UploadCommand { get; }

        // Properties
        private Item item;
        public Item Item { get => item; set { item = value; NotifyPropertyChanged(() => item); } }

        private string file;
        public string File { get => file; set { file = value; NotifyPropertyChanged(() => file); } }

        private int selectedIndex;
        public int SelectedIndex {get => selectedIndex; set {selectedIndex = value; NotifyPropertyChanged(() => SelectedIndex); } }

        public string Service { get; set; }
        public DataTable data { get; set; }
        public TableDefinition Definition { get; private set; }
        public FeatureClass Featureclass { get; private set; }
        public Table Table { get; private set; }
        public int Count { get; private set; }
        public int Index { get; set; }
        public string Type { get; set; }
        private Uri Gdb;
        public RowCursor Cursor { get; private set; }

        // Methods

        public void Initialization(int index, Item item) {
            Item = item;
            Type = item.Type;
        }

        public virtual async void LoadData()
        {

        }


        public Task FillDataTable()
        {
            return QueuedTask.Run(() =>
            {
                var _resultTable = new DataTable();

                foreach (var column in Columns)
                {
                    _resultTable.Columns.Add(new DataColumn(column));
                }

                var features = WebInteraction.Query(Service, "1=1", "*");

                foreach (var feature in (System.Collections.ArrayList)features)
                {
                    var feat = (Dictionary<string, object>)feature;
                    var atts = (Dictionary<string, object>)feat["attributes"];

                    var addRow = _resultTable.NewRow();
                    foreach (var column in Columns)
                    {
                        addRow[column] = atts[column];
                    }

                    _resultTable.Rows.Add(addRow);
                }

                data = _resultTable;
            });
        }

        public void BrowseFile()
        {
            var openTable = new OpenItemDialog()
            {
                Title = $"Seleccione {ElementType} que contiene los datos de {ElementName}",
                Filter = FilterType
            };
            Nullable<bool> result = openTable.ShowDialog();
            if (result == true)
            {
                Item = openTable.Items.First();
                File = Item.Path;
                NotifyPropertyChanged(() => File);
            }
        }


        public virtual async void UnselectRow()
        {

        }


        public async Task<int> GetProperties(Item _item)
        {
            var path = new Uri(_item.Path);
            var directory = new Uri(path, ".");

            if (Type == "File Geodatabase Feature Class")
            {
                var gdbPath = directory.AbsolutePath.Remove(directory.AbsolutePath.Length - 1);
                Gdb = new Uri(gdbPath);

                Featureclass = await QueuedTask.Run(() =>
                {
                    var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(Gdb));
                    var fc = geodatabase.OpenDataset<FeatureClass>(Item.Title);
                    return fc;
                });

                Table = await QueuedTask.Run(() =>
                {
                    Table table = Featureclass;
                    return table;
                });

            }
            else if (Type == "Shapefile")
            {

                Table = await QueuedTask.Run(() =>
                {
                    var shapefilePath = new FileSystemConnectionPath(directory, FileSystemDatastoreType.Shapefile);
                    var shapefile = new FileSystemDatastore(shapefilePath);
                    var tbl = shapefile.OpenDataset<Table>(_item.Title);
                    return tbl;
                });

            }

            else if (Type == "File Geodatabase Table")
            {

                var gdbPath = directory.AbsolutePath.Remove(directory.AbsolutePath.Length - 1);
                Gdb = new Uri(gdbPath);

                Table = await QueuedTask.Run(() =>
                {
                    var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(Gdb));
                    var table = geodatabase.OpenDataset<Table>(_item.Title);
                    return table;
                });

            }

            else if (Type == "Excel Table")
            {
                var outPath = Project.Current.DefaultGeodatabasePath;
                var outName = $"Tabla_{DateTime.Now:yyyyMMddHHmmssfff}";
                var xlsFile = Item.PhysicalPath;
                var sheetName = Item.Name.Remove(Item.Name.Length-1);
                //var outTable = "memory\\table";

                Table = await ConvertExcelToTable(xlsFile, sheetName, outPath, outName);
          
            }

            else if (Type == "Text File")
            {
                var outPath = Project.Current.DefaultGeodatabasePath;
                var outName = $"Tabla_{DateTime.Now:yyyyMMddHHmmssfff}";
                var csvFile = Item.Path;

                Table = await ConvertCSVToTable(csvFile, outPath, outName);
            }

            Definition = await QueuedTask.Run(() =>
            {
                var def = Table.GetDefinition();
                return def;
            });

            Count = await QueuedTask.Run(() => {
                var _c = Table.GetCount();
                return _c;
            });

            Cursor = await QueuedTask.Run(() =>
            {
                var cur = Table.Search();
                return cur;
            });

            return 0;
        }

        private async Task<Table> ConvertExcelToTable(string inputPath, string sheetName, string outPath, string outName)

        {
            var progressDlg = new ProgressDialog("Leyendo datos del archivo Excel seleccionado", "Cancelar", false);
            progressDlg.Show();

            var outTable = System.IO.Path.Combine(outPath, outName);
            var parameters = Geoprocessing.MakeValueArray(inputPath, outTable, sheetName);
            var result = await Geoprocessing.ExecuteToolAsync("conversion.ExcelToTable", parameters, null, new CancelableProgressorSource(progressDlg).Progressor, GPExecuteToolFlags.None);
            var _outTable = result.Values[0];

            var table = await QueuedTask.Run(() =>
            {
                var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(outPath)));
                var tbl = geodatabase.OpenDataset<Table>(outName);
                return tbl;
            });

            progressDlg.Hide();
            return table;

        }

        public virtual async Task<Table> ConvertCSVToTable(string inputPath, string outPath, string outName)

        {
            var progressDlg = new ProgressDialog("Leyendo datos del archivo CSV seleccionado", "Cancelar", false);
            progressDlg.Show();

            var parameters = Geoprocessing.MakeValueArray(inputPath, outPath, outName);
            var result = await Geoprocessing.ExecuteToolAsync("conversion.TableToTable", parameters, null, new CancelableProgressorSource(progressDlg).Progressor, GPExecuteToolFlags.None);
            var _outTable = result.Values[0];

            var table = await QueuedTask.Run(() =>
            {
                var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(outPath)));
                var tbl = geodatabase.OpenDataset<Table>(outName);
                return tbl;
            });

            progressDlg.Hide();
            return table;

        }

        public static DateTime UnixTimeStampToDateTime(string strUnixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var unixTimeStamp = double.Parse(strUnixTimeStamp)/1000;
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static int StringToInt(string value)
        {
            int new_value;
            try
            {
                new_value = Convert.ToInt32(value);
            }
            catch (Exception)
            {
                new_value = 0; ////review this value !!!!!!!!!!!
            }

            return new_value;
        }

        public virtual object Serialize(string json_geom)
        {
            return new { };
        }

        public string ToString(Row row, string field_name)
        {
            string new_value;
            try
            {
                new_value = Convert.ToString(row[field_name]);
            }
            catch (Exception)
            {
                new_value = "";
            }

            return new_value;
        }

        public double ToDate(Row row, string field_name)
        {
            double new_value;
            try
            {
                new_value = (TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(row[field_name])) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
            }
            catch (Exception)
            {
                new_value = 0; ////review this value !!!!!!!!!!!
            }
            return new_value;
        }

        public double ToDouble(Row row, string field_name)
        {
            double new_value;
            try
            {
                new_value = Convert.ToDouble(row[field_name]);
            }
            catch (Exception)
            {
                new_value = 0; ////review this value !!!!!!!!!!!
            }

            return new_value;
        }

        public int ToInt(Row row, string field_name)
        {
            int new_value;
            try
            {
                new_value = Convert.ToInt32(row[field_name]);
            }
            catch (Exception)
            {
                new_value = 0; ////review this value !!!!!!!!!!!
            }

            return new_value;
        }

        public virtual void UploadData() {}
        public virtual object FormatAttributes(Row row) { return new { }; }

    }
}