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
            Utility.WriteLog("Signpad condition : sign pad process running", "step-action");
            process.Start();
            process.WaitForExit();
            Utility.WriteLog("Signpad condition : sign pad process done", "step-action");
            if (process.HasExited)
            {
                Utility.WriteLog("Signpad condition : sign pad process close", "step-action");
                process.Close();
                //process.Dispose();
            }
        }

        public async Task saveSign()
        {
            string strBase64 = string.Empty;
            string path = Directory.GetCurrentDirectory();
            path = path + "\\" + config.Read("PATH", Config.PARAM_PATH_IMAGE_SAVESIGNPAD);
            File.Delete(path);
            Utility.WriteLog("Sign pad condition : signature from " + path + " has deleted", "step-action");

            pathWorking = workingdirectory + "\\hwsign.png";
            if (File.Exists(pathWorking))
            {
                await Task.Run(() => File.Move(pathWorking, path));
                Utility.WriteLog("Sign pad condition : signature from " + pathWorking + " has moved to " + path, "step-action");

                strBase64 = convertToBase64(path);
                trxbaru.setImageTTD1(strBase64);
                Utility.WriteLog("Sign pad condition : set image signpad base64 success", "step-action");
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
                    Utility.WriteLog("Sign pad condition : convert image to base64 success", "step-action");
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
            Utility.WriteLog("Sign pad condition : signature from " + path + " has deleted", "step-action");

            pathWorking = workingdirectory + "\\hwsign.png";
            if (File.Exists(pathWorking))
            {
                await Task.Run(() => File.Move(pathWorking, path));
                Utility.WriteLog("Sign pad condition : signature from " + pathWorking + " has moved to " + path, "step-action");

                strBase64 = convertToBase64(path);
                trxbaru.setImageTTD2(strBase64);
                Utility.WriteLog("Sign pad condition : set image signpad2 base64 success", "step-action");
            }
        }

        public async Task DeleteSign()
        {
            string path = Directory.GetCurrentDirectory();
            string path1 = path + "\\" + config.Read("PATH", Config.PARAM_PATH_IMAGE_SAVESIGNPAD);
            File.Delete(path1);
            Utility.WriteLog("Sign pad condition : signature 1 from " + path1 + " has deleted", "step-action");
            string path2 = path + "\\" + config.Read("PATH", Config.PARAM_PATH_IMAGE_SAVESIGNPAD2);
            File.Delete(path2);
            Utility.WriteLog("Sign pad condition : signature 2 from " + path2 + " has deleted", "step-action");
            Utility.WriteLog("Sign pad condition : all file signature has been deleted", "step-action");
        }

        public async Task DeleteSign2()
        {
            string path = Directory.GetCurrentDirectory();
            path += "\\" + config.Read("PATH", Config.PARAM_PATH_IMAGE_SAVESIGNPAD2);
            File.Delete(path);
            Utility.WriteLog("Sign pad condition : signature 2 has been deleted", "step-action");
        }

        public async Task CloseProcess()
        {
            process.Close();
            Utility.WriteLog("Signpad condition : sign pad process close", "step-action");
        }
    }
}
