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
        public const string PARAM_NAMA_NASABAH = "param.nama.nasabah";
        public const string PARAM_REKENING_NASABAH = "param.rekening.nasabah";
        public const string PARAM_ALAMAT_NASABAH = "param.alamat.nasabah";
        public const string PARAM_SALDO_NASABAH = "param.saldo.nasabah";

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
    }
}
