using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using OpenAccount.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAccount.Report
{
    public class RptMonthlyStatement
    {
        
        #region Declaration
        int _maxColumn = 11;
        Document _document;
        PdfPTable _pdfTable = new PdfPTable(11);
        PdfPCell _pdfCell;
        Font _fontStyle;
        MemoryStream _memoryStream = new MemoryStream();
        Transaksi _trx = new Transaksi();
        #endregion

        public RptMonthlyStatement(Transaksi trx)
        {
            _trx = trx;
        }

        private PdfPTable AddLogo()
        {
            int maxColumn = 1;
            PdfPTable pdfPTable = new PdfPTable(maxColumn);

            string imgCombine = System.IO.Directory.GetCurrentDirectory() + @"\wwwroot\assets\img\LogoBRI_A4.png"; ;
            Image img = Image.GetInstance(imgCombine);

            _pdfCell = new PdfPCell(img);
            _pdfCell.Colspan = maxColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            pdfPTable.CompleteRow();
            return pdfPTable;
        }
        private PdfPTable AddRekap()
        {
            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            var fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            var fontStyle2 = FontFactory.GetFont("Calibri", 7f, 1);

            foreach(var dt in _trx._listMonthly)
            {
                _trx.totalDebet = _trx.totalDebet + dt.mutasiDebet;
                _trx.totalKredit = _trx.totalKredit + dt.mutasiKredit;
            }

            int maxColumn = 4;
            PdfPTable pdfPTable = new PdfPTable(maxColumn);

            string imgCombine = System.IO.Directory.GetCurrentDirectory() + @"\wwwroot\assets\img\Logo-Bank-BRI-Mini.png"; ;
            Image img = Image.GetInstance(imgCombine);

            #region Rekap Header
            _pdfCell = new PdfPCell(new Phrase("SALDO AWAL",_fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("TOTAL MUTASI DEBET", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("TOTAL MUTASI KREDIT", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("SALDO AKHIR", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            pdfPTable.CompleteRow();
            #endregion

            #region Rekap Body
            _pdfCell = new PdfPCell(new Phrase(_trx._listMonthly[0].saldoAwalMutasi.ToString("N"), _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase(_trx.totalDebet.ToString("N"), _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase(_trx.totalKredit.ToString("N"), _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase(_trx._listMonthly[_trx._listMonthly.Count-1].saldoAkhirMutasi.ToString("N"), _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            pdfPTable.CompleteRow();

            _pdfCell = new PdfPCell(new Phrase("Terbilang", _fontStyle));
            _pdfCell.Colspan = maxColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            pdfPTable.CompleteRow();

            _pdfCell = new PdfPCell(new Phrase(Terbilang(_trx._listMonthly[_trx._listMonthly.Count-1].saldoAkhirMutasi), _fontStyle));
            _pdfCell.Colspan = maxColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.ExtraParagraphSpace = 0;
            if(_trx._listMonthly[_trx._listMonthly.Count - 1].TERBILANG==null|| _trx._listMonthly[_trx._listMonthly.Count - 1].TERBILANG=="")
            {
                _pdfCell.PaddingBottom = 20f;
            }
            pdfPTable.AddCell(_pdfCell);

            pdfPTable.CompleteRow();

            _pdfCell = new PdfPCell(new Phrase("Biaya materai telah dibayar Lunas", fontStyle2));
            _pdfCell.Colspan = maxColumn;
            _pdfCell.Border = 0;
            _pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            pdfPTable.CompleteRow();

            _pdfCell = new PdfPCell(new Phrase("", fontStyle2));
            _pdfCell.Colspan = 2;
            _pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfCell.PaddingBottom = 30f;
            pdfPTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("", fontStyle2));
            _pdfCell.Colspan = 2;
            _pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfCell.PaddingBottom = 30f;
            pdfPTable.AddCell(_pdfCell);

            pdfPTable.CompleteRow();

            _pdfCell = new PdfPCell(new Phrase("- Apabila terdapat perbedaan dengan catatan Saudara, harap menghubungi kami selambat-lambatnya 14 hari sejak diterimanya rekening koran ini", fontStyle2));
            _pdfCell.Colspan = maxColumn;
            _pdfCell.Border = 0;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            pdfPTable.CompleteRow();

            _pdfCell = new PdfPCell(new Phrase("- Salinan rekening koran ini merupakan hasil centakan komputer, tidak diperlukan tanda tangan pejabat Bank", fontStyle2));
            _pdfCell.Colspan = maxColumn;
            _pdfCell.Border = 0;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            pdfPTable.CompleteRow();

            _pdfCell = new PdfPCell(new Phrase("- Apabila ada perubahan alamat email mohon diinformasikan pada Unit Kerja BANK BRI", fontStyle2));
            _pdfCell.Colspan = maxColumn;
            _pdfCell.Border = 0;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.ExtraParagraphSpace = 0;
            pdfPTable.AddCell(_pdfCell);

            pdfPTable.CompleteRow();
            #endregion
            return pdfPTable;
        }
        public byte[] Report(Transaksi trx)
        {
            _trx = trx;

            _document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Calibri", 8f, 1);
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();

            float[] sizes = new float[_maxColumn];
            for (var i = 0; i < _maxColumn; i++)
            {
                if (i == 0) sizes[i] = 50;
                else sizes[i] = 100;
            }

            _pdfTable.SetWidths(sizes);

            this.ReportHeader();
            this.ReportBody();
            _pdfTable.HeaderRows = 12 ;
            _document.Add(_pdfTable);
            _document.Close();
            return _memoryStream.ToArray();

        }
        
        private void ReportHeader()
        {
            #region Header LAPORAN TRANSAKSI
            _fontStyle = FontFactory.GetFont("Calibri", 14f, 1);
            _pdfCell = new PdfPCell(new Phrase("LAPORAN TRANSAKSI", _fontStyle));
            _pdfCell.Colspan = _maxColumn-3;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border=0;
            _pdfCell.PaddingBottom = 5f;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(this.AddLogo());
            _pdfCell.Colspan = 3;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border=0;
            _pdfCell.PaddingBottom = 5f; 
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();
            #endregion

            #region Header Kepala Surat
            //Kepada Yth.
            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("Kepada Yth.", _fontStyle));
            _pdfCell.Colspan = 3;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.DisableBorderSide(2);
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = _maxColumn - 3;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();

            //Nama
            string Nama = "RORO JONGRANG";
            _fontStyle = FontFactory.GetFont("Calibri", 12f, 1);
            _pdfCell = new PdfPCell(new Phrase(_trx.namaNasabah, _fontStyle));
            _pdfCell.Colspan = 3;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.DisableBorderSide(2);
            _pdfCell.DisableBorderSide(1);
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = _maxColumn - 3;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();

            //Tanggal Laporan
            DateTime dtLaporan = DateTime.Now;
            _fontStyle = FontFactory.GetFont("Calibri", 12f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = 3;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.DisableBorderSide(2);
            _pdfCell.DisableBorderSide(1);
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("Tanggal Laporan", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(":", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(dtLaporan.ToString("dd/MM/yy"), _fontStyle));
            _pdfCell.Colspan = _maxColumn - 6;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();

            //Alamat 1 + Periode Transaksi
            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(_trx.alamat1, _fontStyle));
            _pdfCell.Colspan = 3;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.DisableBorderSide(2);
            _pdfCell.DisableBorderSide(1);
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("Periode Transaksi", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(":", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(_trx.startDT.ToString("dd/MM/yyyy") + "-" + _trx.endDT.ToString("dd/MM/yyyy"), _fontStyle));
            _pdfCell.Colspan = _maxColumn - 6;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();

            //Alamat 2 + Jumlah Halaman
            string Halaman = "";
            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(_trx.alamat2, _fontStyle));
            _pdfCell.Colspan = 3;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.DisableBorderSide(2);
            _pdfCell.DisableBorderSide(1);
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("Halaman", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(":", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(Halaman, _fontStyle));
            _pdfCell.Colspan = _maxColumn - 6;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();

            //Alamat 3
            string Alamat3 = "SELATAN";
            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(_trx.kota, _fontStyle));
            _pdfCell.Colspan = 3;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.DisableBorderSide(1);
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfCell.PaddingBottom = 15f;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = _maxColumn-3;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfCell.PaddingBottom = 15f;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();


            //Nomer Rekening + Unit Kerja
            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("No. Rekening", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0; 
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(":", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(_trx._AccountNumber, _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("Unit Kerja", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(":", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(_trx.uker.unitKerja, _fontStyle));
            _pdfCell.Colspan = _maxColumn - 6;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();

            //Nomer Kartu + Alamat1 Unit Kerja
            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("No. Kartu", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0; 
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(":", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(_trx.nomerKartu, _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("Alamat Unit Kerja", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(":", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(_trx.uker.alamat1UKer, _fontStyle));
            _pdfCell.Colspan = _maxColumn - 6;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();

            //Nama Produk + Alamat2 Unit Kerja
            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("Nama Produk", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0; 
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(":", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(_trx._AccountProductType, _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);
            
            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(_trx.uker.alamat2Uker, _fontStyle));
            _pdfCell.Colspan = _maxColumn - 6;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();

            //Valuta
            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("Valuta", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(":", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase(_trx.acctCurr, _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = _maxColumn - 3;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();
            
            //BLANK SPACE
            _fontStyle = FontFactory.GetFont("Calibri", 12f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = _maxColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border=0;
            _pdfCell.PaddingBottom = 10f; 
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();

            #endregion

            #region Header Padding Bottom
            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            _pdfCell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfCell.Colspan = _maxColumn;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfCell.PaddingBottom = 15f;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();
            #endregion
        }
        private void ReportBody()
        {
            _fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            var fontStyle = FontFactory.GetFont("Calibri", 9f, 1);
            var fontStyle2 = FontFactory.GetFont("Calibri", 7f, 1);

            #region Table Header
            _pdfCell = new PdfPCell(new Phrase("Tanggal Transaksi", _fontStyle));
            _pdfCell.Colspan = 2;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Uraian Transaksi", _fontStyle));
            _pdfCell.Colspan = 4;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Chq No", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Debet", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Kredit", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Saldo", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("Teller", _fontStyle));
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.Gray;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();
            #endregion

            #region Table Body
            int nSL = 1;
            BaseColor bgColor;
            for(var i = 0; i <_trx._listMonthly.Count; i++)
            {
                if (i % 2 == 0)
                    bgColor = BaseColor.White;
                else bgColor = BaseColor.LightGray;

                _pdfCell = new PdfPCell(new Phrase(_trx._listMonthly[i].TGL_TRAN, fontStyle2));
                _pdfCell.Colspan = 2;
                _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = bgColor;
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(_trx._listMonthly[i].DESK_TRAN, fontStyle));
                _pdfCell.Colspan = 4;
                _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = bgColor;
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase("", fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = bgColor;
                _pdfTable.AddCell(_pdfCell);

                string mutasiDebet = string.Empty;
                if (_trx._listMonthly[i].mutasiDebet > 0)
                    mutasiDebet = _trx._listMonthly[i].mutasiDebet.ToString("N") + "D";
                else
                    mutasiDebet = _trx._listMonthly[i].mutasiDebet.ToString("N");
                _pdfCell = new PdfPCell(new Phrase(mutasiDebet, fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = bgColor;
                _pdfTable.AddCell(_pdfCell);

                string mutasiKredit = string.Empty;
                if (_trx._listMonthly[i].mutasiKredit > 0)
                    mutasiKredit = _trx._listMonthly[i].mutasiKredit.ToString("N") + "K";
                else
                    mutasiKredit = _trx._listMonthly[i].mutasiKredit.ToString("N");
                _pdfCell = new PdfPCell(new Phrase(mutasiKredit, fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = bgColor;
                _pdfTable.AddCell(_pdfCell);

                string saldoAkhir = string.Empty;
                if (_trx._listMonthly[i].SALDO_AKHIR_MUTASI.Substring(0,1)=="-")
                    saldoAkhir = _trx._listMonthly[i].saldoAkhirMutasi.ToString("N") + "D";
                else
                    saldoAkhir = _trx._listMonthly[i].saldoAkhirMutasi.ToString("N") + "K";
                _pdfCell = new PdfPCell(new Phrase(saldoAkhir, fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = bgColor;
                _pdfTable.AddCell(_pdfCell);

                _pdfCell = new PdfPCell(new Phrase(_trx._listMonthly[i].TRUSER, fontStyle));
                _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfCell.BackgroundColor = bgColor;
                _pdfTable.AddCell(_pdfCell);

                _pdfTable.CompleteRow();
            }
            #endregion

            #region Table Footer
            //FOOTER
            _pdfCell = new PdfPCell(new Phrase("",fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.Border = 0;
            _pdfCell.PaddingTop = 20f;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(this.AddRekap());
            _pdfCell.Colspan = _maxColumn-2;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.Border = 0;
            _pdfCell.PaddingTop = 20f;
            _pdfTable.AddCell(_pdfCell);

            _pdfCell = new PdfPCell(new Phrase("", fontStyle));
            _pdfCell.Colspan = 1;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.Border = 0;
            _pdfCell.PaddingTop = 20f;
            _pdfTable.AddCell(_pdfCell);

            _pdfTable.CompleteRow();
            #endregion
        }
        string[] satuan = new string[10] { "NOL", "SATU", "DUA", "TIGA", "EMPAT", "LIMA", "ENAM", "TUJUH", "DELAPAN", "SEMBILAN" };
        string[] belasan = new string[10] { "SEPULUH", "SEBELAS", "DUA BELAS", "TIGA BELAS", "EMPAT BELAS", "LIMA BELAS", "ENAM BELAS", "TUJUH BELAS", "DELAPAN BELAS", "SEMBILAN BELAS" };
        string[] puluhan = new string[10] { "", "", "DUA PULUH", "TIGA PULUH", "EMPAT PULUH", "LIMA PULUH", "ENAM PULUH", "TUJUH PULUH", "DELAPAN PULUH", "SEMBILAN PULUH" };
        string[] ribuan = new string[5] { "", "RIBU", "JUTA", "MILYAR", "TRILIYUN" };

        string Terbilang(Decimal d)
        {
            string strHasil = "";
            Decimal frac = d - Decimal.Truncate(d);

            if (Decimal.Compare(frac, 0.0m) != 0)
                strHasil = " KOMA " + Terbilang(Decimal.Round(frac * 100));
            else
                strHasil = "RUPIAH";
            int xDigit = 0;
            int xPosisi = 0;

            string strTemp = Decimal.Truncate(d).ToString();
            for (int i = strTemp.Length; i > 0; i--)
            {
                string tmpx = "";
                xDigit = Convert.ToInt32(strTemp.Substring(i - 1, 1));
                xPosisi = (strTemp.Length - i) + 1;
                switch (xPosisi % 3)
                {
                    case 1:
                        bool allNull = false;
                        if (i == 1)
                            tmpx = satuan[xDigit] + " ";
                        else if (strTemp.Substring(i - 2, 1) == "1")
                            tmpx = belasan[xDigit] + " ";
                        else if (xDigit > 0)
                            tmpx = satuan[xDigit] + " ";
                        else
                        {
                            allNull = true;
                            if (i > 1)
                                if (strTemp.Substring(i - 2, 1) != "0")
                                    allNull = false;
                            if (i > 2)
                                if (strTemp.Substring(i - 3, 1) != "0")
                                    allNull = false;
                            tmpx = "";
                        }

                        if ((!allNull) && (xPosisi > 1))
                            if ((strTemp.Length == 4) && (strTemp.Substring(0, 1) == "1"))
                                tmpx = "SE" + ribuan[(int)Decimal.Round(xPosisi / 3m)] + " ";
                            else
                                tmpx = tmpx + ribuan[(int)Decimal.Round(xPosisi / 3)] + " ";
                        strHasil = tmpx + strHasil;
                        break;
                    case 2:
                        if (xDigit > 0)
                            strHasil = puluhan[xDigit] + " " + strHasil;
                        break;
                    case 0:
                        if (xDigit > 0)
                            if (xDigit == 1)
                                strHasil = "SERATUS " + strHasil;
                            else
                                strHasil = satuan[xDigit] + " RATUS " + strHasil;
                        break;
                }
            }
            strHasil = strHasil.Trim().ToUpper();
            if (strHasil.Length > 0)
            {
                strHasil = strHasil.Substring(0, 1).ToUpper() +
                  strHasil.Substring(1, strHasil.Length - 1);
            }
            return strHasil;
        }
        public void Verify_MultiColumn_Report_CanBe_Processed()
        {
            string path = Directory.GetCurrentDirectory();
            string encryptFileName = "TRILOGI" + _trx._AccountNumber + "_" +_trx.startDT.ToString("yyyyMMdd") + "-" + _trx.endDT.ToString("yyyyMMdd") + ".pdf";
            var pdfFilePath = TestUtil.GetOutputFileName();
            var pdfEncryptFilePath = path + TestUtil.GetOutputFileNameEncrypt() + encryptFileName;
            string passwd = _trx._AccountNumber.Substring(6, 6);
            //_trx.attachmentPath = TestUtil.GetOutputFileNameEncrypt() + encryptFileName;
            _trx.attachmentPath = pdfEncryptFilePath;
            _trx.emailAttachment = encryptFileName;
            var fileStream = new FileStream(pdfFilePath, FileMode.Create);
            _document = new Document(PageSize.A4,10f,10f,10f,10f);
            var pdfWriter = PdfWriter.GetInstance(_document, fileStream);
            pdfWriter.PageEvent = new PdfFooterPart();

            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Calibri", 8f, 1);

            _document.AddAuthor(TestUtil.Author);
            _document.Open();

            float[] sizes = new float[_maxColumn];
            for (var i = 0; i < _maxColumn; i++)
            {
                if (i == 0) sizes[i] = 45;
                else if (i == 1) sizes[i] = 5;
                else if (i == 2) sizes[i] = 55;
                else if (i == 3) sizes[i] = 20;
                else if (i == 4) sizes[i] = 50;
                else if (i == 5) sizes[i] = 5;
                else if (i == 6) sizes[i] = 20;
                else if (i == 7) sizes[i] = 40;
                else if (i == 8) sizes[i] = 40;
                else if (i == 9) sizes[i] = 40;
                else if (i == 10) sizes[i] = 30;
            }
            _pdfTable.SetWidths(sizes);

            this.ReportHeader();
            this.ReportBody();

            _pdfTable.HeaderRows = 0 ;
            _pdfTable.FooterRows = 1;
            _document.Add(_pdfTable);
            _document.Close();
            fileStream.Dispose();

            TestUtil.VerifyPdfFileIsReadable(pdfFilePath);

            //bisa dibuat function baru//
            /*using (Stream input = new FileStream(pdfFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (Stream output = new FileStream(pdfEncryptFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    PdfReader reader = new PdfReader(input);
                    PdfEncryptor.Encrypt(reader, output, true, passwd, "BRI-Super-Secret-Code", PdfWriter.ALLOW_SCREENREADERS);
                    _trx.emailAttachment = pdfEncryptFilePath;
                    _trx.emailAttachmentPage = TestUtil.GetNumberOfPages(pdfFilePath);
                }
            }*/
            EncryptPDF(pdfFilePath, pdfEncryptFilePath, passwd);
        }

        public void EncryptPDF(string inputFile, string outputFile, string uPassword)
        {
            //var pdfFilePath = TestUtil.GetOutputFileName();
            //var pdfEncryptFilePath = TestUtil.GetOutputFileNameEncrypt();
            using (Stream input = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (Stream output = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    PdfReader reader = new PdfReader(input);
                    Utility.WriteLog("Monthly Statement condition : pdf encrypt start.", "step-action");
                    PdfEncryptor.Encrypt(reader, output, true, uPassword, "BRIxTrilogi-Super-Secret-Code", PdfWriter.ALLOW_SCREENREADERS);
                    Utility.WriteLog("Monthly Statement condition : pdf encrypt success.", "step-action");
                    //_trx.emailAttachment = outputFile;
                    _trx.emailAttachmentPage = TestUtil.GetNumberOfPages(inputFile);
                }
            }
        }

        public void Verify_MultiColumn_Report_CanBe_Processed2()
        {
            var pdfFilePath = TestUtil.GetOutputFileName();
            var fileStream = new FileStream(pdfFilePath, FileMode.Create);
            var pdfDoc = new Document(PageSize.A4);
            var pdfWriter = PdfWriter.GetInstance(pdfDoc, fileStream);

            pdfDoc.AddAuthor(TestUtil.Author);
            pdfDoc.Open();

            var table1 = new PdfPTable(1)
            {
                WidthPercentage = 100f,
                HeaderRows = 3,
                FooterRows = 1
            };

            //header row
            var headerCell1 = new PdfPCell(new Phrase("header row-1"));
            table1.AddCell(headerCell1);

            var headerCell2 = new PdfPCell(new Phrase("header row-2"));
            table1.AddCell(headerCell2);

            //footer row
            var footerCell = new PdfPCell(new Phrase(" footer "));
            table1.AddCell(footerCell);

            //adding some rows
            for (int i = 0; i < 400; i++)
            {
                var rowCell = new PdfPCell(new Phrase(i.ToString()));
                table1.AddCell(rowCell);
            }

            //table1.SkipFirstHeader = true;

            // wrapping table1 in multiple columns
            ColumnText ct = new ColumnText(pdfWriter.DirectContent)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };
            ct.AddElement(table1);

            int status = 0;
            int count = 0;
            int l = 0;
            int columnsWidth = 100;
            int columnsMargin = 0;
            int columnsPerPage = 4;
            int r = columnsWidth;
            bool isRtl = false;

            // render the column as long as it has content
            while (ColumnText.HasMoreText(status))
            {
                if (isRtl)
                {
                    ct.SetSimpleColumn(
                        pdfDoc.Right - l, pdfDoc.Bottom,
                        pdfDoc.Right - r, pdfDoc.Top
                    );
                }
                else
                {
                    ct.SetSimpleColumn(
                        pdfDoc.Left + l, pdfDoc.Bottom,
                        pdfDoc.Left + r, pdfDoc.Top
                    );
                }

                var delta = columnsWidth + columnsMargin;
                l += delta;
                r += delta;

                // render as much content as possible
                status = ct.Go();

                // go to a new page if you've reached the last column
                if (++count > columnsPerPage)
                {
                    count = 0;
                    l = 0;
                    r = columnsWidth;
                    pdfDoc.NewPage();
                }
            }

            pdfDoc.Close();
            fileStream.Dispose();

            TestUtil.VerifyPdfFileIsReadable(pdfFilePath);
        }
    }
}
