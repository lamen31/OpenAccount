using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//using Newtonsoft.Json;

namespace OpenAccount.Data
{
    public class HitLog
    {
        public static async Task<string> SendLog(Transaksi trx, Config config, string errorMessage)
        {
            string ret = string.Empty;
            try
            {
                string myLink = config.Read("LINK", Config.PARAM_SERVICES_LINK);
                string myPath = config.Read("LINK", Config.PARAM_SERVICES_LOG);
                string myUrl = myLink + myPath;
                LogData logdata = new LogData();
                logdata.jenisTransaksi = trx.pilihanLayanan[trx.jenisLayanan];
                logdata.kodeTransaksi = trx.kodeLayanan[trx.jenisLayanan];
                logdata.idTransaksi = trx._TransaksiID;
                logdata.namaNasabah = trx.namaNasabah;
                logdata.noKartu = trx.nomerKartu.Substring(0, 12) + "****";
                logdata.noSeriPassbook = trx._BukuSerial;
                logdata.saldoBuku = trx._BukuSaldo;
                logdata.lineInput = trx._BukuBaris;
                logdata.startDate = trx.startDate;
                logdata.endDate = trx.endDate;
                logdata.idxMonth = trx.periodMonth;
                logdata.tglTransaksi = DateTime.Now.ToString("s");
                logdata.noRekening = trx._AccountNumber;
                logdata.statusTransaksi = trx.statusLayanan;
                logdata.smsNotif = trx.smsNotif;
                logdata.emailNotif = trx.emailNotif;
                logdata.errorMessage = errorMessage;

                using (var handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (HttpClient client = new HttpClient(handler))
                    {
                        var _jsonSerializerOptions = new JsonSerializerOptions
                        {
                            WriteIndented = true
                        };
                        var content = new StringContent(
                            JsonSerializer.Serialize(logdata, _jsonSerializerOptions),
                            Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(myUrl, content);
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
    }
    public class LogData
    {
        public string namaNasabah { get; set; }
        public string noRekening { get; set; }
        public string noKartu { get; set; }
        public string noSeriPassbook { get; set; }
        public string statusTransaksi { get; set; }
        public string jenisTransaksi { get; set; }
        public string kodeTransaksi { get; set; }
        public string idTransaksi { get; set; }
        public string idxMonth { get; set; }
        public string tglTransaksi { get; set; }
        public string emailNotif { get; set; }
        public string lineInput { get; set; }
        public string saldoBuku { get; set; }
        public string smsNotif { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string errorMessage { get; set; }
    }
}
