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
        private readonly Random _random = new Random();
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

        public static async Task<string> SendLog(Transaksi trx, Config config, string errorMessage)
        {
            string myLink = config.Read("LINK", Config.PARAM_SERVICES_LINK);
            string myPath = config.Read("LINK", Config.PARAM_SERVICES_LOG);
            string myUrl = myLink + myPath;
            LogData logdata = new LogData {
                jenisTransaksi = trx.pilihanLayanan[trx.jenisLayanan],
                kodeTransaksi = trx.kodeLayanan[trx.jenisLayanan],
                idTransaksi = config.Read("LINK", Config.PARAM_DEVICE_TERMINAL_ID) + DateTime.Now.ToString("ddMMyyyyHHmmss"),
                namaNasabah = trx.namaNasabah,
                noKartu = trx.nomerKartu.Substring(0, 12) + "****",
                noSeriPassbook = trx._BukuSerial,
                saldoBuku = trx._BukuSaldo,
                lineInput = trx._BukuBaris,
                startDate = trx.startDate,
                endDate = trx.endDate,
                idxMonth = trx.periodMonth,
                tglTransaksi = DateTime.Now.AddHours(-7).ToString("s"),
                noRekening = trx._AccountNumber,
                statusTransaksi = trx.statusLayanan,
                smsNotif = trx.smsNotif,
                emailNotif = trx.emailNotif,
                errorMessage = errorMessage,
                externalId = trx.externalID,
            };

            var _jsonSerializerOptions = new JsonSerializerOptions {WriteIndented = true};

            var content = new StringContent(
                JsonSerializer.Serialize(logdata, _jsonSerializerOptions),
                Encoding.UTF8, "application/json");

            return await CallAPI(myUrl, content, "POST");
        }
        public static async Task<string> GetExternalTest(Transaksi trx, Config config, string errorMessage)
        {
            string myLink = config.Read("LINK", Config.PARAM_SERVICES_REPORT);
            string myPath = "log/external";
            string myUrl = myLink + myPath;
            LogData logdata = new LogData
            {
                jenisTransaksi = "5Trx",
                kodeTransaksi = "5trx",
                idTransaksi = "123123",
                namaNasabah = "nana",
                noKartu = "123",
                noSeriPassbook = "",
                saldoBuku = "",
                lineInput = "",
                startDate = "",
                endDate = "",
                idxMonth = "NO",
                tglTransaksi = "2020-12-01",
                noRekening = "123",
                statusTransaksi = "Sukse",
                smsNotif = "success",
                emailNotif = "success",
                errorMessage = "",
                externalId = "20210113154536897",
            };

            var _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

            var content = new StringContent(
                JsonSerializer.Serialize(logdata, _jsonSerializerOptions),
                Encoding.UTF8, "application/json");

            return await CallAPI(myUrl, content, "GET");
        }

        public static async Task<string> SendLogTest(Transaksi trx, Config config, string errorMessage)
        {
            string myLink = config.Read("LINK", Config.PARAM_SERVICES_REPORT);
            string myPath = config.Read("LINK", Config.PARAM_SERVICES_LOG);
            string myUrl = myLink + myPath;
            LogData logdata = new LogData
            {
                jenisTransaksi = "5Trx",
                kodeTransaksi = "5trx",
                idTransaksi = "123123",
                namaNasabah = "nana",
                noKartu = "123",
                noSeriPassbook = "",
                saldoBuku = "",
                lineInput = "",
                startDate = "",
                endDate = "",
                idxMonth = "NO",
                tglTransaksi = "2020-12-01",
                noRekening = "123",
                statusTransaksi = "Sukse",
                smsNotif = "success",
                emailNotif = "success",
                errorMessage = "",
                externalId = trx.externalID,
            };

            var _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

            var content = new StringContent(
                JsonSerializer.Serialize(logdata, _jsonSerializerOptions),
                Encoding.UTF8, "application/json");

            return await CallAPI(myUrl, content, "POST");
        }

        public static async Task<string> InquiryNotification(Transaksi trx, Config config)
        {
            string ret = string.Empty;
            string myLink = config.Read("LINK", Config.PARAM_SERVICES_LINK);
            string myPath = config.Read("LINK", Config.PARAM_SERVICES_INQUIRY_NOTIFICATION);
            string myUrl = myLink + myPath;
            InquiryData logdata = new InquiryData {
                noRekening = trx._AccountNumber,
            };

            var _jsonSerializerOptions = new JsonSerializerOptions {WriteIndented = true, };

            var content = new StringContent(
                JsonSerializer.Serialize(logdata, _jsonSerializerOptions),
                Encoding.UTF8, "application/json");

            return await CallAPI(myUrl, content, "POST");
        }

        public static async Task<string> SendEmail(Transaksi trx, Config config)
        {
            string myLink = config.Read("LINK", Config.PARAM_SERVICES_LINK);
            string myPath = config.Read("LINK", Config.PARAM_SERVICES_EMAIL);
            string myUrl = myLink + myPath;
            if (!RegexUtilities.IsValidEmail(trx.emailNasabah))
            {
                return "Format Email Tidak Valid";
            }
            EmailData emaildata = new EmailData
            {
                emailNasabah = trx.emailNasabah,
                jenisTransaksi = trx.pilihanLayanan[trx.jenisLayanan],
                namaNasabah = trx.namaNasabah,
                noRekening = trx._AccountNumber2,
                statusTransaksi = trx.statusLayanan,
                lampiran = trx.emailAttachment,
                path = trx.attachmentPath,
            };
            Utility.WriteLog("Hit Service condition : request JSON --> " + emaildata, "step-action");
            var _jsonSerializerOptions = new JsonSerializerOptions {WriteIndented = true, };

            var content = new StringContent(
                JsonSerializer.Serialize(emaildata, _jsonSerializerOptions),
                Encoding.UTF8, "application/json");

            return await CallAPI(myUrl, content, "POST");
        }

        public static async Task<string> SendEmailAttachment(Transaksi trx, Config config)
        {
            string myLink = config.Read("LINK", Config.PARAM_SERVICES_LINK);
            string myUrl = myLink + "notif/emailattachment";
            if (!RegexUtilities.IsValidEmail(trx.emailNasabah))
            {
                return "Format Email Tidak Valid";
            }
            EmailData emaildata = new EmailData
            {
                emailNasabah = trx.emailNasabah,
                jenisTransaksi = trx.pilihanLayanan[trx.jenisLayanan],
                namaNasabah = trx.namaNasabah,
                noRekening = trx._AccountNumber2,
                statusTransaksi = trx.statusLayanan,
                lampiran = trx.emailAttachment,
                path = trx.attachmentPath,
            };
            Utility.WriteLog("Hit Service condition : request JSON --> " + emaildata, "step-action");
            var _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true, };

            var content = new StringContent(
                JsonSerializer.Serialize(emaildata, _jsonSerializerOptions),
                Encoding.UTF8, "application/json");

            return await CallAPI(myUrl, content, "POST");
        }
        public static async Task<string> SendEmailReport(Reports reports, Config config)
        {
            //string error;
            //string errorcode;
            //string errormessage;
            //try
            //{
                string myLink = config.Read("LINK", Config.PARAM_SERVICES_REPORT);
                string myUrl = myLink + "notif/emailreport";
            //if (!RegexUtilities.IsValidEmail(trx.emailNasabah))
            //{
            //    return "Format Email Tidak Valid";
            //}
            Console.WriteLine("lampiran " + reports.CSVName);
            Console.WriteLine("path " + reports.CSVPath);
            EmailData emaildata = new EmailData
                {
                    emailNasabah = "",
                    jenisTransaksi = "Attachement Report",
                    namaNasabah = "",
                    noRekening = "0",
                    statusTransaksi = "Sukses",
                    lampiran = reports.CSVName,
                    path = reports.CSVPath,
                };
            Utility.WriteLog("Hit Service condition : request JSON --> " + emaildata, "step-action");
            var _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true, };

                var content = new StringContent(
                    JsonSerializer.Serialize(emaildata, _jsonSerializerOptions),
                    Encoding.UTF8, "application/json");

                return await CallAPI(myUrl, content, "POST");
            //}
            //catch(Exception ex)
            //{
            //    //tambahan
            //    errorcode = "ReportError";
            //    errormessage = "SendDataReportError";
            //    error = ex.Message;
            //    //return;
            //    trx.reportStatus = "FAILED";
            //    return errormessage;
            //}
        }
        public static async Task<string> CopyReport(Transaksi trx, Config config)
        {
            //string error;
            //string errorcode;
            //string errormessage;
            //try
            //{
            string myLink = config.Read("LINK", Config.PARAM_SERVICES_REPORT);
            string myUrl = myLink + "notif/emailreport";
            //if (!RegexUtilities.IsValidEmail(trx.emailNasabah))
            //{
            //    return "Format Email Tidak Valid";
            //}
            CopyData copydata = new CopyData
            {
                filename = trx.reportPath,
            };

            var _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true, };

            var content = new StringContent(
                JsonSerializer.Serialize(copydata, _jsonSerializerOptions),
                Encoding.UTF8, "application/json");

            return await CallAPI(myUrl, content, "POST");
            //}
            //catch(Exception ex)
            //{
            //    //tambahan
            //    errorcode = "ReportError";
            //    errormessage = "SendDataReportError";
            //    error = ex.Message;
            //    //return;
            //    trx.reportStatus = "FAILED";
            //    return errormessage;
            //}
        }

        public static async Task<string> SendSms(Transaksi trx, Config config)
        {
            string myLink = config.Read("LINK", Config.PARAM_SERVICES_LINK);
            string myPath = config.Read("LINK", Config.PARAM_SERVICES_SMS);
            string myUrl = myLink + myPath;
            SmsData smsdata = new SmsData {
                jenisTransaksi = trx.pilihanLayanan[trx.jenisLayanan],
                namaNasabah = trx.namaNasabah,
                noRekening = trx._AccountNumber2,
                msisdn = trx.MSISDN,
                statusTransaksi = trx.statusLayanan,
            };
            Utility.WriteLog("Hit Service condition : request JSON --> " + smsdata, "step-action");
            var _jsonSerializerOptions = new JsonSerializerOptions {WriteIndented = true, };

            var content = new StringContent(
                JsonSerializer.Serialize(smsdata, _jsonSerializerOptions),
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
                            return "Invalid Supported HTTP Method";
                        }
                        
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
    public class InquiryData
    {
        public string noRekening { get; set; }
    }
}
