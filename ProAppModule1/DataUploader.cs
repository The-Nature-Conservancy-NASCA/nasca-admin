using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace ProAppModule1
{

    public class DataUploader : IUploader

    {
        private Uri _gdb;
        private string token = WebInteraction.GenerateToken("GeoTNCDev", "GeoTNC123");

        private readonly FieldValidator  _fieldValidator;
        private readonly Geoprocessor _geoprocessor;

        public DataUploader(FieldValidator fieldValidator, Geoprocessor geoprocessor) {
            _fieldValidator = fieldValidator;
            _geoprocessor = geoprocessor;
        }

        private async Task<FeatureClassDefinition> GetDefinition(Item item) {
            var path = new Uri(item.Path);
            var directory = new Uri(path, ".");
            var gdbPath = directory.AbsolutePath.Remove(directory.AbsolutePath.Length - 1);
            _gdb = new Uri(gdbPath);


            FeatureClassDefinition definition = await QueuedTask.Run(()=> {
                var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(_gdb));
                var featureclass = geodatabase.OpenDataset<FeatureClass>(item.Title);
                var _definition = featureclass.GetDefinition();
                return _definition;
            });

            return definition;
        }

        private int AddFeatures(List<Object> features, string service) {

            // Create a request for the URL.          
            var url = $"{service}/addFeatures";

            // Serialize the objects to json
            var serializer = new JavaScriptSerializer();
            var jsonFeatures = serializer.Serialize(features);
            var postData = $"features={jsonFeatures}&f=pjson&token={token}";

            // Build request
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            //Debug.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server. 
            // The using block ensures the stream is automatically closed. 
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();

                // Display the content.  
                //Debug.WriteLine(responseFromServer);

                // Desealize content
                var result = serializer.Deserialize<AddResults>(responseFromServer);

                var addResult = result.addResults[0];

                if (addResult.success != null)
                {
                    //objectid = addResult.objectId;
                }
            }
            // Close the response.  
            response.Close();

            return 0;
        }

        private Task<FeatureClass> GetFeatureClass(FeatureClassDefinition def, Item item) {

            var featureClass = QueuedTask.Run(()=> {
                var fc_geom = def.GetShapeType();
                var geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(_gdb));
                var _featureClass = geodatabase.OpenDataset<FeatureClass>(item.Title);
                return _featureClass;
            });

            return featureClass;

        }

       private Task LoadData(FeatureClass featureclass, string class_name, string service, CancelableProgressorSource status, int chunksize) {

            return QueuedTask.Run(() => {

                var featuresList = new List<Object>();
                var webMercator = SpatialReferenceBuilder.CreateSpatialReference(102100);


                using (RowCursor rowCursor = featureclass.Search())
                {
                    while (rowCursor.MoveNext())
                    {
                        using (Row row = rowCursor.Current)
                        {
                            Feature feature = row as Feature;
                            Geometry shape = feature.GetShape();

                            Type type = Type.GetType(class_name);
                            var _attributes = Activator.CreateInstance(type, row);

                            Geometry poly = GeometryEngine.Instance.Project(shape, webMercator);
                            var json_poly = GeometryEngine.Instance.ExportToJSON(JSONExportFlags.jsonExportSkipCRS, poly);

                            var serializer = new JavaScriptSerializer();
                            Point geom;
                            //if (fc_geom == GeometryType.Polygon)
                            //    { }
                            //else
                            geom = serializer.Deserialize<Point>(json_poly);

                            var feat = new { attributes = _attributes, geometry = geom };
                            featuresList.Add(feat);

                            if (featuresList.Count == chunksize)
                            {
                                var _result = AddFeatures(featuresList, service);
                                featuresList.Clear();
                                status.Progressor.Value += 1;
                                //status.Progressor.Status = (status.Progressor.Value * 100 / status.Progressor.Max) + @" % Completed";
                                status.Progressor.Message = String.Format("Registros cargados {0}", status.Progressor.Value*chunksize) ;
                            }
                        }
                    }
                }

                if (featuresList.Count > 0)
                    AddFeatures(featuresList, service);
            }, status.Progressor);


        }


        public async void UploadData(Item item, string class_name, string service)
        {
            // Defensive programmming
            if (item == null)
                throw new NullReferenceException();

            // Get definition and shapetype
            var def = await GetDefinition(item);

            // Validating fields
            var schema = await _fieldValidator.ValidateFields(class_name, def);
            if (schema != true)
            {
                //MessageBox.Show("Invalid data schema");
                //return;
            }

            // Loading data
            //var path = new Uri(item.Path);
            //var steps =  await _geoprocessor.GetCount(path);
            var chunksize = 500;
            var featureClass = await GetFeatureClass(def, item);
            var count = await QueuedTask.Run(() => { return featureClass.GetCount(); });
            var steps = (uint) (count / chunksize + 1);

            using (var progress = new ProgressDialog("Showing Progress", "Canceled", steps, false))
            {
                var status = new CancelableProgressorSource(progress);
                progress.Show();
                
                status.Max = steps;
                await LoadData(featureClass, class_name, service, status, chunksize);

                progress.Hide();
            }
        }
    }
}
