using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Controls.WinForms;
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
            var doc = PdfDocument.Load(@"c:\Test\testpdf.pdf");
            var printDoc = new PdfPrintDocument(doc);
            PrintController printController = new StandardPrintController();
            printDoc.PrintController = printController;
            printDoc.PrinterSettings.PrinterName = "Brother HL-L5100DN Series";
            printDoc.Print(); // Print PDF document
            this.Close();
        }
    }
}
