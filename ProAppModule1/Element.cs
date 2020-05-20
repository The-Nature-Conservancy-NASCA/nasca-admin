using System;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;
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

                //_cursor = await QueuedTask.Run(() =>
                //{
                //    var cur = _featureclass.Search();
                //    return cur;
                //});
                
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