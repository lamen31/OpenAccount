using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenAccount.Data
{
    public class HitServices
    {
        public static async Task<string> PostCallAPI(string url, string jsonString)
        {
            string ret = string.Empty;
            try
            {
                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (HttpClient client = new HttpClient(handler))
                    {
                        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(url, content);

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode || response.StatusCode.ToString() == "BadRequest" || response.StatusCode.ToString() == "InternalServerError")
                            {
                                var jsonStringResult = await response.Content.ReadAsStringAsync();
                                return jsonStringResult;
                            }
                            else
                            {
                                return response.StatusCode.ToString();
                            }
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            return ret;
        }

        public async Task<string> GetCallAPI(string url, string jsonString)
        {
            string ret = string.Empty;
            try
            {
                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (HttpClient client = new HttpClient(handler))
                    {
                        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        var response = await client.GetAsync(url);
                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var jsonStringResult = await response.Content.ReadAsStringAsync();
                                return jsonStringResult;
                            }
                            else
                                return response.StatusCode.ToString();
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            return ret;
        }
    }
}
