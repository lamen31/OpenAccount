using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Printing;

namespace OpenAccount.Data
{
    public class Printer
    {
        PrintDocument printdoc = new PrintDocument();
        Transaksi _trx = new Transaksi();
        Config config = new Config();
        Font font = new Font("Calibri", 8, FontStyle.Regular);
        string printername;

        public void BeginPrintEH(object sender, PrintEventArgs e)
        {
            SolidBrush blackbrush = new SolidBrush(Color.Black);
        }

        public void EndPrintEH(object sender, PrintEventArgs e)
        {
            SolidBrush blackbrush = new SolidBrush(Color.Black);
            blackbrush.Dispose();
        }

        public string PrintHistori(Transaksi trx)
        {
            printdoc = new PrintDocument();
            _trx = trx;
            PrinterSettings settings = new PrinterSettings();
            printername = settings.PrinterName;
            printdoc.PrinterSettings.PrinterName = printername;
            printdoc.BeginPrint += new PrintEventHandler(BeginPrintEH);
            printdoc.EndPrint += new PrintEventHandler(EndPrintEH);
            printdoc.PrintPage += new PrintPageEventHandler(HistoriPrintPage);
            printdoc.Print();
            return _trx._HistoriSaldo;
        }

        public void HistoriPrintPage(object sender, PrintPageEventArgs e)
        {
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            Graphics g = e.Graphics;

            string pathlogo = config.Read("PATH", Config.PARAM_PATH_IMAGE_A4);
            Image img = Image.FromFile(pathlogo);
            g.DrawImage(img, new Point(50, 30));

            string cabang = "Bank BRI Cabang Cikupa";
            string nasabah = _trx._Nasabah[0];
            string alamat = _trx._Nasabah[2];

            g.DrawString("Nama Nasabah", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(50, 130));
            g.DrawString("Alamat Nasabah", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(50, 150));

            g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(170, 130));
            g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(170, 150));

            g.DrawString(cabang, new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(50, 110));
            g.DrawString(nasabah, new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(180, 130));
            g.DrawString(alamat, new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(180, 150));

            string periode = _trx._HistoriStartDate + " s/d " + _trx._HistoriEndDate;
            string norekening = _trx._Nasabah[1];
            string jumlah = _trx._HistoriJumlahTransaksi + " transaksi";

            g.DrawString("Periode", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(350, 90));
            g.DrawString("Nomor Rekening", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(350, 110));
            g.DrawString("Jenis Tabungan", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(350, 130));
            g.DrawString("Jumlah Transaksi", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(350, 150));

            g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(470, 90));
            g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(470, 110));
            g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(470, 130));
            g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(470, 150));

            g.DrawString(periode, new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(480, 90));
            g.DrawString(norekening, new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(480, 110));
            g.DrawString("Taplus", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(480, 130));
            g.DrawString(jumlah, new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(480, 150));

            g.DrawString("Uraian", new Font("Arial", 12, FontStyle.Regular), blackBrush, new Point(50, 200));
            g.DrawString("Tipe", new Font("Arial", 12, FontStyle.Regular), blackBrush, new Point(350, 200));
            g.DrawString("Nominal", new Font("Arial", 12, FontStyle.Regular), blackBrush, new Point(450, 200));
            g.DrawString("Saldo", new Font("Arial", 12, FontStyle.Regular), blackBrush, new Point(600, 180));
            g.DrawString("Akhir", new Font("Arial", 12, FontStyle.Regular), blackBrush, new Point(600, 200));

            int saldo = Convert.ToInt32(_trx._HistoriSaldo);
            int ypoint = 220;
            font = new Font("Arial", 10, FontStyle.Regular);
            for (int i = 0; i < _trx._HistoriUraian.Length; i++)
            {
                int ypoint1 = ypoint;
                int nominal = int.Parse(_trx._HistoriNominal[i]);
                if (_trx._HistoriUraian[i].Length >= 40)
                {
                    string[] arrayhsl = stringSplit(_trx._HistoriUraian[i]);
                    for (int j = 0; j < arrayhsl.Length; j++)
                    {
                        g.DrawString(arrayhsl[j], font, blackBrush, new Point(50, ypoint1));
                        ypoint1 += 20;
                    }
                }
                else
                {
                    g.DrawString(_trx._HistoriUraian[i], font, blackBrush, new Point(50, ypoint));
                    ypoint1 += 20;
                }
                g.DrawString(_trx._HistoriEndDate, font, blackBrush, new Point(50, ypoint1));
                g.DrawString(_trx._HistoriHourDate, font, blackBrush, new Point(130, ypoint1));
                ypoint1 += 40;
                g.DrawString(_trx._HistoriTipe[i], font, blackBrush, new Point(350, ypoint));
                g.DrawString(nominal.ToString("N0"), font, blackBrush, new Point(450, ypoint));
                g.DrawString(saldo.ToString("N0"), font, blackBrush, new Point(600, ypoint));
                ypoint = ypoint1;
                if (_trx._HistoriTipe[i] == "D")
                    saldo -= nominal;
                else
                    saldo += nominal;
            }
            ypoint += 10;
            string halaman = "halaman ke " + _trx._HistoriHalaman + " dari " + _trx._HistoriMaxHalaman;
            g.DrawString(halaman, new Font("Calibri", 10, FontStyle.Regular), blackBrush, new Point(600, ypoint));
            _trx._HistoriSaldo = saldo.ToString();
        }

        public string[] stringSplit(string strtext)
        {
            string[] strarray = strtext.Split().ToArray();
            string temp = string.Empty;
            int j = 0;
            int pos = 0;
            int n = strtext.Length / 40;
            if (strtext.Length % 40 > 0)
            {
                n += 1;
            }
            string[] arrayhasil = new string[n];
            string[] temparray = null;
            for (int i = 0; i < n; i++)
            {
                while (j < strarray.Length)
                {
                    if (j == 0)
                    {
                        temp = temp + strarray[j];
                        pos = j;
                    }
                    else
                    {
                        temp = temp + " " + strarray[j];
                        pos = j;
                    }
                    if (temp.Length < 40)
                    {
                        arrayhasil[i] = temp;
                    }
                    else
                    {
                        j += strarray.Length;
                    }
                    j++;
                }

                if ((strarray.Length - pos) != 0)
                    temparray = new string[strarray.Length - pos];
                else
                    temparray = new string[1];
                Array.Copy(strarray, pos, temparray, 0, strarray.Length - pos);
                strarray = temparray;
                pos = 0;
                j = 0;
                temp = string.Empty;
            }
            return arrayhasil;
        }
    }
}
