using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAccount.Data
{
    public class FingerPrint
    {
        int DeviceHandle;
        byte[] FingerBuf = new byte[256 * 360]; //Image Buffer
        byte[] ISOtz = new byte[1024];

        public bool OpenDevice()
        {
            bool result = false;
            DeviceHandle = FingerDLL.FpStdP41M1_OpenDevice();
            if(DeviceHandle > 0)
            {
                Console.WriteLine("Open Device Successed");
                result = true;
            }
            else
            {
                Console.WriteLine("Open Device Failed");
                result = false;
            }
            return result;
        }

        public bool MatchFinger()
        {
            bool result = false;
            result = Match_Func();
            return result;
        }

        private bool Match_Func()
        {
            bool result = false;
            try
            {
                bool loopFinger = true;
                int lRV = -1;
                byte[] MatchImgBuf = new byte[256 * 360]; //Image raw Buffer
                byte[] bmpFingerBuf = new byte[256 * 360 + 1078]; //Image bmp Buffer
                byte[] tz = new byte[1024];
                int SafeLevel = 3;
                while (loopFinger)
                {
                    lRV = FingerDLL.FpStdP41M1_GetImage(0, MatchImgBuf);
                    if (lRV != 1)
                    {
                        string strRet = "";
                        strRet = "Get Image Failed,ret=" + lRV.ToString();
                        Console.WriteLine(strRet);
                        result = false;
                        //return;
                    }
                    else
                    {
                        int AreaScore = FingerDLL.FpStdP41M1_IsFinger(0, MatchImgBuf);
                        if (AreaScore < 45)
                        {
                            continue;
                        }
                        else
                        {
                            loopFinger = false;
                            break;
                        }
                    }
                }
                lRV = Miaxis.zzGetTz_ISO(MatchImgBuf, tz);
                if (lRV != 1)
                {
                    string strRet = "";
                    strRet = "Get ANSI TZ Failed,ret=" + lRV.ToString();
                    Console.WriteLine(strRet);
                    result = false;
                    //return;
                }
                lRV = Miaxis.zzVerifyFingerPrint_ISO(tz, tz, SafeLevel);
                if (lRV == 1)
                {

                    Console.WriteLine("Match Successed");
                    result = true;
                }
                else
                {
                    Console.WriteLine("Match Failed");
                    result = false;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + "abnormal");
            }
            return result;
        }

    }
}
