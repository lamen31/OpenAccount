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
    public class SendNotification
    {
        public static async Task<string> SendEmail(Transaksi trx, Config config)
        {
            string ret = string.Empty;
            try
            {
                string myLink = config.Read("LINK", Config.PARAM_SERVICES_LINK);
                string myPath = config.Read("LINK", Config.PARAM_SERVICES_EMAIL);
                string myUrl = myLink + myPath;
                if (!RegexUtilities.IsValidEmail(trx.emailNasabah))
                {
                    return "Format Email Tidak Valid";
                }
                EmailData emaildata = new EmailData();
                emaildata.emailNasabah = trx.emailNasabah;
                emaildata.jenisTransaksi = trx.pilihanLayanan[trx.jenisLayanan];
                emaildata.namaNasabah = trx.namaNasabah;
                emaildata.noRekening = trx._AccountNumber;
                emaildata.statusTransaksi = trx.statusLayanan;
                emaildata.lampiran = trx.emailAttachment;

                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (HttpClient client = new HttpClient(handler))
                    {
                        //----------------Prepared to send Transaction Log-----------------//
                        var _jsonSerializerOptions = new JsonSerializerOptions
                        {
                            WriteIndented = true
                        };
                        var content = new StringContent(
                            JsonSerializer.Serialize(emaildata, _jsonSerializerOptions),
                            Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(myUrl, content);
                        
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

        public static async Task<string> SendSms(Transaksi trx, Config config)
        {
            string ret = string.Empty;
            try
            {
                string myLink = config.Read("LINK", Config.PARAM_SERVICES_LINK);
                string myPath = config.Read("LINK", Config.PARAM_SERVICES_SMS);
                string myUrl = myLink + myPath;
                SmsData smsdata = new SmsData();
                smsdata.jenisTransaksi = trx.pilihanLayanan[trx.jenisLayanan];
                smsdata.namaNasabah = trx.namaNasabah;
                smsdata.noRekening = trx._AccountNumber;
                smsdata.msisdn = trx.MSISDN;

                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (HttpClient client = new HttpClient(handler))
                    {
                        //----------------Prepared to send Transaction Log-----------------//
                        var _jsonSerializerOptions = new JsonSerializerOptions
                        {
                            WriteIndented = true
                        };
                        var content = new StringContent(
                            JsonSerializer.Serialize(smsdata, _jsonSerializerOptions),
                            Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(myUrl, content);

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

        public static async Task<string> SendSmsOtp(Transaksi trx, Config config)
        {
            string ret = string.Empty;
            try
            {
                string myLink = config.Read("LINK", Config.PARAM_SERVICES_LINK);
                string myPath = config.Read("LINK", Config.PARAM_SERVICES_SMS_OTP);
                string myUrl = myLink + myPath;
                SmsData smsdata = new SmsData();
                smsdata.jenisTransaksi = trx.pilihanLayanan[trx.jenisLayanan];
                smsdata.namaNasabah = trx.namaNasabah;
                smsdata.noRekening = trx._AccountNumber;
                smsdata.msisdn = trx.MSISDN;

                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (HttpClient client = new HttpClient(handler))
                    {
                        //----------------Prepared to send Transaction Log-----------------//
                        var _jsonSerializerOptions = new JsonSerializerOptions
                        {
                            WriteIndented = true
                        };
                        var content = new StringContent(
                            JsonSerializer.Serialize(smsdata, _jsonSerializerOptions),
                            Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(myUrl, content);

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
    }

    public class EmailData
    {
        public string namaNasabah { get; set; }
        public string emailNasabah { get; set; }
        public string emailNasamahCC { get; set; }
        public string noRekening { get; set; }
        public string jenisTransaksi { get; set; }
        public string statusTransaksi { get; set; }
        public string lampiran { get; set; }
    }

    public class SmsData
    {
        public string namaNasabah { get; set; }
        public string msisdn { get; set; }
        public string noRekening { get; set; }
        public string jenisTransaksi { get; set; }
        public string produk { get; set; }
        public string divisi { get; set; }
    }
}
