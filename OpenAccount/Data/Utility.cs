using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Diagnostics;

namespace OpenAccount.Data
{
    public class Utility
    {
        Config config = new Config();
        Process process = new Process();

        public void PrintPdf()
        {
            string pathPrintPDF;
            string workingdirectory;
            string path = Directory.GetCurrentDirectory();
            pathPrintPDF = path + "\\" + "printPDF\\Print PDF.exe";
            workingdirectory = Path.GetDirectoryName(pathPrintPDF);
            process.StartInfo.FileName = pathPrintPDF;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = workingdirectory;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //Utility.WriteLog("Signpad condition : sign pad process running", "step-action");
            process.Start();
            process.WaitForExit();
            //Utility.WriteLog("Signpad condition : sign pad process done", "step-action");
            if (process.HasExited)
            {
                Utility.WriteLog("Print PDF condition : Print PDF process close", "step-action");
                process.Close();
                //process.Dispose();
            }
            //using (var document = PdfDocument.Load(path))
            //{
            //    using (var printDocument = document.CreatePrintDocument())
            //    {
            //        printDocument.PrinterSettings.PrintFileName = "Letter_SkidTags_Report_9ae93aa7-4359-444e-a033-eb5bf17f5ce6.pdf";
            //        printDocument.PrinterSettings.PrinterName = @"EPSOND03466 (L3150 Series)";
            //        printDocument.DocumentName = "Verify_MultiColumn_Report_CanBe_Processed.pdf";
            //        printDocument.PrinterSettings.PrintFileName = "Verify_MultiColumn_Report_CanBe_Processed.pdf";
            //        printDocument.PrintController = new StandardPrintController();
            //        printDocument.Print();
            //    }
            //}
        }

        public static string ReadLog(string strdatapath)
        {
            string result;
            result = File.ReadAllText(strdatapath);
            return result;
        }
        public static void ClearLog(string strdatapath)
        {
            File.WriteAllText(strdatapath, "");
            File.Delete(strdatapath);
        }
        public static void WriteLog(string strmessage, string strfilename)
        {
            string errormessage = string.Empty;
            WriteLog(strmessage, strfilename, ref errormessage);
        }

        private static void WriteLog(string strmessage, string strfilename, ref string strerror)
        {
            strerror = string.Empty;
            try
            {
                string dir = DirectoryFolder() + @"\logs" + DateTime.Now.ToString("yyyyMM");
                CreateDirectory(dir);

                string filename = dir + @"\" + strfilename + DateTime.Now.ToString("yyMMdd-HH") + ".txt";
                //CreateFile(filename);

                string msg = string.Empty;
                WriteToFile(DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " " + strmessage, filename, ref strerror);
            }
            catch (Exception ex)
            {
                strerror = ex.Message;
            }
        }

        private static string DirectoryFolder()
        {
            string dir = Directory.GetCurrentDirectory() + @"\logs";
            CreateDirectory(dir);
            return dir;
        }

        private static void CreateDirectory(string strdirectory)
        {
            if (!Directory.Exists(strdirectory))
                Directory.CreateDirectory(strdirectory);
        }
        private static void CreateFile(string strfilename)
        {
            if (!File.Exists(strfilename))
                File.Create(strfilename);
        }
        private static bool WriteToFile(string strmessage, string strfilename, ref string strerror)
        {
            bool result = false;
            strerror = string.Empty;
            try
            {
                using (StreamWriter streamwriter = new StreamWriter(strfilename, true))
                {
                    streamwriter.WriteLine(strmessage);
                }
                result = true;
            }
            catch (Exception ex)
            {
                strerror = ex.Message;
            }
            return result;
        }

        public static async Task AuditTrail()
        {
            Config config = new Config();
            Transaksi trx = new Transaksi();
            string _myURL = config.Read("LINK", Config.PARAM_SERVICES_LINK);
            string saveURL = config.Read("LINK", Config.PARAM_SERVICES_SAVE);

            //trans.endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            string auditTrail = string.Empty;
            int stepTrail = 1;
            auditTrail = auditTrail + "[ ";
            foreach (dynamic at in trx._auditTrail)
            {
                auditTrail = auditTrail +
                    "{ \"step\" : \"" + stepTrail + "\"," +
                    "\"action\" : \"" + at._action + "\"," +
                    "\"data\" : \"" + at._data + "\"," +
                    "\"result\" : \"" + at._result + "\"" +
                    "},";
                stepTrail += 1;
            }
            auditTrail = auditTrail.Remove(auditTrail.Length - 1);

            string myJson2 = "{ \"transaction\" : " +
                "{ \"transactionId\" : \"" + trx._TransaksiID + "\"," +
                "\"terminalId\" : \"" + trx._TerminalID + "\"," +
                "\"transactionType\" : \"" + trx._JenisTransaksi + "\"," +
                    "\"startTime\" : \"" + trx.startTime.ToString() + "\"," +
                    "\"endTime\" : \"" + trx.endTime.ToString() + "\"," +
                    "\"status\" : \"" + trx._StatusTransaksi + "\"," +
                    "\"description\" : \"" + trx._ErrorCode + "\"," +
                    "}, \"auditTrail\" : " + auditTrail + "]}";
            string myURL2 = _myURL + saveURL;

            //OurUtility.Write_Log("== Request API : " + myJson2, "step-action");
            string strResult2 = await HitServices.PostCallAPI(myURL2, myJson2);
            //OurUtility.Write_Log("== Response API : " + strResult2, "step-action");
        }
    }
}
