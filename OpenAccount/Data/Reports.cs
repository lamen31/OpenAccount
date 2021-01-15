using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenAccount.Data
{
    public class Reports
    {
        //private string startdate = DateTime.Now.ToString("yyyy-MM-dd");
        //private string enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

        //private string startdate = "2020-12-28";
        //private string enddate = "2020-12-29";

        public string startdate;
        public string enddate;
        public string enddateName;

        public string CSVName = string.Empty;
        public string CSVPath = string.Empty;

        private class ReportData
        {
            public string externalId { get; set; }
            public string id { get; set; }
            public string tglTransaksi { get; set; }
            public string namaNasabah { get; set; }
            public string noRekening { get; set; }
            public string noKartu { get; set; }
            public string noSeriPassbook { get; set; }
            public string statusTransaksi { get; set; }
            public string jenisTransaksi { get; set; }
            public string kodeTransaksi { get; set; }
            public string idTransaksi { get; set; }
            public string idxMonth { get; set; }
            public string emailNotif { get; set; }
            public string lineInput { get; set; }
            public string saldoBuku { get; set; }
            public string smsNotif { get; set; }
            public string startDate { get; set; }
            public string endDate { get; set; }
            public string errorMessage { get; set; }

        }

        private List<ReportData> _listReport = new List<ReportData>();

        private void addListReport(string strexternalid, string strid, string strtgltransaksi, string strnamanasabah, string strnorekening, string strnokartu, string strnoseripassbook, string strstatustransaksi,
            string strjenistransaki, string strkodetransaksi, string stridtransaksi, string stridxmonth, string stremailnotif, string strlineinput, string strsaldobuku, string strsmsnotif, string strstartdate,
            string strenddate, string strerromessage)
        {
            ReportData report = new ReportData();
            report.externalId = strexternalid;
            report.id = strid;
            report.tglTransaksi = strtgltransaksi;
            report.namaNasabah = strnamanasabah;
            report.noRekening = strnorekening;
            report.noKartu = strnokartu;
            report.noSeriPassbook = strnoseripassbook;
            report.statusTransaksi = strstatustransaksi;
            report.jenisTransaksi = strjenistransaki;
            report.kodeTransaksi = strkodetransaksi;
            report.idTransaksi = stridtransaksi;
            report.idxMonth = stridxmonth;
            report.emailNotif = stremailnotif;
            report.lineInput = strlineinput;
            report.saldoBuku = strsaldobuku;
            report.smsNotif = strsmsnotif;
            report.startDate = strstartdate;
            report.endDate = strenddate;
            report.errorMessage = strerromessage;
            _listReport.Add(report);
        }

        Config config = new Config();
        Transaksi trx = new Transaksi();

        private async Task GetReport()
        {
            string errorcode;
            string errormessage;
            try
            {
                string myJson = "{" +
                "\"startDate\":\"" + startdate + "\"," +
                "\"endDate\":\"" + enddate + "\"" +
                "}";
                string myLink = config.Read("LINK", Config.PARAM_SERVICES_REPORT);
                string myUrl = myLink + @"/log/daily";

                Utility.WriteLog("Histori condition : post service start", "step-action");
                try
                {
                    string strResult = await HitServices.PostCallAPI(myUrl, myJson);
                    if (strResult != null)
                    {
                        JObject jobResult = JObject.Parse(strResult);

                        if ((string)jobResult["responseCode"] == "0000")
                        {
                            foreach(var response in jobResult["listReport"].Select((response) => (response)))
                            {
                                addListReport((string)response.SelectToken("externalId"), (string)response.SelectToken("id"), (string)response.SelectToken("tglTransaksi"), (string)response.SelectToken("namaNasabah"),
                                    (string)response.SelectToken("noRekening"), (string)response.SelectToken("noKartu"), (string)response.SelectToken("noSeriPassbook"), (string)response.SelectToken("statusTransaksi"),
                                    (string)response.SelectToken("jenisTransaksi"), (string)response.SelectToken("kodeTransaksi"), (string)response.SelectToken("idTransaksi"), (string)response.SelectToken("idxMonth"),
                                    (string)response.SelectToken("emailNotif"), (string)response.SelectToken("lineInput"), (string)response.SelectToken("saldoBuku"), (string)response.SelectToken("smsNotif"),
                                    (string)response.SelectToken("startDate"), (string)response.SelectToken("endDate"), (string)response.SelectToken("errorMessage"));
                            }
                            trx.reportStatus = "SUCCESS";
                            Console.WriteLine("GAIN DATA REPORT SUCCESS");
                        }
                        else
                        {
                            //tambahan
                            errorcode = "ReportError";
                            errormessage = "GainDataReportError";
                            //return;
                            trx.reportStatus = "FAILED";
                            Console.WriteLine("GAIN DATA REPORT FAILED");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //tambahan
                    errorcode = "ReportError";
                    errormessage = ex.Message;
                    //return;
                    trx.reportStatus = "FAILED";
                    Console.WriteLine(ex.Message);
                }
            }
             catch (Exception ex)
            {
                //tambahan
                errorcode = "ReportError";
                errormessage = ex.Message;
                //return;
                trx.reportStatus = "FAILED";
                Console.WriteLine(ex.Message);
            }
        }

        public async Task createReportCSV()
        {
            string errorcode;
            string errormessage;
            await GetReport();

            if (trx.reportStatus == "SUCCESS")
            {
                string path = Directory.GetCurrentDirectory();
                CSVName = "TRILOGI" + "_REPORT_" + startdate + "_" + enddateName + ".csv";
                CSVPath =  @"c:\Reports\" + CSVName;

                // Set the variable "delimiter" to ", ".
                string delimiter = ", ";

                //This text is added only once to the file.
                if (!File.Exists(CSVPath))
                {
                    File.Delete(CSVPath);
                    // Create a file to write to.
                    string createText = "extenal_id" + delimiter + "id" + delimiter + "no_rekening" + delimiter + "no_seri_passbook" + delimiter +
                    "no_kartu" + delimiter + "nama_nasabah" + delimiter + "jenis_transaksi" + delimiter +
                    "status_transaksi" + delimiter + "error_message" + delimiter + "tgl_transaksi" + delimiter +
                    "kode_transaksi" + delimiter + "id_transaksi" + delimiter + "idx_month" + delimiter +
                    "email_notif" + delimiter + "line_input" + delimiter + "saldo_buku" + delimiter +
                    "sms_notif" + delimiter + "start_date" + delimiter + "end_date" + delimiter + Environment.NewLine;
                File.WriteAllText(CSVPath, createText);
                Console.WriteLine("CREATE REPORT HEADER SUCCESS");
                }
                foreach (var report in _listReport)
                {
                    string appendText = report.externalId + delimiter + report.id + delimiter + report.noRekening + delimiter + report.noSeriPassbook + delimiter +
                        report.noKartu + delimiter + report.namaNasabah + delimiter + report.jenisTransaksi + delimiter +
                        report.statusTransaksi + delimiter + report.errorMessage + delimiter + report.tglTransaksi + delimiter +
                        report.kodeTransaksi + delimiter + report.idTransaksi + delimiter + report.idxMonth + delimiter +
                        report.emailNotif + delimiter + report.lineInput + delimiter + report.saldoBuku + delimiter +
                        report.smsNotif + delimiter + report.startDate + delimiter + report.endDate + delimiter + Environment.NewLine;
                    File.AppendAllText(CSVPath, appendText);
                }
                Console.WriteLine("CREATE REPORT SUCCESS");
                trx.reportAttachment = CSVName;
                trx.reportPath = CSVPath;
            }
            else
            {
                //tambahan
                errorcode = "ReportError";
                errormessage = "GainDataReportError";
                //return;
                trx.reportStatus = "FAILED";
            }
        }
    }
}
