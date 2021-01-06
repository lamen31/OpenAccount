﻿using System;
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
using OpenAccount.Component.BukaRekening;

namespace OpenAccount.Data
{
    public class Printer
    {
        PrintDocument printdoc = new PrintDocument();
        PrinterStatus printerstatus = new PrinterStatus();
        Transaksi _trx = new Transaksi();
        TransaksiBaru _trxbaru = new TransaksiBaru();
        Config config = new Config();
        
        Font font = new Font("Calibri", 8, FontStyle.Regular);
        string printername;
        private string path = string.Empty;
        private int Status;

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
            string pathStatus;
            string strfilename = "step-action";
            string textStatus;
            //printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_PRINTERCOBA);
            PrinterSettings settings = new PrinterSettings();
            printername = settings.PrinterName;
            printdoc.PrinterSettings.PrinterName = printername;
            printdoc.BeginPrint += new PrintEventHandler(BeginPrintEH);
            printdoc.EndPrint += new PrintEventHandler(EndPrintEH);
            printdoc.PrintPage += new PrintPageEventHandler(HistoriPrintPage);
            Utility.WriteLog("Printer condition : print histori in " + printername + " start", "step-action");
            printdoc.Print();
            Utility.WriteLog("Printer condition : check status printing start", "step-action");
            printerstatus.StatusPrinting(printername);
            pathStatus = printerstatus.workingdirectory;
            pathStatus = pathStatus + "\\logs\\logs" + DateTime.Now.ToString("yyyyMM") + "\\" + strfilename + DateTime.Now.ToString("yyMMdd-HH") + ".txt";
            textStatus = Utility.ReadLog(pathStatus);
            Utility.WriteLog(textStatus, "step-action");
            Utility.ClearLog(pathStatus);
            Utility.WriteLog("Printer condition : log has been moved from " + pathStatus, "step-action");
            Status = printerstatus.StatusCode;
            switch (printerstatus.StatusCode)
            {
                case 0:
                    {
                        Utility.WriteLog("Printer condition : print success", "step-action");
                        trx.AddTrail("PRINT DATA", "", "SUCCESS");
                        break;
                    }
                case 1:
                    {
                        Utility.WriteLog("Printer condition : printer has a paper problem", "step-action");
                        trx.AddTrail("PRINT DATA", "PAPER PROBLEM", "FAILED");
                        break;
                    }
                case 2:
                    {
                        Utility.WriteLog("Printer condition : printer is out of toner", "step-action");
                        trx.AddTrail("PRINT DATA", "OUT OF TONER", "FAILED");
                        break;
                    }
                case 3:
                    {
                        Utility.WriteLog("Printer condition : printer is in an error state", "step-action");
                        trx.AddTrail("PRINT DATA", "ERROR", "FAILED");
                        break;
                    }
                case 4:
                    {
                        Utility.WriteLog("Printer condition : printer has a paper jam", "step-action");
                        trx.AddTrail("PRINT DATA", "PAPER JAM", "FAILED");
                        break;
                    }
                case 5:
                    {
                        Utility.WriteLog("Printer condition : printer is out of paper", "step-action");
                        trx.AddTrail("PRINT DATA", "OUT OF PAPER", "FAILED");
                        break;
                    }
                case 6:
                    {
                        Utility.WriteLog("Printer condition : printer is off line", "step-action");
                        trx.AddTrail("PRINT DATA", "OFFLINE", "FAILED");
                        break;
                    }
                case 7:
                    {
                        Utility.WriteLog("Printer condition : printer is out of memory", "step-action");
                        trx.AddTrail("PRINT DATA", "OUT OF MEMORY", "FAILED");
                        break;
                    }
                case 8:
                    {
                        Utility.WriteLog("Printer condition : printer is low on toner", "step-action");
                        trx.AddTrail("PRINT DATA", "LOW ON TONER", "FAILED");
                        break;
                    }
            }
            trx.setStatusPrinting(Status.ToString());
            return _trx._HistoriSaldo;
        }

