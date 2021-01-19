using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;
using System.IO;

namespace PrintServerA4
{
    class Config
    {

        public const string PARAM_PRINTERNAME_A4 = "param.printername.a4";
        public const string PARAM_PRINTERNAME_PASSBOOK = "param.printername.passbook";
        public const string PARAM_PRINTERNAME_THERMAL = "param.printername.thermal";

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
