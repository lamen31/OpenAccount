using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAccount.Data
{
    public class TransaksiBaru
    {
        public string _TransaksiID { get; set; }
        public string _TujuanRekening { get; set; }
        public string _NIK { get; set; }
        public string _Nama { get; set; }
        public string _TTL { get; set; }
        public string _PerkawinanKTP { get; set; }
        public string _AlamatKTP { get; set; }
        public string _AgamaKTP { get; set; }
        public string _PekerjaanKTP { get; set; }
        public string _ImageKTP { get; set; }
        public string _JenisTabungan { get; set; }
        public string _ModelKartu { get; set; }
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
        public string _ImageNPWP { get; set; }
        public string _ImageTTD1 { get; set; }
        public string _ImageTTD2 { get; set; }
        public string _PinATM1 { get; set; }
        public string _PinATM2 { get; set; }
        public string _PinEBanking1 { get; set; }
        public string _PinEBanking2 { get; set; }
        public string _NomorRekening { get; set; }
        public string _MenuEBanking { get; set; }
        public string _ReferenceID { get; set; }
        public string _StatusPrinter { get; set; }
        public bool _StatusSignPad { get; set; } = false;

        public void setTransaksiID(string strtransaksiid)
        {
            _TransaksiID = strtransaksiid;
        }

        public void setTujuanRekening(string strtujuan)
        {
            _TujuanRekening = strtujuan;
        }

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

        public void setImageKTP(string strimage)
        {
            _ImageKTP = strimage;
        }

        public void setJenisTabungan(string strjenis)
        {
            _JenisTabungan = strjenis;
        }

        public void setModelKartu(string strmodel)
        {
            _ModelKartu = strmodel;
        }

        public void setNasabahBaru(string strnamaibu, string strhandphone, string stremail, string strpekerjaan, string strpenghasilan)
        {
            _NamaIbu = strnamaibu;
            _Handphone = strhandphone;
            _Email = stremail;
            _PekerjaanData = strpekerjaan;
            _Penghasilan = strpenghasilan;
        }

        public void clearNasabahBaru()
        {
            _NamaIbu = string.Empty;
            _Handphone = string.Empty;
            _Email = string.Empty;
            _PekerjaanData = string.Empty;
            _Penghasilan = string.Empty;
        }

        public void setNasabahBaru3(string strnamaibu, string strhandphone, string stremail, string strpekerjaan, string strpenghasilan, string strperusahaan, string strusaha, string strtelepon, string strjabatan, string strkota, string stralamat)
        {
            _NamaIbu = strnamaibu;
            _Handphone = strhandphone;
            _Email = stremail;
            _PekerjaanData = strpekerjaan;
            _Penghasilan = strpenghasilan;
            _NamaPerusahaan = strperusahaan;
            _UsahaKantor = strusaha;
            _TeleponKantor = strtelepon;
            _Jabatan = strjabatan;
            _Kota = strkota;
            _AlamatKantor = stralamat;
        }

        public void setNasabahBaru2(string strperusahaan, string strusaha, string strtelepon, string strjabatan, string strkota, string stralamat)
        {
            _NamaPerusahaan = strperusahaan;
            _UsahaKantor = strusaha;
            _TeleponKantor = strtelepon;
            _Jabatan = strjabatan;
            _Kota = strkota;
            _AlamatKantor = stralamat;
        }

        public void clearNasabahBaru2()
        {
            _NamaPerusahaan = string.Empty;
            _UsahaKantor = string.Empty;
            _TeleponKantor = string.Empty;
            _Jabatan = string.Empty;
            _Kota = string.Empty;
            _AlamatKantor = string.Empty;
        }

        public void setNomorNPWP(string strnpwp)
        {
            _NomorNPWP = strnpwp;
        }

        public void clearNomorNPWP()
        {
            _NomorNPWP = string.Empty;
        }

        public void setImageNPWP(string strimage)
        {
            _ImageNPWP = strimage;
        }

        public void setImageTTD1 (string strimage)
        {
            _ImageTTD1 = strimage;
        }

        public void setImageTTD2(string strimage)
        {
            _ImageTTD2 = strimage;
        }

        public void setPinPertama(string strpin)
        {
            _PinATM1 = strpin;
        }

        public void clearPinPertama()
        {
            _PinATM1 = string.Empty;
        }

        public void setPinRekening(string strpin)
        {
            _PinATM2 = strpin;
        }

        public void setEBanking(string strmenu)
        {
            _MenuEBanking = strmenu;
        }

        public void setPinEBankingPertama(string strpin)
        {
            _PinEBanking1 = strpin;
        }

        public void setPinEBanking(string strpin, string strreference)
        {
            _PinEBanking2 = strpin;
            _ReferenceID = strreference;
        }

        public void setNomorRekening(string strnorek)
        {
            _NomorRekening = strnorek;
        }

        public void setStatusPrinting(string strstatus)
        {
            _StatusPrinter = strstatus;
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
            _NomorRekening = string.Empty;
            _MenuEBanking = string.Empty;
            _ReferenceID = string.Empty;
        }
    }
}
