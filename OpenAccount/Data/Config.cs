using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;
using System.IO;

namespace OpenAccount.Data
{
    public class Config
    {
        public const string PARAM_PATH_PASSBOOK = "param.path.passbook";
        public const string PARAM_PATH_A4 = "param.path.a4";
        public const string PARAM_PATH_THERMAL = "param.path.thermal";
        public const string PARAM_PATH_IMAGE_A4 = "param.path.image.a4";
        public const string PARAM_PATH_IMAGE_THERMAL = "param.path.image.thermal";
        public const string PARAM_PATH_IMAGE_SAVESCANNER = "param.path.image.savescanner";
        public const string PARAM_PATH_IMAGE_SAVESIGNPAD = "param.path.image.savesignpad";
        public const string PARAM_PATH_IMAGE_SAVESIGNPAD2 = "param.path.image.savesignpad2";
        public const string PARAM_PATH_PRINTSERVER_A4 = "param.path.printserver.a4";
        public const string PARAM_PATH_PRINTSERVER_PASSBOOK = "param.path.printserver.passbook";
        public const string PARAM_PATH_PRINTSERVER_PRINTCOBA = "param.path.printserver.printcoba";
        public const string PARAM_PATH_PRINTSERVER_THERMAL = "param.path.printserver.thermal";
        public const string PARAM_PATH_SIGNPAD = "param.path.signpad";
        public const string PARAM_NAMA_NASABAH = "param.nama.nasabah";
        public const string PARAM_REKENING_NASABAH = "param.rekening.nasabah";
        public const string PARAM_ALAMAT_NASABAH = "param.alamat.nasabah";
        public const string PARAM_SALDO_NASABAH = "param.saldo.nasabah";
        public const string PARAM_PIN_NASABAH = "param.pin.nasabah";
        public const string PARAM_PASSBOOK_MAXBARIS = "param.passbook.maxbaris";
        public const string PARAM_PASSBOOK_MAXHALAMAN = "param.passbook.maxhalaman";
        public const string PARAM_PASSBOOK_MAXBARISBISNIS = "param.passbook.maxbarisbisnis";
        public const string PARAM_PRINTERNAME_PRINTERCOBA = "param.printername.printercoba";
        public const string PARAM_PRINTERNAME_A4 = "param.printername.a4";
        public const string PARAM_PRINTERNAME_PASSBOOK = "param.printername.passbook";
        public const string PARAM_PRINTERNAME_THERMAL = "param.printername.thermal";
        public const string PARAM_PORT_EDC = "param.port.edc";
        public const string PARAM_PORT_CARD_DISPENSER = "param.port.card.dispenser";
        public const string PARAM_SERVICES_LINK = "param.services.link";
        public const string PARAM_SERVICES_LINK2 = "param.services.link2";
        public const string PARAM_SERVICES_REKENING = "param.services.rekening";
        public const string PARAM_SERVICES_PASSBOOK = "param.services.passbook";
        public const string PARAM_SERVICES_PASSBOOK1 = "param.services.passbook1";
        public const string PARAM_SERVICES_LOG = "param.services.log";
        public const string PARAM_SERVICES_EMAIL = "param.services.email";
        public const string PARAM_SERVICES_SMS = "param.services.sms";
        public const string PARAM_SERVICES_SMS_OTP = "param.services.smsotp"; 
        public const string PARAM_SERVICES_INQUIRY_NOTIFICATION = "param.services.inquirynotification";
        public const string PARAM_SERVICES_INQUIRY_ESTATEMENT = "param.services.inquiryestatement";
        public const string PARAM_SERVICES_HISTORI = "param.services.histori";
        public const string PARAM_SERVICES_THERMAL = "param.services.thermal";
        public const string PARAM_SERVICE_CEKPIN = "param.service.cekpin";
        public const string PARAM_SERVICES_REPORT = "param.services.report";
        public const string PARAM_DEVICE_TERMINAL_ID = "param.device.terminalid";

        public const string PARAM_SERVICES_POST_HISTORI = "param.services.post.histori";
        public const string PARAM_SERVICES_NASABAH = "param.service.nasabah";
        public const string PARAM_SERVICES_SAVE = "param.service.save";
        public const string PARAM_READER_SAMPCID = "param.reader.sampcid";
        public const string PARAM_READER_SAMPCONF = "param.reader.sampconf";
        public const string PARAM_READER_SAM = "param.reader.sam";
        public const string PARAM_READER_RF = "param.reader.rf";
        public const string PARAM_READER_LOOP = "param.reader.loop";
        public const string PARAM_READER_LOOP2 = "param.reader.loop2";
        public const string PARAM_DISPENSER_BOX = "param.dispenser.box";

