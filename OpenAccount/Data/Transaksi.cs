using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Threading.Tasks;

namespace OpenAccount.Data
{
    public class Transaksi
    {
        public string[] _Nasabah { get; set; }
        public string _HistoriJenisPeriode { get; set; }
        public string _HistoriStartDate { get; set; }
        public string _HistoriEndDate { get; set; }
        public string _HistoriHourDate { get; set; }
        public string _HistoriJumlahTransaksi { get; set; }
        public string[] _HistoriUraian { get; set; }
        public string[] _HistoriTipe { get; set; }
        public string[] _HistoriNominal { get; set; }
        public string _HistoriSaldo { get; set; }
        public string _HistoriHalaman { get; set; }
        public string _HistoriMaxHalaman { get; set; }
        public string _BukuBaris { get; set; }
        public string _BukuHalaman { get; set; }
        public string _BukuDate { get; set; }
        public string[] _BukuTipe { get; set; }
        public string[] _BukuSandi { get; set; }
        public string[] _BukuNominal { get; set; }
        public string _BukuSaldo { get; set; }
        public string[] _BukuPengesahan { get; set; }
        public string[] _ThermalDate { get; set; }
        public string[] _ThermalNominal { get; set; }
        public string[] _ThermalKode { get; set; }
        public string _ThermalSaldo { get; set; }
        public string _TransaksiID { get; set; }
        public string _KTPNIK { get; set; }
        public string _KTPNama { get; set; }
        public string _KTPTTL { get; set; }
        public string _KTPPerkawinan { get; set; }
        public string _KTPAlamat { get; set; }
        public string _KTPAgama { get; set; }
        public string _KTPPekerjaan { get; set; }
        public string _PinATM1 { get; set; }
        public string _PinATM2 { get; set; }

        public class HistoriTransaksi
        {
            public string _TransactionID { get; set; }
            public string _Keterangan { get; set; }
            public string _JenisTransaksi { get; set; }
            public string _Nominal { get; set; }
            public string _KodeTransaksi { get; set; }
            public string _Tanggal { get; set; }
            public string _SecurityCode { get; set; }
        }

        public class printBuku
        {
            public string _TransactionID { get; set; }
            public string _JenisTransaksi { get; set; }
            public string _Nominal { get; set; }
            public string _Keterangan { get; set; }
            public string _KodeTransaksi { get; set; }
            public string _Tanggal { get; set; }
            public string _SecurityCode { get; set; }
        }

        public class printThermal
        {
            public string _TransactionID { get; set; }
            public string _JenisTransaksi { get; set; }
            public string _Nominal { get; set; }
            public string _Keterangan { get; set; }
            public string _KodeTransaksi { get; set; }
            public string _Tanggal { get; set; }
            public string _SecurityCode { get; set; }
        }

        public List<HistoriTransaksi> _listhistori = new List<HistoriTransaksi>();
        public List<printBuku> _listbuku = new List<printBuku>();
        public List<printThermal> _listthermal = new List<printThermal>();

        public void AddListHistori(string strid, string strjenis, string strnominal, string strketerangan, string strkodetransaksi, string strtanggal, string strsecurity)
        {
            HistoriTransaksi histori = new HistoriTransaksi();
            histori._TransactionID = strid;
            histori._JenisTransaksi = strjenis;
            histori._Nominal = strnominal;
            histori._Keterangan = strketerangan;
            histori._KodeTransaksi = strkodetransaksi;
            histori._Tanggal = strtanggal;
            histori._SecurityCode = strsecurity;
            _listhistori.Add(histori);
        }

        public void AddListBuku(string strid, string strjenis, string strnominal, string strketerangan, string strkodetransaksi, string strtanggal, string strsecurity)
        {
            printBuku buku = new printBuku();
            buku._TransactionID = strid;
            buku._JenisTransaksi = strjenis;
            buku._Nominal = strnominal;
            buku._Keterangan = strketerangan;
            buku._KodeTransaksi = strkodetransaksi;
            buku._Tanggal = strtanggal;
            buku._SecurityCode = strsecurity;
            _listbuku.Add(buku);
        }

        public void AddListThermal(string strid, string strjenis, string strnominal, string strketerangan, string strkodetransaksi, string strtanggal, string strsecurity)
        {
            printThermal thermal = new printThermal();
            thermal._TransactionID = strid;
            thermal._JenisTransaksi = strjenis;
            thermal._Nominal = strnominal;
            thermal._Keterangan = strketerangan;
            thermal._KodeTransaksi = strkodetransaksi;
            thermal._Tanggal = strtanggal;
            thermal._SecurityCode = strsecurity;
            _listthermal.Add(thermal);
        }

        public void setTransaksiID(string strtransaksiid)
        {
            _TransaksiID = strtransaksiid;
        }

        public void setArrayNasabah(string[] strnasabah)
        {
            _Nasabah = strnasabah;
        }

