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

        public async Task signpad()
        {
            PathSignPad = config.Read("PATH", Config.PARAM_PATH_SIGNPAD);
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
                process.Dispose();
            }
        }
    }
}
