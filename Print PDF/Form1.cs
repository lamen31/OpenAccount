<<<<<<< HEAD
﻿using Microsoft.Win32;
using Spire.Pdf;
=======
﻿using Spire.Pdf;
>>>>>>> origin/newfeature
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
<<<<<<< HEAD
using System.Diagnostics;
=======
>>>>>>> origin/newfeature
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Print_PDF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
<<<<<<< HEAD
            //var doc = PdfDocument.Load(@"c:\Test\testpdf.pdf");
            //var printDoc = new PdfPrintDocument(doc);
            //PdfDocument doc = new PdfDocument();
            //doc.LoadFromFile(@"c:\Test\testpdf.pdf");
            //PrintController printController = new StandardPrintController();
            //printDoc.PrintController = printController;
            //printDoc.PrinterSettings.PrinterName = "Brother HL-L2360D series";
            //printDoc.Print(); // Print PDF document

            //PrintDialog dialogPrint = new PrintDialog();
            //dialogPrint.AllowPrintToFile = true;
            //dialogPrint.AllowSomePages = true;
            //dialogPrint.PrinterSettings.MinimumPage = 1;
            //dialogPrint.PrinterSettings.MaximumPage = doc.Pages.Count;
            //dialogPrint.PrinterSettings.FromPage = 1;
            //dialogPrint.PrinterSettings.ToPage = doc.Pages.Count;
            //if (dialogPrint.ShowDialog() == DialogResult.OK)
            //{
            //    doc.PrintFromPage = dialogPrint.PrinterSettings.FromPage;
            //    doc.PrintToPage = dialogPrint.PrinterSettings.ToPage;
            //    doc.PrinterName = "Brother HL-L2360D series"    ;
            //    PrintDocument printDoc = doc.PrintDocument;
            //    printDoc.Print();
            //}
            //doc.PrintSettings.PrintController = new StandardPrintController();
            //doc.PrinterName = "Brother HL-L5100DN Series";
            //PrintDocument printdoc = doc.PrintDocument;
            //printdoc.PrintController = new StandardPrintController();
            //printdoc.Print();
            string pathToPdf = @"c:\Test\testpdf.pdf";
            //string printerName = "EPSON L3150 Series";
            string printerName = "Brother HL-L2360D series";

            var exePath = Registry.LocalMachine.OpenSubKey(
            @"SOFTWARE\Microsoft\Windows\CurrentVersion" +
            @"\App Paths\SumatraPDF.exe").GetValue("").ToString();

            //var exePath = Registry.LocalMachine.OpenSubKey(
            //@"SOFTWARE\Microsoft\Windows\CurrentVersion" +
            //@"\App Paths\SumatraPDF-3.2-64.exe").GetValue("").ToString();

            //Console.WriteLine(exePath);

            var args = $"-print-to \"{printerName}\" {pathToPdf}";

            var process = Process.Start(exePath, args);
            process.WaitForExit();

            PrinterStatus status = new PrinterStatus();
            string pathStatus;
            string strfilename = "step-action";
            string textStatus;
            int printStatus;
            Task.Delay(500);
            status.StatusPrinting(printerName);
            //pathStatus = status.workingdirectory;
            //pathStatus = pathStatus + "\\logs\\logs" + DateTime.Now.ToString("yyyyMM") + "\\" + strfilename + DateTime.Now.ToString("yyMMdd-HH") + ".txt";
            //textStatus = Utility.ReadLog(pathStatus);
            //Utility.WriteLog(textStatus, "step-action");
            //Utility.ClearLog(pathStatus);
            //Utility.WriteLog("Printer condition : log has been moved from " + pathStatus, "step-action");
            printStatus = status.StatusCode;
            Console.WriteLine("STATUS CODE " + printStatus.ToString());
            switch (printStatus)
            {
                case 0:
                    {
                        //Utility.WriteLog("Printer condition : print success", "step-action");
                        Console.WriteLine("print success");
                        break;
                    }
                case 1:
                    {
                        //Utility.WriteLog("Printer condition : printer has a paper problem", "step-action");
                        Console.WriteLine("printer has a paper problem");
                        break;
                    }
                case 2:
                    {
                        //Utility.WriteLog("Printer condition : printer is out of toner", "step-action");
                        Console.WriteLine("printer is out of toner");
                        break;
                    }
                case 3:
                    {
                        //Utility.WriteLog("Printer condition : printer is in an error state", "step-action");
                        Console.WriteLine("pprinter is in an error state");
                        break;
                    }
                case 4:
                    {
                        //Utility.WriteLog("Printer condition : printer has a paper jam", "step-action");
                        Console.WriteLine("rinter has a paper jam");
                        break;
                    }
                case 5:
                    {
                        //Utility.WriteLog("Printer condition : printer is out of paper", "step-action");
                        Console.WriteLine("printer is out of paper");
                        break;
                    }
                case 6:
                    {
                        //Utility.WriteLog("Printer condition : printer is off line", "step-action");
                        Console.WriteLine("printer is off line");
                        break;
                    }
                case 7:
                    {
                        //Utility.WriteLog("Printer condition : printer is out of memory", "step-action");
                        Console.WriteLine("printer is out of memory");
                        break;
                    }
                case 8:
                    {
                        //Utility.WriteLog("Printer condition : printer is low on toner", "step-action");
                        Console.WriteLine("printer is low on toner");
                        break;
                    }
            }
=======
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(@"c:\Test\testpdf.pdf");
            doc.PrintSettings.PrinterName = "Brother HL-L5100DN Series";

            PrintDocument printdoc = new PrintDocument();

            //printdoc.DocumentName = @"c:\Test\testpdf.pdf";
            //var doc = printdoc.Load(@"c:\Test\testpdf.pdf");
            //var printDoc = new PdfPrintDocument(doc);
            PrintController printController = new StandardPrintController();
            printdoc.PrintController = printController;
            printdoc.PrinterSettings.PrinterName = "Brother HL-L5100DN Series";
            //printdoc.PrinterSettings.PrinterName = "Brother HL-L5100DN Series";
            printdoc.Print(); // Print PDF document
>>>>>>> origin/newfeature
            this.Close();
        }
    }
}