        public void setHistoriTransaksi(string strjenisperiode, string strstartdate, string strenddate, string strhourdate)
        {
            _HistoriJenisPeriode = strjenisperiode;
            _HistoriStartDate = strstartdate;
            _HistoriEndDate = strenddate;
            _HistoriHourDate = strhourdate;
        }

        public void setHistoriTransaksi(string[] struraian, string[] strtipe, string[] strnominal, string strhalaman)
        {
            _HistoriUraian = struraian;
            _HistoriTipe = strtipe;
            _HistoriNominal = strnominal;
            _HistoriHalaman = strhalaman;
        }

        public void setHistori(string strjenisperiode, string startdate, string enddate, string strjumlahtransaksi, string strsaldo)
        {
            _HistoriJenisPeriode = strjenisperiode;
            _HistoriStartDate = startdate;
            _HistoriEndDate = enddate;
            _HistoriJumlahTransaksi = strjumlahtransaksi;
            _HistoriSaldo = strsaldo;
        }

        public void setDataHistoriTransaksi(string strjumlahtransaksi, string[] struraian, string[] strtipe, string[] strnominal, string strsaldo)
        {
            _HistoriJumlahTransaksi = strjumlahtransaksi;
            _HistoriUraian = struraian;
            _HistoriTipe = strtipe;
            _HistoriNominal = strnominal;
            _HistoriSaldo = strsaldo;
        }

        public void setMaxHalamanHistoriTransaksi(string strmaxhalaman)
        {
            _HistoriMaxHalaman = strmaxhalaman;
        }

        public void setHalamanHistori(string strhalaman)
        {
            _HistoriHalaman = strhalaman;
        }

        public void setHistoriThermal(string[] strdate, string[] strkode, string[] strnominal, string strsaldo)
        {
            _ThermalDate = strdate;
            _ThermalKode = strkode;
            _ThermalNominal = strnominal;
            _ThermalSaldo = strsaldo;
        }

        public void setSaldoThermal(string strsaldo)
        {
            _ThermalSaldo = strsaldo;
        }

        public void setPassbookTransaksi(string strbaris, string strhalaman, string strdate, string[] strtipe, string[] strsandi, string[] strnominal, string strsaldo, string[] strpengesahan)
        {
            _BukuBaris = strbaris;
            _BukuHalaman = strhalaman;
            _BukuDate = strdate;
            _BukuTipe = strtipe;
            _BukuSandi = strsandi;
            _BukuNominal = strnominal;
            _BukuSaldo = strsaldo;
            _BukuPengesahan = strpengesahan;
        }

        public void setPassbookTransaksi(string strbaris, string strhalaman, string strsaldo)
        {
            _BukuBaris = strbaris;
            _BukuHalaman = strhalaman;
            _BukuSaldo = strsaldo;
        }

        public void setBuku(string strbaris, string strhalaman)
        {
            _BukuBaris = strbaris;
            _BukuHalaman = strhalaman;
        }

        public void setKTP(string strnik, string strnama, string strttl, string strperkawinan, string stralamat, string stragama, string strpekerjaan)
        {
            _KTPNIK = strnik;
            _KTPNama = strnama;
            _KTPTTL = strttl;
            _KTPPerkawinan = strperkawinan;
            _KTPAlamat = stralamat;
            _KTPAgama = stragama;
            _KTPPekerjaan = strpekerjaan;
        }

        public void setPinPertama(string strpin)
        {
            _PinATM1 = strpin;
        }

        public void setPinKonfirmasi(string strpin)
        {
            _PinATM2 = strpin;
        }

        public void clear()
        {
            _HistoriJenisPeriode = string.Empty;
            _HistoriStartDate = string.Empty;
            _HistoriEndDate = string.Empty;
            _HistoriHourDate = string.Empty;
            _HistoriJumlahTransaksi = string.Empty;
            _HistoriSaldo = string.Empty;
            _HistoriMaxHalaman = string.Empty;
            _BukuBaris = string.Empty;
            _BukuHalaman = string.Empty;
            _BukuDate = string.Empty;
            _BukuSaldo = string.Empty;
            _KTPAgama = string.Empty;
            _KTPAlamat = string.Empty;
            _KTPNama = string.Empty;
            _KTPNIK = string.Empty;
            _KTPPekerjaan = string.Empty;
            _KTPPerkawinan = string.Empty;
            _KTPTTL = string.Empty;
        }



        public void clearArray()
        {
            _HistoriUraian = null;
            _HistoriTipe = null;
            _HistoriNominal = null;
            _BukuTipe = null;
            _BukuSandi = null;
            _BukuNominal = null;
            _BukuPengesahan = null;
        }

        public void clearArrayNasabah()
        {
            _Nasabah = null;
        }

        public void clearListHistori()
        {
            _listhistori.Clear();
        }

        public void clearListBuku()
        {
            _listbuku.Clear();
        }

        public void clearListThermal()
        {
            _listthermal.Clear();
        }

        public void ClearList()
        {
            _listhistori.Clear();
            _listbuku.Clear();
            _listthermal.Clear();
        }
    }
}
