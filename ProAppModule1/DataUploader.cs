using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using MessageBox = ArcGIS.Desktop.Framework.Dialogs.MessageBox;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using System.Windows;

namespace ProAppModule1
{
    public class DataUploader : IUploader
    {
        private Uri _gdb;
        private readonly string token = WebInteraction.GenerateToken("GeoTNCDev", "GeoTNC123");
        private SpatialReference webMercator = SpatialReferenceBuilder.CreateSpatialReference(102100);


        private readonly FieldValidator  _fieldValidator;
        private readonly Geoprocessor _geoprocessor;
        public DataUploader(FieldValidator fieldValidator, Geoprocessor geoprocessor) {
            _fieldValidator = fieldValidator;
            _geoprocessor = geoprocessor;
        }

        private int AddFeatures(List<Object> features, Element element) {

            // Create a request for the URL.          
            var url = $"{element.service}/addFeatures";

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
            //Debug.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server. 
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //Debug.WriteLine(responseFromServer);

                // Deserialize content
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

       private Task LoadToService(Element element, CancelableProgressorSource status, int chunksize) {

            return QueuedTask.Run(() => {

                var featuresList = new List<Object>();
                using (RowCursor rowCursor = element.cursor)
                {
                    while (rowCursor.MoveNext())
                    {
                        using (Row row = rowCursor.Current)
                        {

                            Object feat;

                            // Read attributes
                            var _attributes = element.FormatAttributes(row);

                            // Read and convert geometry
                            if (element.item.Type == "File Geodatabase Feature Class" || element.item.Type == "Shapefile")
                            {
                                var feature = row as Feature;
                                var shape = feature.GetShape();
                                var shape_prj = GeometryEngine.Instance.Project(shape, webMercator);
                                var json_geom = GeometryEngine.Instance.ExportToJSON(JSONExportFlags.jsonExportSkipCRS, shape_prj);
                                var geom = element.Serialize(json_geom);
                                feat = new { attributes = _attributes, geometry = geom };
                            }
                            else if (element.item.Type == "File Geodatabase Table" || element.item.Type == "Excel Table" || element.item.Type == "Text File")
                                feat = new {attributes = _attributes};
                            else
                                feat = new { }; // Maybe Throw a exception?

                            // Add feature
                            featuresList.Add(feat);

                            // Evaluate size
                            if (featuresList.Count == chunksize)
                            {
                                var _result = AddFeatures(featuresList, element);
                                featuresList.Clear();
                                status.Progressor.Value += 1;
                                status.Progressor.Status = String.Format("{0} de {1}", status.Progressor.Value * chunksize, element.count);
                                status.Progressor.Message = String.Format("Registros cargados {0}", status.Progressor.Value*chunksize) ;
                            }
                        }
                    }
                }

                if (featuresList.Count > 0)
                {
                    AddFeatures(featuresList, element);
                    status.Progressor.Value += 1;
                    status.Progressor.Message = String.Format("Registros cargados {0}", status.Progressor.Value * chunksize);
                }
            }, status.Progressor);
        }


        public async void UploadData(Element element)
        {
            try
            {
                // Defensive programmming
                if (element.item == null)
                    throw new NullReferenceException();

                element.Initialization(element.index);

                // Get definition and shapetype
                await element.GetProperties();

                // Validating fields
                //var schema = await _fieldValidator.ValidateFields(class_name, def);
                //if (schema != true)
                //{
                //    //MessageBox.Show("Invalid data schema");
                //    //return;
                //}

                // Loading data
                int chunksize;
                if (element.count > 100)
                    chunksize = 500;
                else
                {
                    chunksize = 10;
                }

                var steps = (uint)(element.count / chunksize + 1);

                using (var progress = new ProgressDialog("Cargando datos", "Canceled", steps, false))
                {
                    var status = new CancelableProgressorSource(progress);
                    progress.Show();
                    status.Max = steps;
                    await LoadToService(element, status, chunksize);
                    progress.Hide();
                }


                var n = element.count; // Review the real number of loaded records

                if (n > 0)
                    MessageBox.Show(string.Format("Registros cargados: {0}", n), "Resultado de cargue", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("No se cargó ningún registro", "Resultado de cargue", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error al Cargar los datos de Proyecto {0}", ex.ToString()), "Cargue de datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

        }
    }
}
