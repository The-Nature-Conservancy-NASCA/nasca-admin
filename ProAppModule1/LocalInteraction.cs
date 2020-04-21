using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ProAppModule1

{
    
    class LocalInteraction
    {

        public static bool ValidateFields(Object item, TableDefinition table_def) {

            PropertyInfo[] properties = item.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var name = property.Name;
                int index = table_def.FindField(name);
                if (index < 0)
                {
                    MessageBox.Show($"El campo {name} no fue encontrado en el elemento seleccionado.");
                    return false;
                }
            }
            return true;
        }

        public static bool ValidateFieldsShapefile(Object item, TableDefinition table_def)
        {
            PropertyInfo[] properties = item.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var name = property.Name.Substring(0,10);
                int index = table_def.FindField(name);
                if (index < 0)
                {
                    MessageBox.Show($"El campo {name} no fue encontrado en el elemento seleccionado.");
                    return false;
                }
            }
            return true;
        }

        public static bool ValidateFields(Object item, FeatureClassDefinition fc_def)
        {
            PropertyInfo[] properties = item.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var name = property.Name;
                int index = fc_def.FindField(name);
                if (index < 0)
                {
                    MessageBox.Show($"El campo {name} no fue encontrado en el archivo seleccionado.");
                    return false;
                }
            }
            return true;
        }

        public static string GetRings(ArcGIS.Core.Geometry.Geometry shape)
        {

            SpatialReference webMercator = SpatialReferenceBuilder.CreateSpatialReference(102100);
            ArcGIS.Core.Geometry.Geometry poly = GeometryEngine.Instance.Project(shape, webMercator);
            var rings = GeometryEngine.Instance.ExportToJSON(JSONExportFlags.jsonExportSkipCRS, poly);
            return rings;
        }

        public static async void UploadFeatureClass(ArcGIS.Desktop.Core.Item SelectedItem, string service, string ClassName) {

            var progressDlg = new ProgressDialog("Leyendo datos del Feature class seleccionado", "Cancelar", false);
            progressDlg.Show();
            var progressSrc = new CancelableProgressorSource(progressDlg);

            int n = 0;
            await QueuedTask.Run(() =>
            {
                Uri path = new System.Uri(SelectedItem.Path);
                Uri directory = new Uri(path, ".");

                var gdbPath = directory.AbsolutePath.Remove(directory.AbsolutePath.Length - 1);
                Uri gdb = new Uri(gdbPath);
                var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(gdb));
                FeatureClass featureclass = geodatabase.OpenDataset<FeatureClass>(SelectedItem.Title);

                // validate fields
                Type t = Type.GetType(ClassName);
                var item = Activator.CreateInstance(t);
                FeatureClassDefinition fc_def = featureclass.GetDefinition();
                var schema = ValidateFields(item, fc_def);
                if (schema != true)
                {
                    progressDlg.Hide();
                    return;
                }
                   
                using (RowCursor rowCursor = featureclass.Search())
                {
                    while (rowCursor.MoveNext())
                    {
                        using (Row row = rowCursor.Current)
                        {
                            Feature feature = row as Feature;
                            Geometry shape = feature.GetShape();
                            var _attributes = Activator.CreateInstance(t, row);
                            var _rings = GetRings(shape);
                            var serializer = new JavaScriptSerializer();
                            var geom = serializer.Deserialize<Rings>(_rings);
                            WebInteraction.AddFeatures(service, _attributes, geom);
                            n += 1;
                        }
                    }
                }
            });
            progressDlg.Hide();
            MessageBox.Show(string.Format("Registros cargados: {0}", n));
        }


        public static async void UploadShapefile(ArcGIS.Desktop.Core.Item SelectedItem, string service, string ClassName)
        {

            var progressDlg = new ProgressDialog("Leyendo datos del Shapefile seleccionado", "Cancelar", false);
            progressDlg.Show();
            var progressSrc = new CancelableProgressorSource(progressDlg);

            int n = 0;
            await QueuedTask.Run(() =>
            {
                Uri path = new System.Uri(SelectedItem.Path);
                Uri directory = new Uri(path, ".");

                var shapefilePath = new FileSystemConnectionPath(directory, FileSystemDatastoreType.Shapefile);
                var shapefile = new FileSystemDatastore(shapefilePath);
                var table = shapefile.OpenDataset<Table>(SelectedItem.Title);
                
                // validate fields
                Type t = Type.GetType(ClassName);
                var item = Activator.CreateInstance(t);
                TableDefinition table_def = table.GetDefinition();
                var schema = ValidateFieldsShapefile(item, table_def);
                if (schema != true)
                {
                    progressDlg.Hide();
                    return;
                }

                using (RowCursor rowCursor = table.Search())
                {
                    while (rowCursor.MoveNext())
                    {
                        using (Row row = rowCursor.Current)
                        {
                            Feature feature = row as Feature;
                            Geometry shape = feature.GetShape();

                            var _attributes = Activator.CreateInstance(t, row);
                            var _rings = GetRings(shape);

                            var serializer = new JavaScriptSerializer();
                            var geom = serializer.Deserialize<Rings>(_rings);

                            WebInteraction.AddFeatures(service, _attributes, geom);
                            n += 1;
                        }
                    }
                }
            });

            progressDlg.Hide();
            MessageBox.Show(string.Format("Registros cargados: {0}", n));

        }

        public static async void UploadTable(ArcGIS.Desktop.Core.Item SelectedItem, string service, string ClassName)
        {

            var progressDlg = new ProgressDialog("Leyendo datos de la tabla seleccionada", "Cancelar", false);
            progressDlg.Show();
            var progressSrc = new CancelableProgressorSource(progressDlg);

            int n = 0;
            await QueuedTask.Run(() =>
            {
            Uri path = new System.Uri(SelectedItem.Path);
            Uri directory = new Uri(path, ".");
            var gdbPath = directory.AbsolutePath.Remove(directory.AbsolutePath.Length - 1);
            Uri gdb = new Uri(gdbPath);

                var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(gdb));
                var table = geodatabase.OpenDataset<Table>(SelectedItem.Title);

                // validate fields
                Type t = Type.GetType(ClassName);
                var item = Activator.CreateInstance(t);
                TableDefinition table_def = table.GetDefinition();
                var schema = ValidateFields(item, table_def);
                if (schema != true)
                {
                    progressDlg.Hide();
                    return;
                }

                using (RowCursor rowCursor = table.Search())
                {
                    while (rowCursor.MoveNext())
                    {
                        using (Row row = rowCursor.Current)
                        {
                            var _attributes = Activator.CreateInstance(t, row);
                            WebInteraction.AddFeatures(service, _attributes);
                            n += 1;
                        }
                    }
                }
            });

            progressDlg.Hide();
            MessageBox.Show(string.Format("Registros cargados: {0}", n));
        }

        public static async void UploadExcel(ArcGIS.Desktop.Core.Item SelectedItem, string service, string ClassName)
        {

            var progressDlg = new ProgressDialog("Leyendo datos del archivo Excel seleccionado", "Cancelar", false);
            progressDlg.Show();
            var progressSrc = new CancelableProgressorSource(progressDlg);

            var conversionProcess = await QueuedTask.Run(() => {

                string outPath = Project.Current.DefaultGeodatabasePath;
                string outName = string.Format("Tabla_{0}", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                string outhTable = System.IO.Path.Combine(outPath, outName);
                string toolPath = "conversion.ExcelToTable";
                var parameters = Geoprocessing.MakeValueArray(SelectedItem.Path, outhTable);
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);
                var result = Geoprocessing.ExecuteToolAsync(toolPath, parameters, environments, new CancelableProgressorSource(progressDlg).Progressor, GPExecuteToolFlags.Default);
                return result;

            });
            progressDlg.Hide();


            var progressDlg2 = new ProgressDialog("Procesando datos desde Tabla (Geodabase)", "Cancelar", false);
            progressDlg2.Show();
            var progressSrc2 = new CancelableProgressorSource(progressDlg2);

            int n = 0;
            await QueuedTask.Run(() =>
            {

                var outTable = conversionProcess.Values[0];
                Uri path = new System.Uri(outTable);
                Uri directory = new Uri(path, ".");
                var gdbPath = directory.AbsolutePath.Remove(directory.AbsolutePath.Length - 1);
                Uri gdb = new Uri(gdbPath);

                var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(gdb));
                var table = geodatabase.OpenDataset<Table>(Path.GetFileName(outTable));

                // validate fields
                Type t = Type.GetType(ClassName);
                var item = Activator.CreateInstance(t);
                TableDefinition table_def = table.GetDefinition();
                var schema = ValidateFields(item, table_def);
                if (schema != true)
                {
                    progressDlg2.Hide();
                    return;
                }

                using (RowCursor rowCursor = table.Search())
                {
                    while (rowCursor.MoveNext())
                    {
                        using (Row row = rowCursor.Current)
                        {
                            var _attributes = Activator.CreateInstance(t, row);
                            WebInteraction.AddFeatures(service, _attributes);
                            n += 1;
                        }
                    }
                }
            });

            progressDlg2.Hide();
            MessageBox.Show(string.Format("Registros cargados: {0}", n));
        }

    }
}
