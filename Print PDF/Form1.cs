//using Patagames.Pdf.Net;
//using Patagames.Pdf.Net.Controls.WinForms;
//using Spire;
//using Spire.Pdf;
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
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(@"c:\Test\testpdf.pdf");
            doc.PrinterName = "Brother HL-L5100DN Series";

            PrintDocument printdoc = new PrintDocument();

            //printdoc.DocumentName = @"c:\Test\testpdf.pdf";
            //var doc = printdoc.Load(@"c:\Test\testpdf.pdf");
            //var printDoc = new PdfPrintDocument(doc);
            PrintController printController = new StandardPrintController();
            printdoc.PrintController = printController;
            //printdoc.PrinterSettings.PrinterName = "Brother HL-L5100DN Series";
            printdoc.Print(); // Print PDF document
            this.Close();
        }
    }
}
