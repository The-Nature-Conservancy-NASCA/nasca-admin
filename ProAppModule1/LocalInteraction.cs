using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;
using System.Windows;

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
                    MessageBox.Show($"El campo {name} no fue encontrado en el elemento seleccionado.", "Validación de campos", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                string name;
                if (property.Name.Length > 10)
                    name = property.Name.Substring(0, 10);
                else
                    name = property.Name;
                int index = table_def.FindField(name);
                if (index < 0)
                {
                    MessageBox.Show($"El campo {name} no fue encontrado en el elemento seleccionado.", "Validación de campos", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show($"El campo {name} no fue encontrado en el elemento seleccionado.", "Validación de campos", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        public static async Task UploadFeatureClass(ArcGIS.Desktop.Core.Item SelectedItem, string service, string ClassName)
        {

            var progressDlg = new ProgressDialog("Leyendo datos del Feature class seleccionado", "Cancelar", false);
            progressDlg.Show();
            var progressSrc = new CancelableProgressorSource(progressDlg);

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
                //var item = Activator.CreateInstance(t);
                //FeatureClassDefinition fc_def = featureclass.GetDefinition();
                //var _fc_geom = fc_def.GetShapeType();
                //var schema = ValidateFields(item, fc_def);
                //if (schema != true)
                //{
                //    progressDlg.Hide();
                //    return;
                //}
            });

            var conversionProcess = await QueuedTask.Run(() => {

                string outPath = Project.Current.HomeFolderPath;
                //string outPath = "in_memory";
                string toolPath = "conversion.FeatureClassToShapefile";
                var parameters = Geoprocessing.MakeValueArray(SelectedItem.Path, outPath);
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);
                var result = Geoprocessing.ExecuteToolAsync(toolPath, parameters, environments, new CancelableProgressorSource(progressDlg).Progressor, GPExecuteToolFlags.Default);
                return result;

            });
            progressDlg.Hide();

            int n = 0;
            await QueuedTask.Run(() =>
            {
                Uri shp_path = new System.Uri(conversionProcess.Values[0]);

                var shapefilePath = new FileSystemConnectionPath(shp_path, FileSystemDatastoreType.Shapefile);
                var shapefile = new FileSystemDatastore(shapefilePath);

                var table = shapefile.OpenDataset<Table>(SelectedItem.Title);

                ///Review this
                Uri path = new System.Uri(SelectedItem.Path);
                Uri directory = new Uri(path, ".");

                var gdbPath = directory.AbsolutePath.Remove(directory.AbsolutePath.Length - 1);
                Uri gdb = new Uri(gdbPath);
                var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(gdb));
                FeatureClass featureclass = geodatabase.OpenDataset<FeatureClass>(SelectedItem.Title);
                FeatureClassDefinition fc_def = featureclass.GetDefinition();
                GeometryType fc_geom = fc_def.GetShapeType();
                ///


                using (RowCursor rowCursor = table.Search())
                {
                    while (rowCursor.MoveNext())
                    {
                        using (Row row = rowCursor.Current)
                        {
                            Feature feature = row as Feature;
                            Geometry shape = feature.GetShape();

                            Type t = Type.GetType(ClassName);
                            var _attributes = Activator.CreateInstance(t, row);
                            string _rings;
                            if (fc_geom == GeometryType.Polygon)
                                _rings = GetRings(shape);
                            else
                                _rings = GetRings(shape);

                            var serializer = new JavaScriptSerializer();
                            //var geom = serializer.Deserialize<Rings>(_rings);
                            var geom = serializer.Deserialize<Point>(_rings);

                            WebInteraction.AddFeatures(service, _attributes, geom);
                            n += 1;
                        }
                    }
                }
            });

            progressDlg.Hide();

            if (n > 0)
                MessageBox.Show(string.Format("Registros cargados: {0}", n), "Resultado de cargue", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("No se cargó ningún registro", "Resultado de cargue", MessageBoxButton.OK, MessageBoxImage.Warning);

        }



        public static async Task UploadShapefile(ArcGIS.Desktop.Core.Item SelectedItem, string service, string ClassName)
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

            if (n>0)
                MessageBox.Show(string.Format("Registros cargados: {0}", n), "Resultado de cargue", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("No se cargó ningún registro", "Resultado de cargue", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        public static async Task UploadTable(ArcGIS.Desktop.Core.Item SelectedItem, string service, string ClassName)
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

            if (n > 0)
                MessageBox.Show(string.Format("Registros cargados: {0}", n), "Resultado de cargue", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("No se cargó ningún registro", "Resultado de cargue", MessageBoxButton.OK, MessageBoxImage.Warning);
        }


        public static async Task UploadExcel(ArcGIS.Desktop.Core.Item SelectedItem, string service, string ClassName)
        {

            var progressDlg = new ProgressDialog("Leyendo datos del archivo Excel seleccionado", "Cancelar", false);
            progressDlg.Show();
            var progressSrc = new CancelableProgressorSource(progressDlg);

            var conversionProcess = await QueuedTask.Run(() => {

                string outPath = Project.Current.DefaultGeodatabasePath;
                string outName = string.Format("Tabla_{0}", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                string outhTable = System.IO.Path.Combine(outPath, outName);
                string toolPath = "conversion.ExcelToTable";
                var parameters = Geoprocessing.MakeValueArray(SelectedItem.PhysicalPath, outhTable);
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
                try
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
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("Ha ocurrido un error al convertir el archivo de entrada {0}", e));
                }

            });

            progressDlg2.Hide();

            if (n > 0)
                MessageBox.Show(string.Format("Registros cargados: {0}", n), "Resultado de cargue", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("No se cargó ningún registro", "Resultado de cargue", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

    }
}
