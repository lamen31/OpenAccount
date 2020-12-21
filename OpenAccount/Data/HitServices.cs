using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

        public static async Task<string> InquiryNotification(Transaksi trx, Config config)
        {
            string ret = string.Empty;
            string myLink = config.Read("LINK", Config.PARAM_SERVICES_LINK);
            string myPath = config.Read("LINK", Config.PARAM_SERVICES_INQUIRY_NOTIFICATION);
            string myUrl = myLink + myPath;
            InquiryData logdata = new InquiryData();
            logdata.noRekening = trx._AccountNumber;
            var _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var content = new StringContent(
                JsonSerializer.Serialize(logdata, _jsonSerializerOptions),
                Encoding.UTF8, "application/json");

            return await CallAPI(myUrl, content, "POST");
        }

        public static async Task<string> CallAPI(string url, HttpContent payload, string method="POST")
        {
            string ret = string.Empty;
            try
            {
                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (HttpClient client = new HttpClient(handler))
                    {
                        var response = new HttpResponseMessage();
                        if (method == "POST")
                        {
                            response = await client.PostAsync(url, payload);
                        }
                        else if (method == "GET")
                        {
                            response = await client.GetAsync(url);
                        }
                        else
                        {
                            return "Invalid HTTP Method";
                        }
                        
                        //----------------Prepared to send Transaction Log-----------------//
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


        public class InquiryData
        {
            public string noRekening { get; set; }
        }
    }
}