        public static readonly byte[] OPENSAM = { 0x00, 0xF0, 0x00, 0x00, 0x20 };
        public static readonly byte[] UIDA = { 0xFF, 0xCA, 0x00, 0x00, 0x00 };
        public static readonly byte[] DFEKTP = { 0x00, 0xA4, 0x00, 0x00, 0x02, 0x7F, 0x0A };
        public static readonly byte[] EFPHOTO = { 0x00, 0xA4, 0x00, 0x00, 0x02, 0x6F, 0xF2 };
        public static readonly byte[] READRF = { 0x00, 0xB0 };
        public static readonly byte[] UIDB = { 0x00, 0xDD, 0x00, 0x00, 0x08 };
        public static readonly byte[] EFCC = { 0x00, 0xA4, 0x00, 0x00, 0x02, 0x6F, 0xF0 };
        public static readonly byte[] CC = { 0x00, 0xB0, 0x00, 0x00, 0x35 };
        public static readonly byte[] SAMRESET = { 0x00, 0xFF, 0x00, 0x00, 0x00 };
        public static readonly byte[] GETCHAL = { 0x00, 0x84, 0x00, 0x00, 0x08 };
        public static readonly byte[] CALCHAL33 = { 0x00, 0xF1, 0x00, 0x01 };
        public static readonly byte[] CALCHAL1D = { 0x00, 0xF1, 0x00, 0x00 };
        public static readonly byte[] EXTAUTH = { 0x00, 0x82, 0x00, 0x00, 0x28 };
        public static readonly byte[] INTAUTH = { 0x00, 0xF2, 0x00, 0x00 };
        public static readonly byte[] ENCRYPT = { 0x00, 0xF3, 0x00, 0x00 };
        public static readonly byte[] EFDS = { 0x00, 0xF3, 0x00, 0x00, 0x07, 0x00, 0xA4, 0x00, 0x00, 0x02, 0x6F, 0xF6 };
        public static readonly byte[] DECRYPT = { 0x00, 0xF4, 0x00, 0x00 };
        public static readonly byte[] READDS = { 0x00, 0xF3, 0x00, 0x00, 0x05, 0x00, 0xB0, 0x00, 0x00, 0x50 };
        public static readonly byte[] AUTOVERIF = { 0x00, 0xFA, 0x00, 0x00, 0x00 };
        public static readonly byte[] RETDS = { 0x00, 0xFA, 0x06, 0x00 };
        public static readonly byte[] EFBIO = { 0x00, 0xF3, 0x00, 0x00, 0x07, 0x00, 0xA4, 0x00, 0x00, 0x02, 0x6F, 0xF1 };
        public static readonly byte[] SAMREAD = { 0x05, 0x00, 0xB0 };
        public static readonly byte[] RETPHOTO = { 0x00, 0xFA, 0x03, 0x00 };
        public static readonly byte[] AUTODECIP = { 0x00, 0xFA, 0x05, 0x00, 0x00 };
        public static readonly byte[] SIGNATURE = { 0x00, 0xF3, 0x00, 0x00, 0x07, 0x00, 0xA4, 0x00, 0x00, 0x02, 0x6F, 0xF3 };
        public static readonly byte[] MINUTIAE1 = { 0x00, 0xF3, 0x00, 0x00, 0x07, 0x00, 0xA4, 0x00, 0x00, 0x02, 0x6F, 0xF4 };
        public static readonly byte[] MINUTIAE2 = { 0x00, 0xF3, 0x00, 0x00, 0x07, 0x00, 0xA4, 0x00, 0x00, 0x02, 0x6F, 0xF5 };
        public static readonly byte[] STOPAUTOVERIF = { 0x00, 0xFA, 0x02, 0x00, 0x00 };
        public static readonly byte[] VERIFYDF = { 0x00, 0xFA, 0x04, 0x00, 0x00 };
        public static readonly byte[] ENCCC = { 0x00, 0xF3, 0x00, 0x00, 0x07, 0x00, 0xA4, 0x00, 0x00, 0x02, 0x6F, 0xF0 };
        public static readonly byte[] ACTIVATION = { 0x00, 0xF8, 0x00, 0x00, 0x00 };
        public static readonly byte[] UIDSAM = { 0x00, 0xF7, 0x00, 0x00, 0x00 };
        public static readonly byte[] LOGACTIVATION = { 0x00, 0xFD, 0x00, 0x00, 0x20, 0x7B, 0xDA, 0x03, 0xC5, 0x78, 0x99, 0xD0, 0x26, 0x7A, 0xAE, 0x80, 0xDB, 0x96, 0x90, 0x00, 0xB3, 0x02, 0xB6, 0x7D, 0x2C, 0xA0, 0x14, 0x47, 0x8C, 0xED, 0x6E, 0xF3, 0x3C, 0x49, 0xD3, 0x14, 0x29 };

        public static string PARAM_EXT_AUTH = string.Empty;
        public static int PARAM_EXT_AUTH_LEN = 0;
        public static string PARAM_CALC_CHALLENGE = string.Empty;
        public static string PARAM_UID = string.Empty;
        public static string PARAM_INT_AUTH = string.Empty;
        public static int PARAM_INT_AUTH_LEN = 0;
        public static string PARAM_RETRIEVE_DS = string.Empty;

        public static string PARAM_READER_SLOT1 = "01";
        public static string PARAM_READER_SLOT2 = "02";

        private FileIniDataParser iniFile = new FileIniDataParser();
        private IniData iniData = new IniData();

        private string filename = "OpenAccount_Config.properties";

        public Config()
        {
            filename = Directory.GetCurrentDirectory() + "\\OpenAccount_Config.properties";
        }

        public void Init()
        {
            iniData = iniFile.ReadFile(filename);
        }

        public string Read(string strSection, string strName)
        {
            string result = string.Empty;
            Init();

            result = iniData[strSection][strName];
            return result;
        }

        public void Write(string strSection, string strName, string strValue)
        {
            Init();

            try
            {
                iniData.Sections.AddSection(strSection);

                iniData[strSection][strName] = strValue;
                iniData.Configuration.NewLineStr = "\r\n";
                iniFile.WriteFile(filename, iniData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
