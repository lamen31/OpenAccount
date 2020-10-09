using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace OpenAccount.Data
{
    public class SignPad
    {
        public string PathSignPad;
        public string workingdirectory;
        Process process = new Process();
        Config config = new Config();
        TransaksiBaru trxbaru = new TransaksiBaru();
        private string pathWorking;

        public async Task signpad()
        {
            string path = Directory.GetCurrentDirectory();
            PathSignPad = path + "\\" + config.Read("PATH", Config.PARAM_PATH_SIGNPAD);
            workingdirectory = Path.GetDirectoryName(PathSignPad);
            process.StartInfo.FileName = PathSignPad;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = workingdirectory;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            Console.WriteLine("SIGN PAD: SIGN PAD PROCESS RUNNING");
            Utility.WriteLog("Signpad condition : sign pad process running", "step-action");
            process.WaitForExit();
            if (process.HasExited)
            {
                process.Close();
                Console.WriteLine("SIGN PAD FUNCTION: SIGN PAD PROCESS CLOSE");
                Utility.WriteLog("Signpad condition : sign pad process close", "step-action");
                //process.Dispose();
            }
        }

        public async Task saveSign()
        {
            string strBase64 = string.Empty;
            string path = Directory.GetCurrentDirectory();
            path = path + "\\" + config.Read("PATH", Config.PARAM_PATH_IMAGE_SAVESIGNPAD);
            File.Delete(path);
            Console.WriteLine("FILE FROM " + path + " HAS DELETED");
            Utility.WriteLog("Buka rekening condition : file from " + path + " has deleted", "step-action");

            pathWorking = workingdirectory + "\\hwsign.png";
            if (File.Exists(pathWorking))
            {
                await Task.Run(() => File.Move(pathWorking, path));
                Console.WriteLine("FILE FROM " + pathWorking + " HAS MOVED TO " + path);
                Utility.WriteLog("Buka rekening condition : file from " + pathWorking + " has moved to " + path, "step-action");

                strBase64 = convertToBase64(path);
                trxbaru.setImageTTD1(strBase64);
            }
        }

        private string convertToBase64(string strpath)
        {
            string base64String = string.Empty;
            var path = strpath;
            using (Image image = Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }
            return base64String;
        }

        public async Task saveSign2()
        {
            string strBase64 = string.Empty;
            string path = Directory.GetCurrentDirectory();
            path = path + "\\" + config.Read("PATH", Config.PARAM_PATH_IMAGE_SAVESIGNPAD2);
            File.Delete(path);
            Console.WriteLine("FILE FROM " + path + " HAS DELETED");
            Utility.WriteLog("Buka rekening condition : file from " + path + " has deleted", "step-action");

            pathWorking = workingdirectory + "\\hwsign.png";
            if (File.Exists(pathWorking))
            {
                await Task.Run(() => File.Move(pathWorking, path));
                Console.WriteLine("FILE FROM " + pathWorking + " HAS MOVED TO " + path);
                Utility.WriteLog("Buka rekening condition : file from " + pathWorking + " has moved to " + path, "step-action");

                strBase64 = convertToBase64(path);
                trxbaru.setImageTTD2(strBase64);
            }
        }
    }
}
