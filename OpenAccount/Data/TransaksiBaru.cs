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
        public string _AlamatKTP { get; set; }
        public string _AgamaKTP { get; set; }
        public string _PekerjaanKTP { get; set; }
        public string _NamaIbu { get; set; }
        public string _Handphone { get; set; }
        public string _Email { get; set; }
        public string _KodePos { get; set; }
        public string _PekerjaanData { get; set; }
        public string _Penghasilan { get; set; }
        public string _NamaPerusahaan { get; set; }
        public string _UsahaKantor { get; set; }
        public string _TeleponKantor { get; set; }
        public string _Jabatan { get; set; }
        public string _Kota { get; set; }
        public string _AlamatKantor { get; set; }
        public string _NomorNPWP { get; set; }

        public void setKTPNasabah(string strnik, string strnama, string strttl, string strperkawinan, string stralamat, string stragama, string strpekerjaan)
        {
            _NIK = strnik;
            _Nama = strnama;
            _TTL = strttl;
            _PerkawinanKTP = strperkawinan;
            _AlamatKTP = stralamat;
            _AgamaKTP = stragama;
            _PekerjaanKTP = strpekerjaan;
        }

        public void setNasabahBaru(string strnamaibu, string strhandphone, string stremail, string strpos, string strpekerjaan, string strpenghasilan, string strperusahaan, string strusaha, string strtelepon, string strjabatan, string strkota, string stralamat)
        {
            _NamaIbu = strnamaibu;
            _Handphone = strhandphone;
            _Email = stremail;
            _KodePos = strpos;
            _PekerjaanData = strpekerjaan;
            _Penghasilan = strpenghasilan;
            _NamaPerusahaan = strperusahaan;
            _UsahaKantor = strusaha;
            _TeleponKantor = strtelepon;
            _Jabatan = strjabatan;
            _Kota = strkota;
            _AlamatKantor = stralamat;
        }

        public void setNomorNPWP(string strnpwp)
        {
            _NomorNPWP = strnpwp;
        }

        public void clear()
        {
            _NIK = string.Empty;
            _Nama = string.Empty;
            _TTL = string.Empty;
            _PerkawinanKTP = string.Empty;
            _PekerjaanKTP = string.Empty;
            _NamaIbu = string.Empty;
            _Handphone = string.Empty;
            _Email = string.Empty;
            _KodePos = string.Empty;
            _PekerjaanData = string.Empty;
            _Penghasilan = string.Empty;
            _NamaPerusahaan = string.Empty;
            _UsahaKantor = string.Empty;
            _TeleponKantor = string.Empty;
            _Jabatan = string.Empty;
            _Kota = string.Empty;
            _AlamatKantor = string.Empty;
            _NomorNPWP = string.Empty;
        }
    }
}