        public void HistoriPrintPage(object sender, PrintPageEventArgs e)
        {
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            Graphics g = e.Graphics;

            path = Directory.GetCurrentDirectory();

            string pathlogo = path + "\\wwwroot\\inputopenaccount" + config.Read("PATH", Config.PARAM_PATH_IMAGE_A4);
            Image img = Image.FromFile(pathlogo);
            g.DrawImage(img, new Point(438, 30));

            g.DrawString("LAPORAN TRANSAKSI", new Font("Arial", 22, FontStyle.Regular), blackBrush, new Point(57, 30));

            g.DrawString("Kepada Yth.", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(57, 174));

            g.DrawString("("+_trx.namaNasabah+")", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(57, 227));

            int pointy = 238;
            g.DrawString("Tanggal Laporan", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(393, pointy));
            pointy += 20;
            g.DrawString("Periode Transaksi", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(393, pointy));
            pointy += 20;
            g.DrawString("Halaman", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(393, pointy));

            pointy = 238;
            g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(503, pointy));
            pointy += 20;
            g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(503, pointy));
            pointy += 20;
            g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(503, pointy));

            pointy = 238;
            g.DrawString("2020-11-16 14:50:56", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(510, pointy));
            pointy += 20;
            g.DrawString("2020-05-01 - 2020-05-31", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(503, pointy));
            pointy += 20;
            g.DrawString("1", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(503, pointy));

            pointy = 355;
            g.DrawString("No Rekening", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(57, pointy));
            pointy += 20;
            g.DrawString("No Kartu", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(57, pointy));
            pointy += 20;
            g.DrawString("Nama Produk", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(57, pointy));
            pointy += 20;
            g.DrawString("Valuta", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(57, pointy));

            pointy = 355;
            g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(144, pointy));
            pointy += 20;
            g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(144, pointy));
            pointy += 20;
            g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(144, pointy));

            //string cabang = "Bank BRI Cabang Cikupa";
            //string nasabah = _trx._Nasabah[0];
            //string alamat = _trx._Nasabah[2];

            //g.DrawString("Nama Nasabah", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(50, 130));
            //g.DrawString("Alamat Nasabah", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(50, 150));

            //g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(170, 130));
            //g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(170, 150));

            //g.DrawString(cabang, new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(50, 110));
            //g.DrawString(nasabah, new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(180, 130));
            //g.DrawString(alamat, new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(180, 150));

            //string periode = _trx._HistoriStartDate + " s/d " + _trx._HistoriEndDate;
            //string norekening = _trx._Nasabah[1];
            //string jumlah = _trx._HistoriJumlahTransaksi + " transaksi";

            //g.DrawString("Periode", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(350, 90));
            //g.DrawString("Nomor Rekening", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(350, 110));
            //g.DrawString("Jenis Tabungan", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(350, 130));
            //g.DrawString("Jumlah Transaksi", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(350, 150));

            //g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(470, 90));
            //g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(470, 110));
            //g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(470, 130));
            //g.DrawString(":", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(470, 150));

            //g.DrawString(periode, new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(480, 90));
            //g.DrawString(norekening, new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(480, 110));
            //g.DrawString("Taplus", new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(480, 130));
            //g.DrawString(jumlah, new Font("Arial", 10, FontStyle.Regular), blackBrush, new Point(480, 150));

            //g.DrawString("Uraian", new Font("Arial", 12, FontStyle.Regular), blackBrush, new Point(50, 200));
            //g.DrawString("Tipe", new Font("Arial", 12, FontStyle.Regular), blackBrush, new Point(350, 200));
            //g.DrawString("Nominal", new Font("Arial", 12, FontStyle.Regular), blackBrush, new Point(450, 200));
            //g.DrawString("Saldo", new Font("Arial", 12, FontStyle.Regular), blackBrush, new Point(600, 180));
            //g.DrawString("Akhir", new Font("Arial", 12, FontStyle.Regular), blackBrush, new Point(600, 200));

            int saldo = Convert.ToInt32(_trx._HistoriSaldo);
            int ypoint = 220;
            font = new Font("Arial", 10, FontStyle.Regular);
            for (int i = 0; i < _trx._listhistori.Count; i++)
            {
                int ypoint1 = ypoint;
                int nominal = int.Parse(_trx._listhistori[i]._Nominal);
                string keterangan = _trx._listhistori[i]._Keterangan;
                if (keterangan.Length >= 40)
                {
                    string[] arrayhsl = stringSplit(keterangan);
                    for (int j = 0; j < arrayhsl.Length; j++)
                    {
                        g.DrawString(arrayhsl[j], font, blackBrush, new Point(50, ypoint1));
                        ypoint1 += 20;
                    }
                }
                else
                {
                    g.DrawString(keterangan, font, blackBrush, new Point(50, ypoint));
                    ypoint1 += 20;
                }
                string date = _trx._listhistori[i]._Tanggal;
                string tanggal = date.Substring(0, 10);
                string jam = date.Substring(11, 8);
                g.DrawString(tanggal, font, blackBrush, new Point(50, ypoint1));
                g.DrawString(jam, font, blackBrush, new Point(130, ypoint1));
                ypoint1 += 40;
                g.DrawString(_trx._listhistori[i]._JenisTransaksi, font, blackBrush, new Point(350, ypoint));
                g.DrawString(nominal.ToString("N0"), font, blackBrush, new Point(450, ypoint));
                if (_trx._listhistori[i]._JenisTransaksi == "Debit")
                    saldo -= nominal;
                else
                    saldo += nominal;
                g.DrawString(saldo.ToString("N0"), font, blackBrush, new Point(600, ypoint));
                ypoint = ypoint1;
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




        public async Task PrintThermal(Transaksi trx)
        {
            printdoc = new PrintDocument();
            _trx = trx;
            string pathStatus;
            string strfilename = "step-action";
            string textStatus;
            string printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_THERMAL);
            //string printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_PRINTERCOBA);
            //PrinterSettings settings = new PrinterSettings();
            //string printername = settings.PrinterName;
            printdoc.PrinterSettings.PrinterName = printername;
            printdoc.PrintPage += new PrintPageEventHandler(ThermalPrintPage);
            Utility.WriteLog("Printer condition : print thermal in " + printername + " start", "step-action");
            printdoc.Print();
            Utility.WriteLog("Printer condition : check status printing start", "step-action");
            printerstatus.StatusPrinting(printername);
            pathStatus = printerstatus.workingdirectory;
            pathStatus = pathStatus + "\\logs\\logs" + DateTime.Now.ToString("yyyyMM") + "\\" + strfilename + DateTime.Now.ToString("yyMMdd-HH") + ".txt";
            textStatus = Utility.ReadLog(pathStatus);
            Utility.WriteLog(textStatus, "step-action");
            Utility.ClearLog(pathStatus);
            Utility.WriteLog("Printer condition : log has been moved from " + pathStatus, "step-action");
            //Status = printerstatus.StatusCode;
            //if (printerstatus.StatusCode == 0)
            //{
            //    Console.WriteLine("Print Selesai ...");
            //    Utility.WriteLog("Printer condition : print thermal in " + printername + " finished", "step-action");
            //}
            //else
            //{
            //    Console.WriteLine("Print Gagal ...");
            //    Utility.WriteLog("Printer condition : print thermal in " + printername + " failed", "step-action");
            //}
            Console.WriteLine("Print Selesai ...");
            Utility.WriteLog("Printer condition : print thermal in " + printername + " finished", "step-action");
            trx.setStatusPrinting(Status.ToString());
        }

        public void ThermalPrintPage(object sender, PrintPageEventArgs e)
        {
            _trx._AccountNumber = _trx._AccountNumber2;
            path = Directory.GetCurrentDirectory();
            string logo = path + "\\wwwroot\\inputopenaccount" + config.Read("PATH", Config.PARAM_PATH_IMAGE_THERMAL);
            StringFormat formatLeft = new StringFormat(StringFormatFlags.NoClip);
            StringFormat formatCenter = new StringFormat(formatLeft);
            formatCenter.Alignment = StringAlignment.Center;
            StringFormat formatRight = new StringFormat(formatLeft);
            formatRight.Alignment = StringAlignment.Far;
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            Graphics g = e.Graphics;
            font = new Font("Arial", 7, FontStyle.Regular);
            float point = 4;
            float sizex = 20;
            float sizey = point;
            float offset = 0;
            float lineheight = font.GetHeight() + point;
            SizeF layoutsize = new SizeF(280, lineheight);
            RectangleF layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            Image img = Image.FromFile(logo);

            string doublespasi = "  ";
            string spasi = " ";

            g.DrawImage(img, (e.PageBounds.Width - img.Width) / 2, 0, img.Width, img.Height);
            offset = img.Height + point;
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            string joint = string.Empty;

            sizex = 10;
            layoutsize = new SizeF(260, lineheight);
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            string date = DateTime.Now.ToString("dd/MM/yy");
            joint = "TANGGAL         : " + date;
            g.DrawString(joint, font, blackBrush, layout, formatLeft);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            string ATMID = "861116";
            joint = "ATM ID             : " + ATMID;
            g.DrawString(joint, font, blackBrush, layout, formatLeft);

            offset = 0;
            point = 4;
            offset = img.Height + point;
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            string hour = DateTime.Now.ToString("HH:mm:ss");
            joint = doublespasi + doublespasi + doublespasi + doublespasi + doublespasi 
                + doublespasi + doublespasi + doublespasi + doublespasi + doublespasi 
                + doublespasi + doublespasi + doublespasi + doublespasi + doublespasi 
                + doublespasi + doublespasi + doublespasi + doublespasi + doublespasi
                + doublespasi + doublespasi + doublespasi + doublespasi + doublespasi
                + "WAKTU   : " + hour;
            g.DrawString(joint, font, blackBrush, layout, formatLeft);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            string noref = "16117";
            joint = doublespasi + doublespasi + doublespasi + doublespasi + doublespasi
                + doublespasi + doublespasi + doublespasi + doublespasi + doublespasi
                + doublespasi + doublespasi + doublespasi + doublespasi + doublespasi
                + doublespasi + doublespasi + doublespasi + doublespasi + doublespasi
                + doublespasi + doublespasi + doublespasi + doublespasi + doublespasi
                + "NO. REF : " + noref;
            g.DrawString(joint, font, blackBrush, layout, formatLeft);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            string lokasi = "0386-BRI 1 KANCA RAWAMANGUN";
            joint = "LOKASI             : " + lokasi;
            g.DrawString(joint, font, blackBrush, layout, formatLeft);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            //string nokartu = "522184..0084";
            joint = "NO. REKENING : " + _trx._AccountNumber;
            g.DrawString(joint, font, blackBrush, layout, formatLeft);
            offset += lineheight;
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            sizex = 0;
            layoutsize = new SizeF(280, lineheight);
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            joint = "INFO 5 MUTASI TERAKHIR";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            //joint = "Lokasi : " + lokasi;
            //g.DrawString(joint, font, blackBrush, layout, formatCenter);
            //offset += lineheight;
            //layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            //g.DrawString(date, font, blackBrush, layout, formatCenter);
            //offset += lineheight;
            //offset += lineheight;
            //layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            //joint = "No Rekening : " + _trx._Nasabah[1];
            //g.DrawString(joint, font, blackBrush, layout, formatCenter);
            //offset += lineheight;
            //layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            //joint = "Nama Nasabah : " + _trx._Nasabah[0];
            //g.DrawString(joint, font, blackBrush, layout, formatCenter);
            //offset += lineheight;
            //offset += lineheight;
            //layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            joint = string.Empty;

            //int batastrx = _trx._listthermal.Count - 10;
            int batastrx = 0;
            int ypoint = 190;
            //string joint = string.Empty;
            //int saldo = int.Parse(_trx._ThermalSaldo);
            //for (int i = batastrx; i < _trx._listthermal.Count; i++)
            //{
            //    int nominal = int.Parse(_trx._listthermal[i]._Nominal);
            //    Random rnd = new Random();
            //    if (_trx._listthermal[i]._JenisTransaksi == "D")
            //    {
            //        saldo -= nominal;
            //    }
            //    else
            //    {
            //        saldo += nominal;
            //    }
            //    string spasi = " ";
            //    string nominalprint = nominal.ToString("N0");
            //    string saldoprint = saldo.ToString("N0");
            //    joint = joint + _trx._listthermal[i]._Tanggal + spasi + spasi + _trx._listthermal[i]._JenisTransaksi + spasi + spasi + nominalprint + spasi + spasi + saldoprint;
            //    g.DrawString(joint, font, blackBrush, layout, formatLeft);
            //    offset += lineheight;
            //    layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            //    ypoint += 20;
            //    joint = string.Empty;
            //}

            for (int i = batastrx; i < _trx._listthermal.Count; i++)
            {
                sizex = 50;
                layoutsize = new SizeF(180, lineheight);
                layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
                //int nominal = int.Parse(_trx._listthermal[i]._AMT);
                //Random rnd = new Random();
                //if (_trx._listthermal[i]._JenisTransaksi == "D")
                //{
                //    saldo -= nominal;
                //}
                //else
                //{
                //    saldo += nominal;
                //}
                //string nominalprint = nominal.ToString("N0");
                //string saldoprint = saldo.ToString("N0");
                //joint = joint + _trx._listthermal[i]._Tanggal + spasi + spasi + _trx._listthermal[i]._JenisTransaksi + spasi + spasi + nominalprint + spasi + spasi + saldoprint;
                joint = _trx._listthermal[i]._Date.Substring(0, 5) + "(" + _trx._listthermal[i]._DORC + ")" + spasi + "RP";
                g.DrawString(joint, font, blackBrush, layout, formatLeft);
                int nominal = Convert.ToInt32(_trx._listthermal[i]._AMT.Substring(0, _trx._listthermal[i]._AMT.Length-3));
                joint = nominal.ToString("N0") + ",00";
                g.DrawString(joint, font, blackBrush, layout, formatRight);
                if (i == _trx._listthermal.Count - 1)
                {
                    offset += lineheight;
                    offset += lineheight;
                }
                else
                offset += lineheight;
                layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
                ypoint += 20;
                joint = string.Empty;
            }
            sizex = 0;
            layoutsize = new SizeF(280, lineheight);
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            joint = "KARTU BRI DAPAT BERTRANSAKSI";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            joint = "DI ATM/EDC BERLOGO JARINGAN";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            joint = "ATM BERSAMA, LINK, PRIMA DAN";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            joint = "MAESTRO-CIRRUS MASTERCARD";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            joint = "CALL BRI";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            joint = "14017/1500017/(021)5798400";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
        }

        //YUDHI MULAI UBAH ====================================================================

        public string backst(int jum)
        {
            // fungsi untuk kembali ke margin awal sesuai panjang karakter tercetak pada butab
            String bcksp = "";

            for (int i = 0; i < jum; i++)
            {
                bcksp = bcksp + "\x08";
            }
            return bcksp;
        }


        public string PrintPassbook(Transaksi trx)
        {


            //Yudhi coba 
            LPrinterTest.LPrinter MyPrinter;

            MyPrinter = new LPrinterTest.LPrinter();
            int PTC = 87;  //total karakter passbook
            _trx = trx;

            MyPrinter.Open("printing test");
             
            string saldo;
            int baris = Convert.ToInt32(_trx._BukuBaris);
            string cetak = "";
            int sisabaris = 0;
            int flipflop = 0;

            sisabaris = baris % 30;
            MyPrinter.Print("\n\n\n\n\n");//sela header

            if (_trx._listbuku.Count > 0) // indikator bahwa line tidak 0
            {
                flipflop = 1;
            }

            //print hal pertama 
            for (int k = 1; k < sisabaris; k++)
            {
                
                MyPrinter.Print("\n");
                if (k == 14)
                {
                    MyPrinter.Print("\n\n");
                }
                else if (k > 14)
                {
                    //MyPrinter.Print("\n\n");
                    flipflop = 2;
                }
            }


         

            for (int i = 0; i < _trx._listbuku.Count; i++)
            {
                string keterangan;
                

                string sandi = _trx._listbuku[i]._Sandi;
                //saldo = checkZero(_trx._listbuku[i]._PassbookBalance);
                saldo = _trx._listbuku[i]._PassbookBalance;
                saldo = checkZero(saldo);
                long nominal = Convert.ToInt64(saldo.Substring(0, saldo.Length - 3));
                if (nominal > 1000)
                    saldo = nominal.ToString("N0") + ".00";
                    saldo = String.Format("{0,15}", saldo);

                string debetprint = _trx._listbuku[i]._PassbookDebitAmount;
                debetprint = checkZero(debetprint);
                long debet = Convert.ToInt64(debetprint.Substring(0, debetprint.Length - 3));
                if (debet > 1000)
                    debetprint = debet.ToString("N0") + ".00";
                    debetprint = String.Format("{0,15}", debetprint);

                string kreditprint =  _trx._listbuku[i]._PassbookCreditAmount;
                kreditprint = checkZero(kreditprint);
                long kredit = Convert.ToInt64(kreditprint.Substring(0, kreditprint.Length - 3));
                if (kredit > 1000)
                    kreditprint = kredit.ToString("N0") + ".00";
                    kreditprint = String.Format("{0,15}", kreditprint);

                string bukuDate = _trx._listbuku[i]._PassbookDate;
                bukuDate = bukuDate.Substring(1, bukuDate.Length - 1);
                string line = baris.ToString();
                if (line.Length < 2)
                    line = "0" + line;


                keterangan = _trx._listbuku[i]._PassbookMNECode;

                string tellerID = _trx._listbuku[i]._PassbookTellerID;

                cetak = " " + bukuDate + "   " + sandi + "      " + debetprint + "     " + kreditprint + "  " + line + "       " + saldo +  "  " + keterangan + "  " + tellerID;
                MyPrinter.Print( cetak + "\n" + backst(100));


                if (baris%14 == 0 && flipflop==1)
                {
                    //MyPrinter.Print("\x0C");
                    MyPrinter.Print("\n\n"); // space tengah buku pindah ke hal 2
                    flipflop = 2;
                }
                else if ((baris%29 == 0) && (flipflop ==2))
                {

                    //MyPrinter.Print("\x0C"); //FF kertas 
                    MyPrinter.Print("\n\n\n\n\n"); // header hal 1
                    flipflop = 1;
                }

                baris = baris + 1;


            }

            MyPrinter.Print("\x0C");
            MyPrinter.Close();


            Status = printerstatus.StatusCode;
            if (printerstatus.StatusCode == 0)
            {
                Console.WriteLine("Print Selesai ...");
                Utility.WriteLog("Printer condition : print passbook in " + printername + " finished", "step-action");
            }
            else
            {
                Console.WriteLine("Print Gagal ...");
                Utility.WriteLog("Printer condition : print passbook in " + printername + " failed", "step-action");
            }
            trx.setStatusPrinting(Status.ToString());
            return trx._BukuSaldo;

                                    
        }



        private string checkZero(string strinput)
        {
            string result;
            int startindex = 0;
            bool isMinus = false;
            if (strinput.Substring(0, 1) == "-")
            {
                isMinus = true;
                strinput = strinput.Substring(1, strinput.Length - 1);
            }
            for(int i = 0; i < strinput.Length; i++)
            {
                if (strinput.Substring(i, 1) == "0")
                    startindex += 1;
                else if (strinput.Substring(i, 1) == ".")
                {
                    startindex -= 1;
                    break;
                }
                else
                    break;

            }
            if(isMinus)
                result = "-" + strinput.Substring(startindex, strinput.Length - startindex);
            else
                result = strinput.Substring(startindex, strinput.Length - startindex);
            return result;
        }

        public string PrintPassbookBisnis(Transaksi trx)
        {
            bool result = false;
            int nol = 0;
            printdoc = new PrintDocument();
            _trx = trx;
            string pathStatus;
            string strfilename = "step-action";
            string textStatus;
            string printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_PASSBOOK);
            //string printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_PRINTERCOBA);
            //PrinterSettings settings = new PrinterSettings();
            //string printername = settings.PrinterName;
            printdoc.PrinterSettings.PrinterName = printername;
            printdoc.BeginPrint += new PrintEventHandler(BeginPrintEH);
            printdoc.EndPrint += new PrintEventHandler(EndPrintEH);
            printdoc.PrintPage += new PrintPageEventHandler(PassbookPrintBisnisPage);
            Utility.WriteLog("Printer condition : print passbook in " + printername + " start", "step-action");
            printdoc.Print();
            Utility.WriteLog("Printer condition : check status printing start", "step-action");
            printerstatus.StatusPrinting(printername);
            pathStatus = printerstatus.workingdirectory;
            pathStatus = pathStatus + "\\logs\\logs" + DateTime.Now.ToString("yyyyMM") + "\\" + strfilename + DateTime.Now.ToString("yyMMdd-HH") + ".txt";
            textStatus = Utility.ReadLog(pathStatus);
            Utility.WriteLog(textStatus, "step-action");
            Utility.ClearLog(pathStatus);
            Utility.WriteLog("Printer condition : log has been moved from " + pathStatus, "step-action");
            Status = printerstatus.StatusCode;
            if (printerstatus.StatusCode == 0)
            {
                Console.WriteLine("Print Selesai ...");
                Utility.WriteLog("Printer condition : print passbook in " + printername + " finished", "step-action");
            }
            else
            {
                Console.WriteLine("Print Gagal ...");
                Utility.WriteLog("Printer condition : print passbook in " + printername + " failed", "step-action");
            }
            trx.setStatusPrinting(Status.ToString());
            return trx._BukuSaldo;
        }

        public void PassbookPrintBisnisPage(object sender, PrintPageEventArgs e)
        {
            font = new Font("Calibri", 7.5F, FontStyle.Regular);
            SolidBrush blackbrush = new SolidBrush(Color.Black);
            Graphics g = e.Graphics;
            long saldo = Convert.ToInt64(_trx._BukuSaldo);
            int baris = Convert.ToInt32(_trx._BukuBaris);
            int ypoint = 75;
            //int sisabaris = 13 * baris;
            int sisabaris = 26 * baris;
            if (baris > 8)
            {
                //sisabaris = sisabaris + (12 * 5);
                sisabaris = sisabaris + (24 * 5);
            }
            ypoint += sisabaris;

            for (int i = 0; i < _trx._listbuku.Count; i++)
            {
                //if (i % 2 == 0)
                //{
                    string keterangan = _trx._listbuku[i]._JenisTransaksi;
                    int nominal = Convert.ToInt32(_trx._listbuku[i]._Nominal.Substring(0, _trx._listbuku[i]._Nominal.Length - 2));
                    if (keterangan == "DBT")
                    {
                        string sandi = _trx._listbuku[i]._KodeTransaksi;
                        saldo -= nominal;
                        string debetprint = "-" + nominal.ToString("N0") + ",00";
                        //g.DrawString(sandi, font, blackbrush, new Point(57, ypoint));
                        g.DrawString(debetprint, font, blackbrush, new Point(95, ypoint));
                        g.DrawString(saldo.ToString("N0"), font, blackbrush, new Point(306, ypoint));
                    }
                    else if (keterangan == "KRT")
                    {
                        string sandi = _trx._listbuku[i]._KodeTransaksi;
                        saldo += nominal;
                        string kreditprint = nominal.ToString("N0") + ",00";
                        //g.DrawString(sandi, font, blackbrush, new Point(57, ypoint));
                        g.DrawString(kreditprint, font, blackbrush, new Point(189, ypoint));
                        g.DrawString(saldo.ToString("N0"), font, blackbrush, new Point(306, ypoint));
                    }
                    //g.DrawString(baris.ToString(), font, blackbrush, new Point(0, ypoint));
                    string bukuDate = _trx._listbuku[i]._Tanggal;
                    bukuDate = bukuDate.Substring(0, 10);
                    g.DrawString(bukuDate, font, blackbrush, new Point(0, ypoint));
                g.DrawString(_trx._listbuku[i]._SecurityCode, font, blackbrush, new Point(261, ypoint));
                g.DrawString(_trx._listbuku[i]._Keterangan, font, blackbrush, new Point(409, ypoint));
                baris += 1;
                    //ypoint += 13;
                ypoint += 26;
                if (baris == 9)
                    {
                        //ypoint = ypoint + (12 * 5);
                    ypoint = ypoint + (24 * 5);
                }
                //}
            }
            //for(int i = 0; i < _trx._BukuTipe.Length; i++)
            //{
            //    string keterangan = _trx._BukuSandi[i];
            //    int nominal = int.Parse(_trx._BukuNominal[i]);

            //    if (keterangan == "DBT")
            //    {
            //        string sandi = _trx._BukuTipe[i];
            //        saldo -= nominal;
            //        string debetprint = "-" + nominal.ToString("N0");
            //        g.DrawString(sandi, font, blackbrush, new Point(87, ypoint));
            //        g.DrawString(debetprint, font, blackbrush, new Point(155, ypoint));
            //        g.DrawString(saldo.ToString("N0"), font, blackbrush, new Point(340, ypoint));
            //    }
            //    else
            //    {
            //        //Random rnd = new Random();
            //        string sandi = _trx._BukuTipe[i];
            //        saldo += nominal;
            //        string kreditprint = nominal.ToString("N0");
            //        g.DrawString(sandi, font, blackbrush, new Point(87, ypoint));
            //        g.DrawString(kreditprint, font, blackbrush, new Point(249, ypoint));
            //        g.DrawString(saldo.ToString("N0"), font, blackbrush, new Point(340, ypoint));
            //    }

            //    g.DrawString(baris.ToString(), font, blackbrush, new Point(0, ypoint));
            //    g.DrawString(_trx._BukuDate, font, blackbrush, new Point(30, ypoint));
            //    g.DrawString(_trx._BukuPengesahan[i], font, blackbrush, new Point(442, ypoint));
            //baris += 1;
            //ypoint += 13;
            //if (baris == 21)
            //{
            //    ypoint = ypoint + (12 * 5);
            //}
            //}
            _trx._BukuSaldo = saldo.ToString();
        }

        public string PrintPassbookSimpedes(Transaksi trx)
        {
            bool result = false;
            int nol = 0;
            printdoc = new PrintDocument();
            _trx = trx;
            string pathStatus;
            string strfilename = "step-action";
            string textStatus;
            string printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_PASSBOOK);
            //string printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_PRINTERCOBA);
            //PrinterSettings settings = new PrinterSettings();
            //string printername = settings.PrinterName;
            printdoc.PrinterSettings.PrinterName = printername;
            printdoc.BeginPrint += new PrintEventHandler(BeginPrintEH);
            printdoc.EndPrint += new PrintEventHandler(EndPrintEH);
            printdoc.PrintPage += new PrintPageEventHandler(PassbookPrintSimpedesPage);
            Utility.WriteLog("Printer condition : print passbook in " + printername + " start", "step-action");
            printdoc.Print();
            Utility.WriteLog("Printer condition : check status printing start", "step-action");
            printerstatus.StatusPrinting(printername);
            pathStatus = printerstatus.workingdirectory;
            pathStatus = pathStatus + "\\logs\\logs" + DateTime.Now.ToString("yyyyMM") + "\\" + strfilename + DateTime.Now.ToString("yyMMdd-HH") + ".txt";
            textStatus = Utility.ReadLog(pathStatus);
            Utility.WriteLog(textStatus, "step-action");
            Utility.ClearLog(pathStatus);
            Utility.WriteLog("Printer condition : log has been moved from " + pathStatus, "step-action");
            Status = printerstatus.StatusCode;
            if (printerstatus.StatusCode == 0)
            {
                Console.WriteLine("Print Selesai ...");
                Utility.WriteLog("Printer condition : print passbook in " + printername + " finished", "step-action");
            }
            else
            {
                Console.WriteLine("Print Gagal ...");
                Utility.WriteLog("Printer condition : print passbook in " + printername + " failed", "step-action");
            }
            trx.setStatusPrinting(Status.ToString());
            return trx._BukuSaldo;
        }

        public void PassbookPrintSimpedesPage(object sender, PrintPageEventArgs e)
        {
            font = new Font("Calibri", 6.5F, FontStyle.Regular);
            SolidBrush blackbrush = new SolidBrush(Color.Black);
            Graphics g = e.Graphics;
            string saldo;
            int baris = Convert.ToInt32(_trx._BukuBaris);
            int ypoint = 115;
            int sisabaris = 13 * baris;
            if (baris > 15)
            {
                sisabaris = sisabaris + (12 * 5);
            }
            ypoint += sisabaris;

            for (int i = 0; i < _trx._listbuku.Count; i++)
            {
                string keterangan;
                //int nominal = Convert.ToInt32(_trx._listbuku[i]._Nominal.Substring(0, _trx._listbuku[i]._Nominal.Length - 2));
                //if (keterangan == "DBT")
                //{
                //    string sandi = _trx._listbuku[i]._KodeTransaksi;
                //    saldo -= nominal;
                //    string debetprint = "-" + nominal.ToString("N0") + ",00";
                //    g.DrawString(sandi, font, blackbrush, new Point(49, ypoint));
                //    g.DrawString(debetprint, font, blackbrush, new Point(95, ypoint));
                //    g.DrawString(saldo.ToString("N0"), font, blackbrush, new Point(306, ypoint));
                //}
                //else if (keterangan == "KRT")
                //{
                //    string sandi = _trx._listbuku[i]._KodeTransaksi;
                //    saldo += nominal;
                //    string kreditprint = nominal.ToString("N0") + ",00";
                //    g.DrawString(sandi, font, blackbrush, new Point(49, ypoint));
                //    g.DrawString(kreditprint, font, blackbrush, new Point(189, ypoint));
                //    g.DrawString(saldo.ToString("N0"), font, blackbrush, new Point(306, ypoint));
                //}
                string sandi = _trx._listbuku[i]._Sandi;
                //saldo = checkZero(_trx._listbuku[i]._PassbookBalance);
                saldo = _trx._listbuku[i]._PassbookBalance;
                saldo = checkZero(saldo);
                //string debetprint = checkZero(_trx._listbuku[i]._PassbookDebitAmount);
                string debetprint = _trx._listbuku[i]._PassbookDebitAmount;
                debetprint = checkZero(debetprint);
                //string kreditprint = checkZero(_trx._listbuku[i]._PassbookCreditAmount);
                string kreditprint = _trx._listbuku[i]._PassbookCreditAmount;
                kreditprint = checkZero(kreditprint);
                g.DrawString(sandi, font, blackbrush, new Point(57 - 11, ypoint));
                g.DrawString(debetprint, font, blackbrush, new Point(95 - 11, ypoint));
                g.DrawString(kreditprint, font, blackbrush, new Point(189 - 11, ypoint));
                g.DrawString(saldo, font, blackbrush, new Point(306 - 11, ypoint));
                //g.DrawString(baris.ToString(), font, blackbrush, new Point(0, ypoint));
                string bukuDate = _trx._listbuku[i]._PassbookDate;
                //bukuDate = bukuDate.Substring(0, 10);
                g.DrawString(bukuDate, font, blackbrush, new Point(0, ypoint));
                //g.DrawString(_trx._listbuku[i]._SecurityCode, font, blackbrush, new Point(442, ypoint));
                //string line = _trx._listbuku[i]._PassbookLine.Substring(1, _trx._listbuku[i]._PassbookLine.Length);
                string line = _trx._listbuku[i]._PassbookLine;
                line = line.Substring(1, line.Length - 1);
                g.DrawString(line, font, blackbrush, new Point(268 - 11, ypoint));
                keterangan = _trx._listbuku[i]._PassbookMNECode + " " + _trx._listbuku[i]._PassbookTellerID;
                g.DrawString(keterangan, font, blackbrush, new Point(420 - 11, ypoint));
                baris += 1;
                ypoint += 13;
                if (baris == 16)
                {
                    ypoint = ypoint + (12 * 5);
                }
            }
            //for(int i = 0; i < _trx._BukuTipe.Length; i++)
            //{
            //    string keterangan = _trx._BukuSandi[i];
            //    int nominal = int.Parse(_trx._BukuNominal[i]);

            //    if (keterangan == "DBT")
            //    {
            //        string sandi = _trx._BukuTipe[i];
            //        saldo -= nominal;
            //        string debetprint = "-" + nominal.ToString("N0");
            //        g.DrawString(sandi, font, blackbrush, new Point(87, ypoint));
            //        g.DrawString(debetprint, font, blackbrush, new Point(155, ypoint));
            //        g.DrawString(saldo.ToString("N0"), font, blackbrush, new Point(340, ypoint));
            //    }
            //    else
            //    {
            //        //Random rnd = new Random();
            //        string sandi = _trx._BukuTipe[i];
            //        saldo += nominal;
            //        string kreditprint = nominal.ToString("N0");
            //        g.DrawString(sandi, font, blackbrush, new Point(87, ypoint));
            //        g.DrawString(kreditprint, font, blackbrush, new Point(249, ypoint));
            //        g.DrawString(saldo.ToString("N0"), font, blackbrush, new Point(340, ypoint));
            //    }

            //    g.DrawString(baris.ToString(), font, blackbrush, new Point(0, ypoint));
            //    g.DrawString(_trx._BukuDate, font, blackbrush, new Point(30, ypoint));
            //    g.DrawString(_trx._BukuPengesahan[i], font, blackbrush, new Point(442, ypoint));
            //baris += 1;
            //ypoint += 13;
            //if (baris == 21)
            //{
            //    ypoint = ypoint + (12 * 5);
            //}
            //}
            //_trx._BukuSaldo = saldo.ToString();
        }

        public async Task PrintBukuHabis(Transaksi trx)
        {
            bool result = false;
            int nol = 0;
            printdoc = new PrintDocument();
            _trx = trx;
            string pathStatus;
            string strfilename = "step-action";
            string textStatus;
            string printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_PASSBOOK);
            //string printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_PRINTERCOBA);
            //PrinterSettings settings = new PrinterSettings();
            //string printername = settings.PrinterName;
            printdoc.PrinterSettings.PrinterName = printername;
            printdoc.BeginPrint += new PrintEventHandler(BeginPrintEH);
            printdoc.EndPrint += new PrintEventHandler(EndPrintEH);
            printdoc.PrintPage += new PrintPageEventHandler(PassbookBukuHabis);
            Utility.WriteLog("Printer condition : print passbook in " + printername + " start", "step-action");
            printdoc.Print();
            Utility.WriteLog("Printer condition : check status printing start", "step-action");
            printerstatus.StatusPrinting(printername);
            pathStatus = printerstatus.workingdirectory;
            pathStatus = pathStatus + "\\logs\\logs" + DateTime.Now.ToString("yyyyMM") + "\\" + strfilename + DateTime.Now.ToString("yyMMdd-HH") + ".txt";
            textStatus = Utility.ReadLog(pathStatus);
            Utility.WriteLog(textStatus, "step-action");
            Utility.ClearLog(pathStatus);
            Utility.WriteLog("Printer condition : log has been moved from " + pathStatus, "step-action");
            Status = printerstatus.StatusCode;
            if (printerstatus.StatusCode == 0)
            {
                Console.WriteLine("Print Selesai ...");
                Utility.WriteLog("Printer condition : print passbook in " + printername + " finished", "step-action");
            }
            else
            {
                Console.WriteLine("Print Gagal ...");
                Utility.WriteLog("Printer condition : print passbook in " + printername + " failed", "step-action");
            }
            trx.setStatusPrinting(Status.ToString());
            //return trx._BukuSaldo;
        }

        public void PassbookBukuHabis(object sender, PrintPageEventArgs e)
        {
            font = new Font("Arial", 8, FontStyle.Regular);
            SolidBrush blackbrush = new SolidBrush(Color.Black);
            Graphics g = e.Graphics;
            string saldo;
            int baris = Convert.ToInt32(_trx._BukuBaris);
            int ypoint = 95;
            int sisabaris = 13 * baris;
            if (baris > 15)
            {
                sisabaris = sisabaris + (12 * 5);
            }
            ypoint += sisabaris;

            for (int i = 0; i < _trx._listbuku.Count; i++)
            {
                string keterangan;
                //int nominal = Convert.ToInt32(_trx._listbuku[i]._Nominal.Substring(0,_trx._listbuku[i]._Nominal.Length - 2));
                //if(keterangan == "DBT")
                //{
                //    string sandi = _trx._listbuku[i]._KodeTransaksi;
                //    saldo -= nominal;
                //    string debetprint = "-" + nominal.ToString("N0") + ",00";
                //    g.DrawString(sandi, font, blackbrush, new Point(57, ypoint));
                //    g.DrawString(debetprint, font, blackbrush, new Point(95, ypoint));
                //    g.DrawString(saldo.ToString("N0"), font, blackbrush, new Point(306, ypoint));
                //}
                //else if(keterangan == "KRT")
                //{
                //    string sandi = _trx._listbuku[i]._KodeTransaksi;
                //    saldo += nominal;
                //    string kreditprint = nominal.ToString("N0") + ",00";
                //    g.DrawString(sandi, font, blackbrush, new Point(57, ypoint));
                //    g.DrawString(kreditprint, font, blackbrush, new Point(189, ypoint));
                //    g.DrawString(saldo.ToString("N0"), font, blackbrush, new Point(306, ypoint));
                //}
                string sandi = _trx._listbuku[i]._Sandi;
                //saldo = checkZero(_trx._listbuku[i]._PassbookBalance);
                saldo = _trx._listbuku[i]._PassbookBalance;
                saldo = checkZero(saldo);
                //string debetprint = checkZero(_trx._listbuku[i]._PassbookDebitAmount);
                string debetprint = _trx._listbuku[i]._PassbookDebitAmount;
                debetprint = checkZero(debetprint);
                //string kreditprint = checkZero(_trx._listbuku[i]._PassbookCreditAmount);
                string kreditprint = _trx._listbuku[i]._PassbookCreditAmount;
                kreditprint = checkZero(kreditprint);
                g.DrawString(sandi, font, blackbrush, new Point(57 - 11, ypoint));
                g.DrawString(debetprint, font, blackbrush, new Point(95 - 11, ypoint));
                g.DrawString(kreditprint, font, blackbrush, new Point(189 - 11, ypoint));
                g.DrawString(saldo, font, blackbrush, new Point(306 - 11, ypoint));
                //g.DrawString(baris.ToString(), font, blackbrush, new Point(0, ypoint));
                string bukuDate = _trx._listbuku[i]._PassbookDate;
                //bukuDate = bukuDate.Substring(0, 10);
                g.DrawString(bukuDate, font, blackbrush, new Point(0, ypoint));
                //g.DrawString(_trx._listbuku[i]._SecurityCode, font, blackbrush, new Point(442, ypoint));
                //string line = _trx._listbuku[i]._PassbookLine.Substring(1, _trx._listbuku[i]._PassbookLine.Length);
                string line = _trx._listbuku[i]._PassbookLine;
                line = line.Substring(1, line.Length - 1);
                g.DrawString(line, font, blackbrush, new Point(268 - 11, ypoint));
                keterangan = _trx._listbuku[i]._PassbookMNECode + " " + _trx._listbuku[i]._PassbookTellerID;
                g.DrawString(keterangan, font, blackbrush, new Point(420 - 11, ypoint));
                baris += 1;
                ypoint += 13;
                if (baris == 16)
                {
                    ypoint = ypoint + (12 * 5);
                }
            }
        }

        public string PrintBukuPenuh(Transaksi trx)
        {
            printdoc = new PrintDocument();
            _trx = trx;
            string pathStatus;
            string strfilename = "step-action";
            string textStatus;
            string printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_THERMAL);
            //string printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_PRINTERCOBA);
            //PrinterSettings settings = new PrinterSettings();
            //string printername = settings.PrinterName;
            printdoc.PrinterSettings.PrinterName = printername;
            printdoc.BeginPrint += new PrintEventHandler(BeginPrintEH);
            printdoc.EndPrint += new PrintEventHandler(EndPrintEH);
            printdoc.PrintPage += new PrintPageEventHandler(BukuPenuhPage);
            Utility.WriteLog("Printer condition : print buku penuh in " + printername + " start", "step-action");
            printdoc.Print();
            Utility.WriteLog("Printer condition : check status printing start", "step-action");
            printerstatus.StatusPrinting(printername);
            pathStatus = printerstatus.workingdirectory;
            pathStatus = pathStatus + "\\logs\\logs" + DateTime.Now.ToString("yyyyMM") + "\\" + strfilename + DateTime.Now.ToString("yyMMdd-HH") + ".txt";
            textStatus = Utility.ReadLog(pathStatus);
            Utility.WriteLog(textStatus, "step-action");
            Utility.ClearLog(pathStatus);
            Utility.WriteLog("Printer condition : log has been moved from " + pathStatus, "step-action");
            //Status = printerstatus.StatusCode;
            //if (printerstatus.StatusCode == 0)
            //{
            //    Console.WriteLine("Print Selesai ...");
            //    Utility.WriteLog("Printer condition : print buku penuh in " + printername + " finished", "step-action");
            //}
            //else
            //{
            //    Console.WriteLine("Print Gagal ...");
            //    Utility.WriteLog("Printer condition : print buku penuh in " + printername + " failed", "step-action");
            //}
            Console.WriteLine("Print Selesai ...");
            Utility.WriteLog("Printer condition : print buku penuh in " + printername + " finished", "step-action");
            trx.setStatusPrinting(Status.ToString());
            return trx._BukuSaldo;
        }

        private void BukuPenuhPage(object sender, PrintPageEventArgs e)
        {
            StringFormat formatLeft = new StringFormat(StringFormatFlags.NoClip);
            StringFormat formatCenter = new StringFormat(formatLeft);
            formatCenter.Alignment = StringAlignment.Center;
            float point = 4;
            float sizex = 20;
            float sizey = point;
            float offset = 0;
            float lineheight = font.GetHeight() + point;
            SizeF layoutsize = new SizeF(280, lineheight);
            //RectangleF layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            RectangleF layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            path = Directory.GetCurrentDirectory();
            string logo = path + "\\wwwroot\\inputopenaccount" + config.Read("PATH", Config.PARAM_PATH_IMAGE_THERMAL);
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            Graphics g = e.Graphics;
            font = new Font("Arial", 10, FontStyle.Regular);
            Image img = Image.FromFile(logo);

            g.DrawImage(img, (e.PageBounds.Width - img.Width) / 2, 0, img.Width, img.Height);
            offset = img.Height + point;
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            //g.DrawString("Cetak Thermal", new Font("Arial", 12, FontStyle.Regular), blackBrush, new Point(5, 10));

            string lokasi = "Cikupa";
            string joint = "Lokasi : " + lokasi;
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            joint = "Date : " + DateTime.Now.ToString("yyyy-MM-dd");
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            joint = "No Rekening : " + _trx._AccountNumber.Substring(0, _trx._AccountNumber.Length - 4) + "****";
            g.DrawString(joint, font, blackBrush, layout, formatLeft);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            joint = "Nama Nasabah : " + _trx.namaNasabah;
            g.DrawString(joint, font, blackBrush, layout, formatLeft);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            int saldo = int.Parse(_trx._BukuSaldo);
            joint = "Saldo Buku : Rp " + saldo.ToString("N0");
            g.DrawString(joint, font, blackBrush, layout, formatLeft);
            offset += lineheight;
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            joint = "Harap Menghubungi Costumer Service";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            joint = "Untuk Melakukan Penggantian Buku";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            joint = "Dengan Menyertakan Struk Ini";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
        }

        public async Task PrintBukaRekening(TransaksiBaru trxbaru)
        {
            printdoc = new PrintDocument();
            _trxbaru = trxbaru;
            string pathStatus;
            string strfilename = "step-action";
            string textStatus;
            string printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_THERMAL);
            //string printername = config.Read("PRINTERNAME", Config.PARAM_PRINTERNAME_PRINTERCOBA);
            printdoc.PrinterSettings.PrinterName = printername;
            printdoc.BeginPrint += new PrintEventHandler(BeginPrintEH);
            printdoc.EndPrint += new PrintEventHandler(EndPrintEH);
            printdoc.PrintPage += new PrintPageEventHandler(BukaRekeningPage);
            Utility.WriteLog("Printer condition : print buka rekening in " + printername + " start", "step-action");
            printdoc.Print();
            Utility.WriteLog("Printer condition : check status printing start", "step-action");
            printerstatus.StatusPrinting(printername);
            pathStatus = printerstatus.workingdirectory;
            pathStatus = pathStatus + "\\logs\\logs" + DateTime.Now.ToString("yyyyMM") + "\\" + strfilename + DateTime.Now.ToString("yyMMdd-HH") + ".txt";
            textStatus = Utility.ReadLog(pathStatus);
            Utility.WriteLog(textStatus, "step-action");
            Utility.ClearLog(pathStatus);
            Utility.WriteLog("Printer condition : log has been moved from " + pathStatus, "step-action");
            //Status = printerstatus.StatusCode;
            //if (Status == 0)
            //{
            //    Console.WriteLine("Print Selesai ...");
            //    Utility.WriteLog("Printer condition : print buka rekening in " + printername + " finished", "step-action");
            //}
            //else
            //{
            //    Console.WriteLine("Print Gagal ...");
            //    Utility.WriteLog("Printer condition : print buka rekening in " + printername + " failed", "step-action");
            //}
            Console.WriteLine("Print Selesai ...");
            Utility.WriteLog("Printer condition : print buka rekening in " + printername + " finished", "step-action");
            trxbaru.setStatusPrinting(Status.ToString());
        }

        private void BukaRekeningPage(object sender, PrintPageEventArgs e)
        {
            StringFormat formatLeft = new StringFormat(StringFormatFlags.NoClip);
            StringFormat formatCenter = new StringFormat(formatLeft);
            formatCenter.Alignment = StringAlignment.Center;
            float point = 4;
            float sizex = 20;
            float sizey = point;
            float offset = 0;
            float lineheight = font.GetHeight() + point;
            SizeF layoutsize = new SizeF(280, lineheight);
            //RectangleF layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            RectangleF layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            path = Directory.GetCurrentDirectory();
            string logo = path + "\\wwwroot\\inputopenaccount" + config.Read("PATH", Config.PARAM_PATH_IMAGE_THERMAL);
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            Graphics g = e.Graphics;
            font = new Font("Arial", 10, FontStyle.Regular);
            Image img = Image.FromFile(logo);

            g.DrawImage(img, (e.PageBounds.Width - img.Width) / 2, 0, img.Width, img.Height);
            offset = img.Height + point;
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            //g.DrawString("Cetak Thermal", new Font("Arial", 12, FontStyle.Regular), blackBrush, new Point(5, 10));

            string lokasi = "Cikupa";
            string joint = "BRI KC " + lokasi;
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);

            joint = "DATETIME : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            g.DrawString(joint, font, blackBrush, layout, formatLeft);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            joint = "TRANSAKSI : PEMBUKAAN AKUN";
            g.DrawString(joint, font, blackBrush, layout, formatLeft);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            joint = "PESAN : SUKSES";
            g.DrawString(joint, font, blackBrush, layout, formatLeft);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            joint = "NO REKENING : " + _trxbaru._NomorRekening;
            g.DrawString(joint, font, blackBrush, layout, formatLeft);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            joint = "REFERENCE ID : " + _trxbaru._ReferenceID;
            g.DrawString(joint, font, blackBrush, layout, formatLeft);
            offset += lineheight;

            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            joint = "PROSES PEMBUKAAN AKUN TELAH BERHASIL";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            joint = "BERHASIL. HARAP UBAH PIN ANDA";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            joint = "UNTUK AKUN TERSEBUT";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            joint = "UNTUK PERTANYAAN LEBIH LANJUT";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
            offset += lineheight;
            layout = new RectangleF(new PointF(sizex, sizey + offset), layoutsize);
            joint = "SILAKAN KUNJUNGI KANTOR CABANG BRI TERDEKAT";
            g.DrawString(joint, font, blackBrush, layout, formatCenter);
        }
    }
}
