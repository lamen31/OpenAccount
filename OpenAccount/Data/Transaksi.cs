using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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

        public void clear()
        {
            _HistoriJenisPeriode = string.Empty;
            _HistoriStartDate = string.Empty;
            _HistoriEndDate = string.Empty;
            _HistoriHourDate = string.Empty;
            _HistoriJumlahTransaksi = string.Empty;
            _HistoriSaldo = string.Empty;
            _HistoriMaxHalaman = string.Empty;
        }

        public void clearArray()
        {
            _HistoriUraian = null;
            _HistoriTipe = null;
            _HistoriNominal = null;
        }
        public void clearArrayNasabah()
        {
            _Nasabah = null;
        }
    }
}
