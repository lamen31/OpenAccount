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
        Process process = new Process();
        PrinterSettings settings = new PrinterSettings();
        private string printername;
        public string workingdirectory;

        public void StatusPrinting(string strnamaprinter)
        {
            process = new Process();
            printername = settings.PrinterName;
            pathA4 = config.Read("PATH", Config.PARAM_PATH_PRINTSERVER_A4);
            pathPassbook = config.Read("PATH", Config.PARAM_PATH_PRINTSERVER_PASSBOOK);
            pathPrintCoba = config.Read("PATH", Config.PARAM_PATH_PRINTSERVER_PRINTCOBA);
            if (strnamaprinter == printername)
            {
                workingdirectory = Path.GetDirectoryName(pathA4);
                process.StartInfo.FileName = pathA4;
            }
            else if(strnamaprinter == "PsiPR-OLI")
            {
                workingdirectory = Path.GetDirectoryName(pathPassbook);
                process.StartInfo.FileName = pathPassbook;
            }
            else if(strnamaprinter== "EPSON L3150 Series")
            {
                workingdirectory = Path.GetDirectoryName(pathPrintCoba);
                process.StartInfo.FileName = pathPrintCoba;
            }
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = workingdirectory;
            process.Start();
            Utility.WriteLog("Printer status condition : check status printing in " + strnamaprinter, "step-action");
            process.WaitForExit();
            if (process.HasExited)
            {
                process.Close();
                process.Dispose();
            }
        }
    }
}
