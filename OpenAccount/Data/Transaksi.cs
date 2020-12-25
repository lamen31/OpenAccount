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
        public string startDate { get; set; }
        public string timeOut { get; set; }
        public string endDate { get; set; }
        public DateTime startDT { get; set; }
        public DateTime endDT { get; set; }
        public string mbStatus { get; set; }
        public string nomerKartu { get; set; } = "****************";
        public string namaNasabah { get; set; }
        public string alamat1 { get; set; }
        public string alamat2 { get; set; }
        public string kota { get; set; }
        public string provinsi { get; set; }
        public string kodePos { get; set; }
        public string kodeNegara { get; set; }
        public decimal totalDebet { get; set; } = 0;
        public decimal totalKredit { get; set; } = 0;
        public string debitCurr { get; set; }
        public string kreditCurr { get; set; }
        public string acctCurr { get; set; }
        public string pinBlock { get; set; }
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
        public string _BukuSerial { get; set; }
        public string _BukuDate { get; set; }
        public string[] _BukuTipe { get; set; }
        public string[] _BukuSandi { get; set; }
        public string[] _BukuNominal { get; set; }
        public string _BukuSaldo { get; set; }
        public string[] _BukuPengesahan { get; set; }
        public string _BukuHalamanPrint { get; set; }
        public string _BukuIndex { get; set; }
        public string[] _ThermalDate { get; set; }
        public string[] _ThermalNominal { get; set; }
        public string[] _ThermalKode { get; set; }
        public string _ThermalSaldo { get; set; }
        public string _StatusPrinter { get; set; }
        public string _TransaksiID { get; set; }
        public string _BUID { get; set; }
        public string _TerminalID { get; set; }
        public string _JenisTransaksi { get; set; }
        public long startTime { get; set; }
        public long endTime { get; set; }
        public string _StatusTransaksi { get; set; }
        public string _ErrorCode { get; set; }
        public string _KTPNIK { get; set; }
        public string _KTPNama { get; set; }
        public string _KTPTempatLahir { get; set; }
        public string _KTPTanggalLahir { get; set; }
        public string _KTPAlamat { get; set; }
        public string _KTPRT { get; set; }
        public string _KTPRW { get; set; }
        public string _KTPKecamatan { get; set; }
        public string _KTPKelurahan { get; set; }
        public string _KTPKabupaten { get; set; }
        public string _KTPJenisKelamin { get; set; }
        public string _KTPGolonganDarah { get; set; }
        public string _KTPAgama { get; set; }
        public string _KTPStatusPerkawinan { get; set; }
        public string _KTPPekerjaan { get; set; }
        public string _KTPKewarganegaraan { get; set; }
        public string _KTPMinutiae1 { get; set; }
        public string _KTPMinutiae2 { get; set; }
        public class UnitKerja
        { 
            public string unitKerja { get; set; }
            public string alamat1UKer { get; set; }
            public string alamat2Uker { get; set; }
        }

        public UnitKerja uker = new UnitKerja();
        public string _PinATM1 { get; set; }
        public string _PinATM2 { get; set; }
        public string _AccountNumber { get; set; }
        public string _AccountProductType { get; set; }
        public string _AccountStatus { get; set; }
        public string _ServiceErrorCode { get; set; }
        public string _ServicesErrorMessage { get; set; }
        public List<string> pilihanLayanan { get; } = new List<string>() {"Pencetakan Passbook Printing", "Pencetakan Mutasi 5 Transaksi Terakhir",
                                                                                "Pencetakan Rekening Koran", "Pengiriman Rekening Koran Via Email", "Proses Persiapan Layanan" };
        public List<string> kodeLayanan { get; } = new List<string>() {"BUTAB", "5LAST","EMONTH", "A4MONTH", "PREP" };
        public string statusLayanan { get; set; }
        public int jenisLayanan { get; set; }
        public string MSISDN { get; set; }
        public bool isEmail { get; set; }
        public string emailNasabah { get; set; }
        public string emailAttachment { get; set; }
        public string attachmentPath { get; set; }
        public int emailAttachmentPage { get; set; }
        public string emailNotif { get; set; }
        public string smsNotif { get; set; }
        public string periodMonth { get; set; } = "NO";

        //1. Pencetakan Passbook Printing 
        //2. Pencetakan Mutasi 5 Transaksi Terakhir 
        //3. Pencetakan Rekening Koran  
        //4. Pengiriman Rekening Koran Via Email


        public class HistoriTransaksi
        {
            public string _TransactionID { get; set; }
            public string _Keterangan { get; set; }
            public string _JenisTransaksi { get; set; }
            public string _Nominal { get; set; }
            public string _KodeTransaksi { get; set; }
            public string _Tanggal { get; set; }
            public string _SecurityCode { get; set; }
            public string _SEQ { get; set; }
            public string _IDNUM { get; set; }
            public string _IDNUM2 { get; set; }
            public string _IDNUM3 { get; set; }
            public string _NOREK { get; set; }
            public string _TGLTRAN { get; set; }
            public string _TGLEFEKTIF { get; set; }
            public string _JAMTRAN { get; set; }
            public string _KODETRAN { get; set; }
            public string _DESKTRAN { get; set; }
            public string _SALDOAWALMUTASI { get; set; }
            public string _MUTASIDEBET { get; set; }
            public string _MUTASIKREDIT { get; set; }
            public string _SALDOAKHIRMUTASI { get; set; }
            public string _TRUSER { get; set; }
            public string _GLSIGN { get; set; }
            public string _TERBILANG { get; set; }
            public string _TRREMK { get; set; }
            public string _AUXTRC { get; set; }
            public string _SERIAL { get; set; }
            public string _TLBDS1 { get; set; }
            public string _TLBDS2 { get; set; }
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
            public string _Sandi { get; set; }
            public string _PassbookBalance { get; set; }
            public string _PassbookTellerID { get; set; }
            public string _PassbookDate { get; set; }
            public string _PassbookMNECode { get; set; }
            public string _PassbookCreditAmount { get; set; }
            public string _PassbookDebitAmount { get; set; }
            public string _PassbookLine { get; set; }
            public string _PassbookBranch { get; set; }
        }

        public class MonthlyStatement
        {
            public string NO_REK { get; set; }
            public string TGL_TRAN { get; set; }
            public string TGL_EFEKTIF { get; set; }
            public string JAM_TRAN { get; set; }
            public string KODE_TRAN { get; set; }
            public string DESK_TRAN { get; set; }
            public string SALDO_AWAL_MUTASI { get; set; }
            public decimal saldoAwalMutasi { get; set; }
            public string MUTASI_DEBET { get; set; }
            public decimal mutasiDebet { get; set; }
            public string MUTASI_KREDIT { get; set; }
            public decimal mutasiKredit { get; set; }
            public string SALDO_AKHIR_MUTASI { get; set; }
            public decimal saldoAkhirMutasi { get; set; }
            public string TRUSER { get; set; }
            public string TRREMK { get; set; } 
            public string TERBILANG { get; set; }
        }
        public List<MonthlyStatement> _listMonthly = new List<MonthlyStatement>();

        public class tempPrintBuku
        {
            public string _Sandi { get; set; }
            public string _PassbookBalance { get; set; }
            public string _PassbookTellerID { get; set; }
            public string _PassbookDate { get; set; }
            public string _PassbookMNECode { get; set; }
            public string _PassbookCreditAmount { get; set; }
            public string _PassbookDebitAmount { get; set; }
            public string _PassbookLine { get; set; }
            public string _PassbookBranch { get; set; }
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
            public string _Account { get; set; }
            public string _Date { get; set; }
            public string _Date2 { get; set; }
            public string _TIMENT { get; set; }
            public string _DORC { get; set; }
            public string _AMT { get; set; }
            public string _REMK { get; set; }
        }

        public class Account
        {
            public string _SHORTNAME { get; set; }
            public string _ACCTCURR { get; set; }
            public string _AVAILABLEBAL { get; set; }
            public string _STATUS { get; set; }
        }

        public List<HistoriTransaksi> _listhistori = new List<HistoriTransaksi>();
        public List<printBuku> _listbuku = new List<printBuku>();
        public List<tempPrintBuku> _listtempbuku = new List<tempPrintBuku>();
        public List<printThermal> _listthermal = new List<printThermal>();
        public List<Account> _listaccount = new List<Account>();

        public void AddListAccount(string strshortname, string stracctcurr, string stravailablebal, string strstatus)
        {
            Account akun = new Account();
            akun._SHORTNAME = strshortname;
            akun._ACCTCURR = stracctcurr;
            akun._AVAILABLEBAL = stravailablebal;
            akun._STATUS = strstatus;
        }

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

        public void AddListHistori2(string strseq, string stridnum, string stridnum2, string stridnum3, string strnorek, string strtgltran, string strtglefektif, string strjamtran, 
            string strkodetran, string strdesktran, string strsaldoawalmutasi, string strmutasidebet, string strmutasikredit, string strsaldoakhirmutasi, string strtruser, string strglsign, 
            string strterbilang, string strtrremk, string strauxtrc, string strserial, string strtlbds1, string strtlbds2)
        {
            HistoriTransaksi histori = new HistoriTransaksi();
            histori._SEQ = strseq;
            histori._IDNUM = stridnum;
            histori._IDNUM2 = stridnum2;
            histori._IDNUM3 = stridnum3;
            histori._NOREK = strnorek;
            histori._TGLTRAN = strtgltran;
            histori._TGLEFEKTIF = strtglefektif;
            histori._JAMTRAN = strjamtran;
            histori._KODETRAN = strkodetran;
            histori._DESKTRAN = strdesktran;
            histori._SALDOAWALMUTASI = strsaldoawalmutasi;
            histori._MUTASIDEBET = strmutasidebet;
            histori._MUTASIKREDIT = strmutasikredit;
            histori._SALDOAKHIRMUTASI = strmutasikredit;
            histori._TRUSER = strtruser;
            histori._GLSIGN = strglsign;
            histori._TERBILANG = strterbilang;
            histori._TRREMK = strtrremk;
            histori._AUXTRC = strauxtrc;
            histori._SERIAL = strserial;
            histori._TLBDS1 = strtlbds1;
            histori._TLBDS2 = strtlbds2;
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

        public void AddListBuku2(string strsandi, string strbalance, string strtellerid, string strdate, string strmnecode, string strcredit, string strdebit, string strline, string strbranch)
        {
            printBuku buku = new printBuku();
            buku._Sandi = strsandi;
            buku._PassbookBalance = strbalance;
            buku._PassbookTellerID = strtellerid;
            buku._PassbookDate = strdate;
            buku._PassbookMNECode = strmnecode;
            buku._PassbookCreditAmount = strcredit;
            buku._PassbookDebitAmount = strdebit;
            buku._PassbookLine = strline;
            buku._PassbookBranch = strbranch;
            _listbuku.Add(buku);
        }

        public void AddListTempBuku(string strsandi, string strbalance, string strtellerid, string strdate, string strmnecode, string strcredit, string strdebit, string strline, string strbranch)
        {
            tempPrintBuku buku = new tempPrintBuku();
            buku._Sandi = strsandi;
            buku._PassbookBalance = strbalance;
            buku._PassbookTellerID = strtellerid;
            buku._PassbookDate = strdate;
            buku._PassbookMNECode = strmnecode;
            buku._PassbookCreditAmount = strcredit;
            buku._PassbookDebitAmount = strdebit;
            buku._PassbookLine = strline;
            buku._PassbookBranch = strbranch;
            _listtempbuku.Add(buku);
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

        public void AddListThermal2(string stracct, string strdate, string strdate2, string strtiment, string strdorc, string stramt, string strremk)
        {
            printThermal thermal = new printThermal();
            thermal._Account = stracct;
            thermal._Date = strdate;
            thermal._Date2 = strdate2;
            thermal._TIMENT = strtiment;
            thermal._DORC = strdorc;
            thermal._AMT = stramt;
            thermal._REMK = strremk;
            _listthermal.Add(thermal);
        }

        public void setAccountNumber(string straccount)
        {
            _AccountNumber = straccount;
        }

        public void setTransaksiID(string strtransaksiid)
        {
            _TransaksiID = strtransaksiid;
        }

        public void SetTransaction(string strTerminalID, string strJenisTransaksi)
        {
            //_BUID = strBUID;
            _TerminalID = strTerminalID;
            _JenisTransaksi = strJenisTransaksi;
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

        public void setPassbookTransaksi(string strbaris, string strdate, string[] strtipe, string[] strsandi, string[] strnominal, string strsaldo, string[] strpengesahan)
        {
            _BukuBaris = strbaris;
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

        public void setPassbookTransaksi(string strbaris, string strsaldo)
        {
            _BukuBaris = strbaris;
            _BukuSaldo = strsaldo;
        }

        public void setPassbookTransaksi2(string strbaris, string strsaldo, string strserial)
        {
            _BukuBaris = strbaris;
            _BukuSaldo = strsaldo;
            _BukuSerial = strserial;
        }

        public void setPassbookTransaksi2(string strindex, string strsaldo)
        {
            _BukuIndex = strindex;
            _BukuSaldo = strsaldo;
        }

        public void setHalamanPrint(string strhalaman)
        {
            _BukuHalamanPrint = strhalaman;
        }

        public void setBuku(string strbaris, string strhalaman)
        {
            _BukuBaris = strbaris;
            _BukuHalaman = strhalaman;
        }

        public void setStatusPrinting(string strstatus)
        {
            _StatusPrinter = strstatus;
        }

        public void setDataKTP(string strnik, string strnama, string strtempatlahir, string strtanggallahir, string stralamat, string strrt, string strrw, 
            string strkecamatan, string strkelurahan, string strkabupaten, string strkelamin, string strgoldarah, string stragama, string strperkawinan,
            string strpekerjaan, string strkewarganegaraan)
        {
            _KTPNIK = strnik;
            _KTPNama = strnama;
            _KTPTempatLahir = strtempatlahir;
            _KTPTanggalLahir = strtanggallahir;
            _KTPAlamat = stralamat;
            _KTPRT = strrt;
            _KTPRW = strrw;
            _KTPKecamatan = strkecamatan;
            _KTPKelurahan = strkelurahan;
            _KTPKabupaten = strkabupaten;
            _KTPJenisKelamin = strkelamin;
            _KTPGolonganDarah = strgoldarah;
            _KTPAgama = stragama;
            _KTPStatusPerkawinan = strperkawinan;
            _KTPPekerjaan = strpekerjaan;
            _KTPKewarganegaraan = strkewarganegaraan;
        }

        public void setMinutiae(string strminutiae1, string strminutiae2)
        {
            _KTPMinutiae1 = strminutiae1;
            _KTPMinutiae2 = strminutiae2;
        }

        public void setPinPertama(string strpin)
        {
            _PinATM1 = strpin;
        }

        public void setPinKonfirmasi(string strpin)
        {
            _PinATM2 = strpin;
        }

        public void setErrorService(string strcode, string strmessage)
        {
            _ServiceErrorCode = strcode;
            _ServicesErrorMessage = strmessage;
        }

        public void setProductType(string strproduct)
        {
            _AccountProductType = strproduct;
        }
        public void setStatus(string strstatus)
        {
            _AccountStatus = strstatus;
        }

        public class AuditTrail
        {
            public string _action { get; set; }
            public string _data { get; set; }
            public string _result { get; set; }
        }

        public List<AuditTrail> _auditTrail = new List<AuditTrail>();

        public void AddTrail(string strAction, string strData, string strResult)
        {
            AuditTrail at = new AuditTrail();
            at._action = strAction;
            at._data = strData;
            at._result = strResult;
            _auditTrail.Add(at);
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
            _KTPNIK = string.Empty;
            _KTPNama = string.Empty;
            _KTPTempatLahir = string.Empty;
            _KTPTanggalLahir = string.Empty;
            _KTPAlamat = string.Empty;
            _KTPRT = string.Empty;
            _KTPRW = string.Empty;
            _KTPKecamatan = string.Empty;
            _KTPKelurahan = string.Empty;
            _KTPKabupaten = string.Empty;
            _KTPJenisKelamin = string.Empty;
            _KTPGolonganDarah = string.Empty;
            _KTPAgama = string.Empty;
            _KTPStatusPerkawinan = string.Empty;
            _KTPPekerjaan = string.Empty;
            _KTPKewarganegaraan = string.Empty;
            _KTPMinutiae1 = string.Empty;
            _KTPMinutiae2 = string.Empty;
            _TransaksiID = string.Empty;
            //_BUID = string.Empty;
            _TerminalID = string.Empty;
            _JenisTransaksi = string.Empty;
            _StatusTransaksi = string.Empty;
            _ErrorCode = string.Empty;
            _ServiceErrorCode = string.Empty;
            _ServicesErrorMessage = string.Empty;
            _AccountProductType = string.Empty;
            _AccountNumber = string.Empty;
            _AccountStatus = string.Empty;
        }

        public void clearTransactionLog()
        {
            _TransaksiID = string.Empty;
            //_BUID = string.Empty;
            _TerminalID = string.Empty;
            _JenisTransaksi = string.Empty;
            _StatusTransaksi = string.Empty;
            _ErrorCode = string.Empty;
        }

        public void clearKTP()
        {
            _KTPNIK = string.Empty;
            _KTPNama = string.Empty;
            _KTPTempatLahir = string.Empty;
            _KTPTanggalLahir = string.Empty;
            _KTPAlamat = string.Empty;
            _KTPRT = string.Empty;
            _KTPRW = string.Empty;
            _KTPKecamatan = string.Empty;
            _KTPKelurahan = string.Empty;
            _KTPKabupaten = string.Empty;
            _KTPJenisKelamin = string.Empty;
            _KTPGolonganDarah = string.Empty;
            _KTPAgama = string.Empty;
            _KTPStatusPerkawinan = string.Empty;
            _KTPPekerjaan = string.Empty;
            _KTPKewarganegaraan = string.Empty;
            _KTPMinutiae1 = string.Empty;
            _KTPMinutiae2 = string.Empty;
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

        public void clearListTempBuku()
        {
            _listtempbuku.Clear();
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
            _listaccount.Clear();
            _listtempbuku.Clear();
        }
    }

    public class EKTP
    {
        public string strBio { get; set; } = "";
        public byte[] bytePhoto { get; set; } = new byte[65536];
        public int photolen { get; set; } = 0;
        public byte[] byteBio { get; set; } = new byte[65536];
        public int biolen { get; set; } = 0;
        public byte[] byteSignature { get; set; } = new byte[65536];
        public int signlen { get; set; } = 0;
        public byte[] byteMinu1 { get; set; } = new byte[65536];
        public int minu1len { get; set; } = 0;
        public byte[] byteMinu2 { get; set; } = new byte[65536];
        public int minu2len { get; set; } = 0;
        public List<byte[]> lbPhoto = new List<byte[]>();
        public byte[] UID { get; set; } = new byte[8];
        public byte[] CC { get; set; } = { };
        public UInt16 CC_LEN { get; set; } = 0;
        public byte[] GETCHAL { get; set; } = { };
        public UInt16 GETCHAL_LEN { get; set; } = 0;
        public byte[] CALCHAL { get; set; } = { };
        public UInt16 CALCHAL_LEN { get; set; } = 0;
        public byte[] EXTAUTH { get; set; } = { };
        public UInt16 EXTAUTH_LEN { get; set; } = 0;
        public byte[] INTAUTH { get; set; } = { };
        public UInt16 INTAUTH_LEN { get; set; } = 0;
        public byte[] RETDS { get; set; } = { };
        public UInt16 RETDS_LEN { get; set; } = 0;
        public byte[] FPR { get; set; } = { };

        public String[] SplitBio(string strBio)
        {
            String[] strSplit;
            String[] strSeparator = new String[] { "\",\"" };
            strSplit = strBio.Split(strSeparator, StringSplitOptions.None);

            strSplit[0] = strSplit[0].Remove(0, 1);
            strSplit[20] = strSplit[20].Remove(strSplit[20].Length - 1, 1);
            return strSplit;
        }

        public void ClearData()
        {
            strBio = "";
            bytePhoto = new byte[65536];
            photolen = 0;
            byteBio = new byte[65536];
            biolen = 0;
            byteSignature = new byte[65536];
            signlen = 0;
            byteMinu1 = new byte[65536];
            minu1len = 0;
            byteMinu2 = new byte[65536];
            minu2len = 0;
            lbPhoto.Clear();
            UID = new byte[8];
            CC_LEN = 0;
            GETCHAL_LEN = 0;
            CALCHAL_LEN = 0;
            EXTAUTH_LEN = 0;
            INTAUTH_LEN = 0;
            RETDS_LEN = 0;
        }
    }
}
