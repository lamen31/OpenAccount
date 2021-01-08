using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            //var doc = PdfDocument.Load(@"c:\Test\testpdf.pdf");
            //var printDoc = new PdfPrintDocument(doc);
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(@"c:\Test\testpdf.pdf");
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
            doc.PrintSettings.PrintController = new StandardPrintController();
            doc.PrinterName = "Brother HL-L5100DN Series";
            PrintDocument printdoc = doc.PrintDocument;
            printdoc.PrintController = new StandardPrintController();
            printdoc.Print();
            this.Close();
        }
    }
}
