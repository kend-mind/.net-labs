using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Utils.Common.Shared;

namespace VPBANK.RMD.API.Common.Helpers
{
    public static class APIHelper
    {
        public static string Get(string uri, string token)
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.Timeout = 15 * 60 * 1000;
                request.ReadWriteTimeout = 15 * 60 * 1000;
                request.Headers.Add(ApiKeys.X_AUTH_TOKEN, token);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (!string.IsNullOrEmpty(ex.InnerException?.Message))
                    msg += $"{SpecificSystems.PILE}Inner Exception: " + ex.InnerException.Message;
                throw new Exception("Api error: " + msg + $"{SpecificSystems.PILE}API URL: " + uri);
            }
        }

        public static byte[] Post(string uri, string token, string data, string contentType = MediaTypeNames.Application.Json, string method = API_METHODS.POST)
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                byte[] dataBytes = string.IsNullOrEmpty(data) ? null : Encoding.UTF8.GetBytes(data);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                if (dataBytes != null)
                {
                    request.ContentLength = dataBytes.Length;
                }
                request.ContentType = string.IsNullOrEmpty(contentType) ? null : contentType;
                request.Method = method;
                request.Timeout = 15 * 60 * 1000;
                request.ReadWriteTimeout = 15 * 60 * 1000;
                request.Headers.Add(ApiKeys.X_AUTH_TOKEN, token);

                if (dataBytes != null)
                {
                    using (Stream requestBody = request.GetRequestStream())
                    {
                        requestBody.Write(dataBytes, 0, dataBytes.Length);
                    }
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    //return reader.ReadToEnd();
                    var ms = new MemoryStream();
                    reader.BaseStream.CopyTo(ms);
                    return ms.GetBuffer();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (!string.IsNullOrEmpty(ex.InnerException?.Message))
                    msg += $"{SpecificSystems.PILE}Inner Exception: " + ex.InnerException.Message;
                throw new Exception("Api error: " + msg + $"{SpecificSystems.PILE}API URL: " + uri);
            }
        }
    }
}
