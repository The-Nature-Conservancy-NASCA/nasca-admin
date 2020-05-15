using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace ProAppModule1

{
    class WebInteraction
    {

        static string token = GenerateToken("GeoTNCDev", "GeoTNC123");
 
        public static string GenerateToken(string user, string pass)
        {

            string token = null;
            var serializer = new JavaScriptSerializer();

            // Create a request for the URL.          
            var url = "https://arcgis.com/sharing/rest/generateToken";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";

            var postData = $"username={user}&password={pass}&referer=https://www.arcgis.com&f=json";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;   

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Debug.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server. 
            // The using block ensures the stream is automatically closed. 
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();

                // Display the content.  
                Debug.WriteLine(responseFromServer);

                // Desealize content
                var result = serializer.Deserialize<TokenResult>(responseFromServer);

                token = result.token;

            }

            // Close the response.  
            response.Close();
            return token;
        }

        public static Object Query(string service, string where, string outFields)

        {
            Object features;
            var operation = "query";
            // Create a request for the URL.          
            var format = "pjson";
            var url = $"{service}/{operation}?where={where}&outFields={outFields}&f={format}&token={token}";
            WebRequest request = WebRequest.Create(url);

            // If required by the server, set the credentials.  
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Debug.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server. 
            // The using block ensures the stream is automatically closed. 
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Debug.WriteLine(responseFromServer);

                JavaScriptSerializer ser = new JavaScriptSerializer();
                var dict = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(responseFromServer);
                features = dict["features"];

            }

            // Close the response.  
            response.Close();
            return features;
        }

        public static void DeleteFeatures(string service, int objectid)

        {

            // Create a request for the URL.    
            var operation = "deleteFeatures";
            var url = $"{service}/{operation}";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            
            var format = "json";
            var postData = $"objectids={objectid}&f={format}&token={token}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Debug.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server. 
            // The using block ensures the stream is automatically closed. 
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Debug.WriteLine(responseFromServer);

            }

            // Close the response.  
            response.Close();
            return;
        }


        public static int AddFeatures(string service, Object _attributes)

        {

            int objectid = 0;
            var operation = "addFeatures";

            // Create a request for the URL.          
            var url = $"{service}/{operation}";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";

            var _features = new { attributes = _attributes };

            var serializer = new JavaScriptSerializer();
            var features = "[" + serializer.Serialize(_features) + "]";

            var format = "pjson";
            var postData = $"features={features}&f={format}&token={token}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Debug.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server. 
            // The using block ensures the stream is automatically closed. 
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();

                // Display the content.  
                Debug.WriteLine(responseFromServer);

                // Desealize content
                var result = serializer.Deserialize<AddResults>(responseFromServer);
                var addResult = result.addResults[0];
                if (addResult.success != null)
                {

                    objectid = addResult.objectId;

                }

            }

            // Close the response.  
            response.Close();

            return objectid;

        }


        public static int AddFeatures(string service, Object _attributes,  Rings geom)

        {
            int objectid = 0;
            var operation = "addFeatures";

            // Create a request for the URL.          
            var url = $"{service}/{operation}";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";

            var serializer = new JavaScriptSerializer();

            var _features = new { attributes = _attributes, geometry = geom };

            var features = "[" + serializer.Serialize(_features) + "]";

            var format = "pjson";
            var postData = $"features={features}&f={format}&token={token}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Debug.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server. 
            // The using block ensures the stream is automatically closed. 
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();

                // Display the content.  
                Debug.WriteLine(responseFromServer);

                // Desealize content
                var result = serializer.Deserialize<AddResults>(responseFromServer);

                var addResult = result.addResults[0];

                if (addResult.success != null)
                {

                    objectid = addResult.objectId;

                }

            }

            // Close the response.  
            response.Close();

            return objectid;

        }

        public static int AddFeatures(string service, Object _attributes, Point geom)

        {
            int objectid = 0;
            var operation = "addFeatures";

            // Create a request for the URL.          
            var url = $"{service}/{operation}";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";

            var serializer = new JavaScriptSerializer();

            var _features = new { attributes = _attributes, geometry = geom };

            var features = "[" + serializer.Serialize(_features) + "]";

            var format = "pjson";
            var postData = $"features={features}&f={format}&token={token}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Debug.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server. 
            // The using block ensures the stream is automatically closed. 
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();

                // Display the content.  
                Debug.WriteLine(responseFromServer);

                // Desealize content
                var result = serializer.Deserialize<AddResults>(responseFromServer);

                var addResult = result.addResults[0];

                if (addResult.success != null)
                {

                    objectid = addResult.objectId;

                }

            }

            // Close the response.  
            response.Close();

            return objectid;

        }

        public static int UpdateFeatures(string service, int objectid, Object _attributes)

        {

            // Create a request for the URL.          
            var operation = "updateFeatures";
            var url = $"{service}/{operation}";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";

            var _features = new { attributes = _attributes };

            var serializer = new JavaScriptSerializer();
            var features = "[" + serializer.Serialize(_features) + "]";

            var format = "pjson";
            var postData = $"features={features}&f={format}&token={token}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Debug.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server. 
            // The using block ensures the stream is automatically closed. 
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();

                // Display the content.  
                Debug.WriteLine(responseFromServer);

                // Desealize content
                var result = serializer.Deserialize<UpdateResults>(responseFromServer);

                var updateResult = result.updateResults[0];

                if (updateResult.success != null)
                {

                    objectid = updateResult.objectId;

                }

            }

            // Close the response.  
            response.Close();

            return objectid;

        }



        public static int AddAttachment(string service, int objectid, string file)

        {

            // Create a request for the URL.          
            var operation = "addAttachment";
            var url = $"{service}/{objectid}/{operation}";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";

            var serializer = new JavaScriptSerializer();

            var format = "json";
            var postData = $"attachment={file}&f={format}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Debug.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server. 
            // The using block ensures the stream is automatically closed. 
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();

                // Display the content.  
                Debug.WriteLine(responseFromServer);

                // Desealize content
                var result = serializer.Deserialize<AttachmentResults>(responseFromServer);


                if (result.success != null)
                {

                    objectid = result.objectId;

                }

            }

            // Close the response.  
            response.Close();

            return objectid;

        }

    }

    public class Result
    {

        public int objectId { get; set; }

        public object globalId { get; set; }

        public string success { get; set; }

        public object error { get; set; }

    }

    public class AttachmentResults
    {
        public int objectId { get; set; }
        public string success { get; set; }


    }


    public class UpdateResults
    {
        public Result[] updateResults { get; set; }

    }

    public class AddResults
    {

        public Result[] addResults { get; set; }

    }

    public class TokenResult {
        public string token { get; set; }

    }

    public class Rings {

        public double[][][] rings { get; set; }

    }

    public class Point
    {

        public double x { get; set; }
        public double y { get; set; }

    }

    public class Curves
    {

        public double[][][] curveRings { get; set; }

    }

    public class WebFeature
    {
        private object _attributes;
        private object _geometry;

        public WebFeature(Object attributes, Object geometry) {
            _attributes = attributes;
            _geometry = geometry;
        }

    }
}
