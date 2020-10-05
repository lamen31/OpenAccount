using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace OpenAccount.Data
{
    public class SignPad
    {
        public string PathSignPad;
        public string workingdirectory;
        Process process = new Process();
        Config config = new Config();
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
            }
        }

        public async Task saveSign2()
        {
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
            }
        }
    }
}
