using System;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace ProAppModule1
{
    public abstract class Element {

        //public const string _serviceURL = "https://services9.arcgis.com/LQG65AprqDvQfUnp/ArcGIS/rest/services/TNCServices4/FeatureServer"; // Production
        public const string serviceURL = "https://services9.arcgis.com/LQG65AprqDvQfUnp/arcgis/rest/services/TNC_Pruebas/FeatureServer"; // Testing
        private Item _item;
        private string _service;
        private TableDefinition _definition;
        private FeatureClass _featureclass;
        private RowCursor _cursor;
        private Table _table;
        private Uri _gdb;
        private int _count;
        private int _index;
        private string _type;
        public JavaScriptSerializer serializer = new JavaScriptSerializer();

        public Item item { get => _item; }
        public string service { get => _service; }
        public TableDefinition definition { get => _definition; }
        public FeatureClass featureclas { get => _featureclass; }
        public int count { get => _count; }
        public int index { get => _index; set { _index = value; } }
        public string type { get => _type; set { _type = value; } }
        public RowCursor cursor { get => _cursor;}

        public Element(Item item) {

            _item = item;
            _type = _item.Type;
        }


        public void Initialization(int index) {

            _service = string.Format("{0}/{1}", serviceURL, index);
            
        }
        
        public async Task<int> GetProperties()
        {

            var path = new Uri(_item.Path);
            var directory = new Uri(path, ".");

            if (_type == "File Geodatabase Feature Class")
            {
                var gdbPath = directory.AbsolutePath.Remove(directory.AbsolutePath.Length - 1);
                _gdb = new Uri(gdbPath);

                _featureclass = await QueuedTask.Run(() =>
                {
                    var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(_gdb));
                    var fc = geodatabase.OpenDataset<FeatureClass>(item.Title);
                    return fc;
                });

                _table = await QueuedTask.Run(() =>
                {
                    Table table = _featureclass;
                    return table;
                });

            }
            else if (_type == "Shapefile")
            {

                _table = await QueuedTask.Run(() =>
                {
                    var shapefilePath = new FileSystemConnectionPath(directory, FileSystemDatastoreType.Shapefile);
                    var shapefile = new FileSystemDatastore(shapefilePath);
                    var tbl = shapefile.OpenDataset<Table>(_item.Title);
                    return tbl;
                });

            }

            else if (_type == "File Geodatabase Table")
            {

                var gdbPath = directory.AbsolutePath.Remove(directory.AbsolutePath.Length - 1);
                _gdb = new Uri(gdbPath);

                _table = await QueuedTask.Run(() =>
                {
                    var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(_gdb));
                    var table = geodatabase.OpenDataset<Table>(_item.Title);
                    return table;
                });

            }

            else if (_type == "Excel Table")
            {
                var outPath = Project.Current.DefaultGeodatabasePath;
                var outName = string.Format("Tabla_{0}", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                var xlsFile = item.PhysicalPath;
                var sheetName = item.Name.Remove(item.Name.Length-1);
                //var outTable = "memory\\table";

                _table = await ConvertExcelToTable(xlsFile, sheetName, outPath, outName);
          
            }

            else if (_type == "Text File")
            {
                var outPath = Project.Current.DefaultGeodatabasePath;
                var outName = string.Format("Tabla_{0}", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                var csvFile = item.Path;

                _table = await ConvertCSVToTable(csvFile, outPath, outName);
            }

            _definition = await QueuedTask.Run(() =>
            {
                var def = _table.GetDefinition();
                return def;
            });

            _count = await QueuedTask.Run(() => {
                var _c = _table.GetCount();
                return _c;
            });

            _cursor = await QueuedTask.Run(() =>
            {
                var cur = _table.Search();
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