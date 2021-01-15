using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Dynamic;

namespace OpenAccount.Data
{
    public class PrinterStatus
    {
        Config config = new Config();
        private string pathA4;
        private string pathPassbook;
        private string pathPrintCoba;
        private string pathThermal;
        Process process = new Process();
        PrinterSettings settings = new PrinterSettings();
        private string printername;
        public string workingdirectory;
        public int StatusCode;
        private string path = string.Empty;

        private string pathStatus = string.Empty;
        private string strfilename = "step-action";
        private string textStatus = string.Empty;
        Transaksi trx = new Transaksi();

        public void checkPrinterA4()
        {
            printername = settings.PrinterName;
            Utility.WriteLog("Printer condition : check status printing start", "step-action");
            StatusPrinting(printername);
            pathStatus = workingdirectory;
            pathStatus = pathStatus + "\\logs\\logs" + DateTime.Now.ToString("yyyyMM") + "\\" + strfilename + DateTime.Now.ToString("yyMMdd-HH") + ".txt";
            textStatus = Utility.ReadLog(pathStatus);
            Utility.WriteLog(textStatus, "step-action");
            Utility.ClearLog(pathStatus);
            Utility.WriteLog("Printer condition : log has been moved from " + pathStatus, "step-action");
            trx.setStatusPrinting(StatusCode.ToString());
        }

        public void checkPrinterPassbook()
        {
            printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_PASSBOOK);
            Utility.WriteLog("Printer condition : check status printing start", "step-action");
            StatusPrinting(printername);
            pathStatus = workingdirectory;
            pathStatus = pathStatus + "\\logs\\logs" + DateTime.Now.ToString("yyyyMM") + "\\" + strfilename + DateTime.Now.ToString("yyMMdd-HH") + ".txt";
            textStatus = Utility.ReadLog(pathStatus);
            Utility.WriteLog(textStatus, "step-action");
            Utility.ClearLog(pathStatus);
            Utility.WriteLog("Printer condition : log has been moved from " + pathStatus, "step-action");
            trx.setStatusPrinting(StatusCode.ToString());
        }

        public void StatusPrinting(string strnamaprinter)
        {
            process = new Process();
            printername = settings.PrinterName;
            path = Directory.GetCurrentDirectory();
            string pathprinter = "";
            pathPrintCoba = config.Read("PATH", Config.PARAM_PATH_PRINTSERVER_PRINTCOBA);
            if (strnamaprinter == config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_A4))
            {
                pathprinter = path + "\\" + config.Read("PATH", Config.PARAM_PATH_PRINTSERVER_A4);
                workingdirectory = Path.GetDirectoryName(pathprinter);
                process.StartInfo.FileName = pathprinter;
            }
            else if(strnamaprinter == config.Read("PRINTERNAME",Config.PARAM_PRINTERNAME_PASSBOOK))
            {
                pathprinter = path + "\\" + config.Read("PATH", Config.PARAM_PATH_PRINTSERVER_PASSBOOK);
                workingdirectory = Path.GetDirectoryName(pathprinter);
                process.StartInfo.FileName = pathprinter;
            }
            else if(strnamaprinter== config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_THERMAL))
            {
                pathprinter = path + "\\" + config.Read("PATH", Config.PARAM_PATH_PRINTSERVER_THERMAL);
                workingdirectory = Path.GetDirectoryName(pathprinter);
                process.StartInfo.FileName = pathprinter;
            }
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = workingdirectory;
            Utility.WriteLog("Printer status condition : check status printing in " + strnamaprinter, "step-action");
            process.Start();
            process.WaitForExit();
            StatusCode = process.ExitCode;
            Utility.WriteLog("Printer status condition : check status printing done", "step-action");
            if (process.HasExited)
            {
                Utility.WriteLog("Printer status condition : check status printing close", "step-action");
                process.Close();
                process.Dispose();
            }
        }
    }
}
