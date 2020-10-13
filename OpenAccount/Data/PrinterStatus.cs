using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;

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
        string path;

        public void StatusPrinting(string strnamaprinter)
        {
            process = new Process();
            printername = settings.PrinterName;
            path = Directory.GetCurrentDirectory();
            string pathprinter = "";
            pathPrintCoba = config.Read("PATH", Config.PARAM_PATH_PRINTSERVER_PRINTCOBA);
            if (strnamaprinter == printername)
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
