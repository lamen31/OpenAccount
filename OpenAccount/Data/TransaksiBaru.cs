using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAccount.Data
{
    public class TransaksiBaru
    {
        public string _NIK { get; set; }
        public string _Nama { get; set; }
        public string _TTL { get; set; }
        public string _PerkawinanKTP { get; set; }
        public string _PekerjaanKTP { get; set; }
        public string _NamaIbu { get; set; }
        public string _Agama { get; set; }
        public string _PerkawinanData { get; set; }
        public string _StatusTinggal { get; set; }
        public bool _SesuaiKTP { get; set; }
        public string _Pendidikan { get; set; }
        public string _Penghasilan { get; set; }
        public string _Email { get; set; }
        public string _PekerjaanData { get; set; }
        public string _Pendapatan { get; set; }
        public string _Handphone { get; set; }
        public string _NamaPerusahaan { get; set; }
        public string _BidangUsaha { get; set; }
        public string _KodePos { get; set; }
        public string _Jabatan { get; set; }
        public string _Kota { get; set; }
        public string _Kantor { get; set; }
        public string _TelpKantor { get; set; }

        public void setKTPNasabah(string strnik, string strnama, string strttl, string strperkawinan, string strpekerjaan)
        {
            _NIK = strnik;
            _Nama = strnama;
            _TTL = strttl;
            _PerkawinanKTP = strperkawinan;
            _PekerjaanKTP = strpekerjaan;
        }

        public void setNasabahBaru(string strnamaibu, string stragama, string strperkawinan, string strstatustinggal, bool boolflagsesuai)
        {
            _NamaIbu = strnamaibu;
            _Agama = stragama;
            _PerkawinanData = strperkawinan;
            _StatusTinggal = strstatustinggal;
            _SesuaiKTP = boolflagsesuai;
        }

        public void setNasabahBaru2(string strpendidikan, string strpenghasilan, string stremail, string strpekerjaan, string strpendapatan, string strhandphone)
        {
            _Pendidikan = strpendidikan;
            _Penghasilan = strpenghasilan;
            _Email = stremail;
            _PekerjaanData = strpekerjaan;
            _Pendapatan = strpendapatan;
            _Handphone = strhandphone;
        }

        public void setNasabahBaru3(string strnamaperusahaan, string strbidangusaha, string strkodepos, string strjabatan, string strkota, string strkantor, string strtelpkantor)
        {
            _NamaPerusahaan = strnamaperusahaan;
            _BidangUsaha = strbidangusaha;
            _KodePos = strkodepos;
            _Jabatan = strjabatan;
            _Kota = strkota;
            _Kantor = strkantor;
            _TelpKantor = strtelpkantor;
        }

        public void clear()
        {
            _NIK = string.Empty;
            _Nama = string.Empty;
            _TTL = string.Empty;
            _PerkawinanKTP = string.Empty;
            _PekerjaanKTP = string.Empty;
            _NamaIbu = string.Empty;
            _Agama = string.Empty;
            _PerkawinanData = string.Empty;
            _StatusTinggal = string.Empty;
            _Pendidikan = string.Empty;
            _Penghasilan = string.Empty;
            _Email = string.Empty;
            _PekerjaanData = string.Empty;
            _Pendapatan = string.Empty;
            _Handphone = string.Empty;
            _NamaPerusahaan = string.Empty;
            _BidangUsaha = string.Empty;
            _KodePos = string.Empty;
            _Jabatan = string.Empty;
            _Kota = string.Empty;
            _Kantor = string.Empty;
            _TelpKantor = string.Empty;
        }
    }
}
