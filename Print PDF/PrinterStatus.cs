using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Dynamic;

namespace Print_PDF
{
    class PrinterStatus
    {
        //Config config = new Config();
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
        //Transaksi trx = new Transaksi();


        public void StatusPrinting(string strnamaprinter)
        {
            process = new Process();
            printername = settings.PrinterName;
            path = Directory.GetCurrentDirectory();
            string pathprinter = "";
                pathprinter = path + "\\" + @"ServerPrinting\printA4\PrintServerA4.exe";
                workingdirectory = Path.GetDirectoryName(pathprinter);
                process.StartInfo.FileName = pathprinter;
            
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = workingdirectory;
            process.Start();
            process.WaitForExit();
            StatusCode = process.ExitCode;
            if (process.HasExited)
            {
                process.Close();
                process.Dispose();
            }
        }
    }
}
