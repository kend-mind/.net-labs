using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using VPBANK.RMD.Utils.Common;
using VPBANK.RMD.Utils.Common.Shared;

namespace VPBANK.RMD.API.Common.Helpers
{
    public static class HttpClientHelper
    {
        public static string GetAsync(string endPoint, string token, string contentType = MediaTypeNames.Application.Json)
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(endPoint) };
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add(ApiKeys.X_AUTH_TOKEN, token);
                HttpResponseMessage response = client.GetAsync(endPoint).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (!string.IsNullOrEmpty(ex.InnerException?.Message))
                    msg += $"{SpecificSystems.PILE}Inner Exception: " + ex.InnerException.Message;
                throw new Exception("Api error: " + msg + $"{SpecificSystems.PILE}API URL: " + endPoint);
            }
        }

        public static string PostAsync(string endPoint, string token, object data, string contentType = MediaTypeNames.Application.Json)
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(endPoint) };
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add(ApiKeys.X_AUTH_TOKEN, token);
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, contentType);
                HttpResponseMessage response = client.PostAsync(endPoint, content).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (!string.IsNullOrEmpty(ex.InnerException?.Message))
                    msg += $"{SpecificSystems.PILE}Inner Exception: " + ex.InnerException.Message;
                throw new Exception("Api error: " + msg + $"{SpecificSystems.PILE}API URL: " + endPoint);
            }
        }
    }
}
