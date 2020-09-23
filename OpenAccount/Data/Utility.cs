using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace OpenAccount.Data
{
    public class Utility
    {
        public static string ReadLog(string strdatapath)
        {
            string result;
            result = File.ReadAllText(strdatapath);
            return result;
        }
        public static void ClearLog(string strdatapath)
        {
            File.WriteAllText(strdatapath, "");
            File.Delete(strdatapath);
        }
        public static void WriteLog(string strmessage, string strfilename)
        {
            string errormessage = string.Empty;
            WriteLog(strmessage, strfilename, ref errormessage);
        }

        private static void WriteLog(string strmessage, string strfilename, ref string strerror)
        {
            strerror = string.Empty;
            try
            {
                string dir = DirectoryFolder() + @"\logs" + DateTime.Now.ToString("yyyyMM");
                CreateDirectory(dir);

                string filename = dir + @"\" + strfilename + DateTime.Now.ToString("yyMMdd-HH") + ".txt";
                //CreateFile(filename);

                string msg = string.Empty;
                WriteToFile(DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " " + strmessage, filename, ref strerror);
            }
            catch (Exception ex)
            {
                strerror = ex.Message;
            }
        }

        private static string DirectoryFolder()
        {
            string dir = Directory.GetCurrentDirectory() + @"\logs";
            CreateDirectory(dir);
            return dir;
        }

        private static void CreateDirectory(string strdirectory)
        {
            if (!Directory.Exists(strdirectory))
                Directory.CreateDirectory(strdirectory);
        }
        private static void CreateFile(string strfilename)
        {
            if (!File.Exists(strfilename))
                File.Create(strfilename);
        }
        private static bool WriteToFile(string strmessage, string strfilename, ref string strerror)
        {
            bool result = false;
            strerror = string.Empty;
            try
            {
                using (StreamWriter streamwriter = new StreamWriter(strfilename, true))
                {
                    streamwriter.WriteLine(strmessage);
                }
                result = true;
            }
            catch (Exception ex)
            {
                strerror = ex.Message;
            }
            return result;
        }
    }
}
