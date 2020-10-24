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
        byte[] ANSItz = new byte[1024];
        byte[] ISOtz = new byte[1024];

        public bool OpenDevice()
        {
            bool result = false;
            DeviceHandle = FingerDLL.FpStdP41M1_OpenDevice();
            if(DeviceHandle > 0)
            {
                Utility.WriteLog("Finger print condition : open device successed", "step-action");
                result = true;
            }
            else
            {
                Utility.WriteLog("Finger print condition : open device failed", "step-action");
                result = false;
            }
            return result;
        }

        private void GetTZ()
        {
            //ISOtz = null;
            //int lRV = Miaxis.zzGetTz_ISO(byteminutiae, ISOtz);
            //if (lRV != 1)
            //{
            //    string strRet = "";
            //    strRet = "Get ISO TZ Failed,ret=" + lRV.ToString();
            //    Console.WriteLine(strRet);
            //    return;
            //}
            //Console.WriteLine("Get ISO TZ Successed");

            int lRV = Miaxis.zzGetTz256x360_ANSI(FingerBuf, ANSItz);
            if (lRV != 1)
            {
                string strRet = "";
                strRet = "Get ANSI TZ Failed,ret=" + lRV.ToString();
                Utility.WriteLog("Finger print condition : " + strRet, "step-action");
                return;
            }
            Utility.WriteLog("Finger print condition : get ANZI TZ successed", "step-action");
        }

        public bool MatchFinger(string strminutiae1, string strminutiae2)
        {
            bool result = false;
            result = Match_Func(strminutiae1, strminutiae2);
            return result;
        }

        private bool Match_Func(string strminutiae1, string strminutiae2)
        {
            bool result = false;
            try
            {
                bool loopFinger = true;
                int lRV = -1;
                byte[] MatchImgBuf = new byte[256 * 360]; //Image raw Buffer
                byte[] bmpFingerBuf = new byte[256 * 360 + 1078]; //Image bmp Buffer
                byte[] tz = new byte[1024];
                string[] minutiae1 = strminutiae1.Split(' ');
                string[] minutiae2 = strminutiae2.Split(' ');
                byte[] byteminutiae1 = new byte[256 * 360];
                byte[] byteminutiae2 = new byte[256 * 360];
                //minutiae1 = DeleteChar(strminutiae1);
                int i = 0;
                foreach (string minu1 in minutiae1)
                {
                    byteminutiae1[i] = byte.Parse(minu1, System.Globalization.NumberStyles.HexNumber);

                    i++;
                }
                i = 0;
                foreach (string minu2 in minutiae2)
                {
                    byteminutiae2[i] = byte.Parse(minu2, System.Globalization.NumberStyles.HexNumber);

                    i++;
                }
                //byteminutiae1 = StringToByteArray(strIns(minutiae1[0], ","));
                //byteminutiae2 = StringToByteArray(strminutiae2);
                int SafeLevel = 2;
                while (loopFinger)
                {
                    lRV = FingerDLL.FpStdP41M1_GetImage(0, MatchImgBuf);
                    if (lRV != 1)
                    {
                        string strRet = "";
                        strRet = "Get Image Failed,ret=" + lRV.ToString();
                        Utility.WriteLog("Finger print condition : " + strRet, "step-action");
                        result = false;
                        //return;
                    }
                    else
                    {
                        int AreaScore = FingerDLL.FpStdP41M1_IsFinger(0, MatchImgBuf);
                        if (AreaScore < 60)
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
                Task.Delay(500);
                if (lRV != 1)
                {
                    string strRet = "";
                    strRet = "Get ANSI TZ Failed,ret=" + lRV.ToString();
                    Utility.WriteLog("Finger print condition : " + strRet, "step-action");
                    result = false;
                    //return;
                }
                FingerBuf = byteminutiae1;
                //GetTZ();
                lRV = Miaxis.zzVerifyFingerPrint_ISO(FingerBuf, tz, SafeLevel);
                Task.Delay(500);
                if (lRV == 1)
                {
                    Utility.WriteLog("Finger print condition : match successed", "step-action");
                    result = true;
                }
                else
                {
                    Utility.WriteLog("Finger print condition : match failed", "step-action");
                    FingerBuf = byteminutiae2;
                    //GetTZ();
                    lRV = Miaxis.zzVerifyFingerPrint_ISO(FingerBuf, tz, SafeLevel);

                    if (lRV == 1)
                    {
                        Utility.WriteLog("Finger print condition : match successed", "step-action");
                        result = true;
                    }
                    else
                    {
                        Utility.WriteLog("Finger print condition : match failed", "step-action");
                        result = false;
                    }
                }
            }
            catch(Exception ex)
            {
                Utility.WriteLog("Finger print condition : " + ex.Message + " abnormal", "step-action");
            }
            return result;
        }

        private static string DeleteChar(string strdata)
        {
            string strRet = strdata;
            for(int i = strRet.Length-2; i > 0; i -= 2)
            {
                strRet = strRet.Remove(i);
            }
            return strRet;
        }

        private static string strIns(string strData, string strInsert)
        {
            int i = 0;
            string strRet = strData;
            try
            {
                for (i = strRet.Length - 2; i > 0; i -= 2)
                {
                    strRet = strRet.Insert(i, strInsert);
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog("Finger print condition : " + ex.ToString(), "step-action");
            }
            return strRet;
        }

        private static byte[] StringToByteArray(string p_str)
        {
            byte[] result = null;

            try
            {
                p_str = p_str.Replace(Environment.NewLine, "");
                p_str = p_str.Replace("0x", "");
                p_str = p_str.Replace(" ", "");

                string[] x_bytes = p_str.Split(" ");

                result = new byte[x_bytes.Length];
                int i = 0;
                foreach (string x_byte in x_bytes)
                {
                    result[i] = byte.Parse(x_byte, System.Globalization.NumberStyles.HexNumber);

                    i++;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog("Finger print condition : " + ex.ToString(), "step-action");
                result = null;
            }

            return result;
        }

        public void CloseDevice()
        {
            FingerDLL.FpStdP41M1_CloseDevice(DeviceHandle);
            Utility.WriteLog("Finger print condition : close device successed", "step-action");
        }

    }
}
