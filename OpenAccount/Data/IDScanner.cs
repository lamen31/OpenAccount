﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components;
using System.IO;
using System.Drawing;
using System.Transactions;

namespace OpenAccount.Data
{
    public class IDScanner
    {
        private byte[] buf;
        private bool isTimeOut;
        public bool isManualStop = false;
        public bool test = false;
        private bool isOpen = false;
        private bool mIsRunning;
        private int deviceNum;
        private int mDeviceId = 0;
        private int res;
        private int overTime;
        NavigationManager navman;
        private TimeSpan span;
        TransaksiBaru trxbaru = new TransaksiBaru();
        Config config = new Config();
        byte[] status;

        public bool Initialize()
        {
            bool result = false;

            deviceNum = 0;
            res = -1;

            res = ScannerDLL.openDeviceList(ref deviceNum);
            if (res == 0)
                result = true;
            else
                result = false;
            res = ScannerDLL.openDevice(0);
            if (res == 0)
                result = true;
            else
                result = false;
            
            return result;
        }

        public async Task FrontSwallow()
        {
            buf = new byte[6];
            isTimeOut = false;
            isManualStop = false;
            mIsRunning = false;
            bool result = false;

            deviceNum = 0;
            res = -1;
            overTime = 300000;

            IDCardInfo idcardinfo = null;
            DateTime startTime = DateTime.Now;
            status = new byte[2];
            res = ScannerDLL.openDeviceList(ref deviceNum);
            if (res == 0)
                Utility.WriteLog("ID scanner condition : open device list success", "step-action");
            else
                Utility.WriteLog("ID scanner condition : open device list failed", "step-action");
            res = ScannerDLL.openDevice(0);
            if(res == 0)
                Utility.WriteLog("ID scanner condition : open device success", "step-action");
            else
                Utility.WriteLog("ID scanner condition : open device failed", "step-action");
            res = ScannerDLL.getDeviceStatus(0);
            if(res == 1)
                Utility.WriteLog("ID scanner condition : get status success", "step-action");
            else
                Utility.WriteLog("ID scanner condition : get status failed", "step-action");
            res = ScannerDLL.backSwallow(0, 0, buf);
            if (res == 0)
                Utility.WriteLog("ID scanner condition : set back entry success", "step-action");
            else
                Utility.WriteLog("ID scanner condition : set back entry failed", "step-action");
            res = ScannerDLL.frontSwallow(0, 1, buf);
            if(res == 0)
                Utility.WriteLog("ID scanner condition : set front entry success", "step-action");
            else
                Utility.WriteLog("ID scanner condition : set front entry failed", "step-action");

            while (!isManualStop)
            {
                try
                {
                    await Task.Delay(100);
                    span = DateTime.Now - startTime;
                    if (overTime > 0 && span.TotalMilliseconds > overTime)
                    {
                        res = -132;
                        isTimeOut = true;
                        Utility.WriteLog("ID scanner condition : device timeout", "step-action");
                        navman.NavigateTo("/");
                        break;
                    }
                    int iRet = ScannerDLL.getDeviceStatus(0);
                    if (iRet != 1)
                    {
                        Utility.WriteLog("ID scanner condition : get status failed", "step-action");
                        continue;
                    }
                    //Utility.WriteLog("ID scanner condition : get status success", "step-action");
                    iRet = ScannerDLL.cisQuery(0, status);
                    if (iRet == 0)
                    {
                        //Utility.WriteLog("ID scanner condition : ics checking success", "step-action");
                        switch (status[0])
                        {
                            case 50:
                                {
                                    Utility.WriteLog("ID scanner condition : the card is at the front card holding", "step-action");
                                    iRet = ScannerDLL.goRadioPos(0, status);
                                    if (iRet != 0)
                                    {
                                        Utility.WriteLog("ID scanner condition : move to the RF card holding failed", "step-action");
                                        break;
                                    }
                                    Thread.Sleep(500);
                                    iRet = ScannerDLL.cisQuery(0, status);
                                    Thread.Sleep(200);
                                    if (status[0] == 51)
                                    {
                                        Utility.WriteLog("ID scanner condition : the card is at RF card reading", "step-action");
                                        res = 0;
                                        isManualStop = true;
                                        test = true;
                                        result = isManualStop;
                                        break;
                                    }
                                    break;
                                }
                            case 51:
                                {
                                    Utility.WriteLog("ID scanner condition : the card is at RF card reading", "step-action");
                                    res = 0;
                                    isManualStop = true;
                                    test = true;
                                    result = isManualStop;
                                    break;
                                }
                            //case 48:
                            //    {
                            //        Utility.WriteLog("ID scanner condition : no card and no card inserted", "step-action");
                            //        break;
                            //    }
                            case 49:
                                {
                                    Utility.WriteLog("ID scanner condition : the card movement needs to be detected according to the state of optical sensor", "step-action");
                                    break;
                                }
                            case 52:
                                {
                                    Utility.WriteLog("ID scanner condition : the card is at the rear card holding", "step-action");
                                    break;
                                }
                            case 53:
                                {
                                    Utility.WriteLog("ID scanner condition : the card is illegally positioned", "step-action");
                                    break;
                                }
                        }
                    }
                    else
                    {
                        Utility.WriteLog("ID scanner condition : ics checking failed", "step-action");
                    }
                }
                catch
                {
                    idcardinfo = null;
                    Utility.WriteLog("ID scanner condition : read card id information error", "step-action");
                }
            }
            mIsRunning = false;
            res = ScannerDLL.frontSwallow(0, 0, buf);
            if (res == 0)
                Utility.WriteLog("ID scanner condition : set front entry success", "step-action");
            else
                Utility.WriteLog("ID scanner condition : set front entry failed", "step-action");
            //return result;
        }
        public void BackPos()
        {
            buf = new byte[6];
            res = ScannerDLL.goBackPos(0, buf);
            if (res == 0)
                Utility.WriteLog("ID scanner condition : set back entry success", "step-action");
            else
                Utility.WriteLog("ID scanner condition : set back entry failed", "step-action");
        }
        public void RadioPos()
        {
            buf = new byte[6];
            res = ScannerDLL.goRadioPos(0, buf);
            if(res == 0)
                Utility.WriteLog("ID scanner condition : move to the RF card holding success", "step-action");
            else
                Utility.WriteLog("ID scanner condition : move to the RF card holding failed", "step-action");
        }
        public void Scan()
        {
            string pathsaveimage = config.Read("PATH", Config.PARAM_PATH_IMAGE_SAVESCANNER);
            buf = new byte[6];
            DateTime dateTimeImage = DateTime.Now;
            string strImageZhengmianFile = AppDomain.CurrentDomain.BaseDirectory + "idcard_zhengmian.BMP";
            string strImageBeimianFile = AppDomain.CurrentDomain.BaseDirectory + "idcard_beimian.BMP";
            string strImageUpFile = pathsaveimage + dateTimeImage.ToString("yyyyMMddHHmm") + "_UP.BMP";
            string strImageBotFile = pathsaveimage + dateTimeImage.ToString("yyyyMMddHHmm") + "_BOT.BMP";
            IDCardInfo idCardInfo = new IDCardInfo();
            byte[] frontPtr = new byte[5242880];
            byte[] backPtr = new byte[5242880];
            CrtRect rect = new CrtRect();
            IDcardImgOcrInfo ocrInfo = new IDcardImgOcrInfo();
            res = ScanIDCard(frontPtr, backPtr, strImageUpFile, strImageBotFile, ref rect, ref ocrInfo);
            if (ocrInfo.idNum.Length > 0)
            {
                idCardInfo.ocrValiddate = Encoding.UTF8.GetString(ocrInfo.dateLimit).Replace("\0", "").Replace(" ", "");
            }
            if (ocrInfo.dateLimit.Length > 0)
            {
                idCardInfo.ocrIDnumber = Encoding.UTF8.GetString(ocrInfo.idNum).Replace("\0", "").Replace(" ", "");
            }
            idCardInfo.frontPictureBase64 = ImageHelper.GetBase64FromImage(strImageUpFile);
            idCardInfo.backPictureBase64 = ImageHelper.GetBase64FromImage(strImageBotFile);

            res = ReturnCard();
            res = CloseCVRReader();
        }
        public int ScanKTP(string strfile)
        {
            string strBase64 = string.Empty;
            string directori = Directory.GetCurrentDirectory();
            string pathsaveimage = config.Read("PATH", Config.PARAM_PATH_IMAGE_SAVESCANNER);
            string strImageUpFile = string.Empty;
            string strImageBotFile = string.Empty;
            pathsaveimage = directori +"\\"+ pathsaveimage;
            buf = new byte[6];
            DateTime dateTimeImage = DateTime.Now;
            string strImageZhengmianFile = AppDomain.CurrentDomain.BaseDirectory + "idcard_zhengmian.BMP";
            string strImageBeimianFile = AppDomain.CurrentDomain.BaseDirectory + "idcard_beimian.BMP";
            if (strfile == "KTP")
            {
                strImageUpFile = pathsaveimage + "KTP_UP.BMP";
                strImageBotFile = pathsaveimage + "KTP_BOT.BMP";
            }
            else if(strfile == "NPWP")
            {
                strImageUpFile = pathsaveimage + "NPWP_UP.BMP";
                strImageBotFile = pathsaveimage + "NPWP_BOT.BMP";
            }
            File.Delete(strImageUpFile);
            Utility.WriteLog("ID scanner condition : file from " + strImageUpFile + " has been deleted", "step-action");
            File.Delete(strImageBotFile);
            Utility.WriteLog("ID scanner condition : file from " + strImageBotFile + " has been deleted", "step-action");
            IDCardInfo idCardInfo = new IDCardInfo();
            byte[] frontPtr = new byte[5242880];
            byte[] backPtr = new byte[5242880];
            CrtRect rect = new CrtRect();
            IDcardImgOcrInfo ocrInfo = new IDcardImgOcrInfo();
            res = ScanIDCard(frontPtr, backPtr, strImageUpFile, strImageBotFile, ref rect, ref ocrInfo);
            if (ocrInfo.idNum.Length > 0)
            {
                idCardInfo.ocrValiddate = Encoding.UTF8.GetString(ocrInfo.dateLimit).Replace("\0", "").Replace(" ", "");
            }
            if (ocrInfo.dateLimit.Length > 0)
            {
                idCardInfo.ocrIDnumber = Encoding.UTF8.GetString(ocrInfo.idNum).Replace("\0", "").Replace(" ", "");
            }
            idCardInfo.frontPictureBase64 = ImageHelper.GetBase64FromImage(strImageUpFile);
            idCardInfo.backPictureBase64 = ImageHelper.GetBase64FromImage(strImageBotFile);
            if(strfile == "KTP")
            {
                strBase64 = convertToBase64(strImageUpFile);
                trxbaru.setImageKTP(strBase64);
                Utility.WriteLog("ID scanner condition : set image KTP base64 success", "step-action");
            }
            else if(strfile == "NPWP")
            {
                strBase64 = convertToBase64(strImageUpFile);
                trxbaru.setImageNPWP(strBase64);
                Utility.WriteLog("ID scanner condition : set image NPWP base64 success", "step-action");
            }
            return res;
        }

        private string convertToBase64(string strpath)
        {
            string base64String = string.Empty;
            var path = strpath;
            using(Image image = Image.FromFile(path))
            {
                using(MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                    Utility.WriteLog("ID scanner condition : convert image to base64 success", "step-action");
                }
            }
            return base64String;
        }

        public void DeleteScan(string strfile)
        {
            string directori = Directory.GetCurrentDirectory();
            string pathsaveimage = config.Read("PATH", Config.PARAM_PATH_IMAGE_SAVESCANNER);
            string strImageUpFile = string.Empty;
            string strImageBotFile = string.Empty;
            pathsaveimage = directori + "\\" + pathsaveimage;
            if (strfile == "KTP")
            {
                strImageUpFile = pathsaveimage + "KTP_UP.BMP";
                strImageBotFile = pathsaveimage + "KTP_BOT.BMP";
            }
            else if (strfile == "NPWP")
            {
                strImageUpFile = pathsaveimage + "NPWP_UP.BMP";
                strImageBotFile = pathsaveimage + "NPWP_BOT.BMP";
            }
            File.Delete(strImageUpFile);
            Utility.WriteLog("ID scanner condition : file from " + strImageUpFile + " has been deleted", "step-action");
            File.Delete(strImageBotFile);
            Utility.WriteLog("ID scanner condition : file from " + strImageBotFile + " has been deleted", "step-action");
        }
        public int ScanIDCard(byte[] frontPtr, byte[] backPtr, string frontFile, string backFile, ref CrtRect rect, ref IDcardImgOcrInfo idcardImgOcrInfo)
        {
            buf = new byte[6];
            int retCode = ScannerDLL.scan(mDeviceId, 648, 1050, 10);
            if (retCode != 0)
            {
                Utility.WriteLog("ID scanner condition : scanning photo failed", "step-action");
            }
            rect.height = 648;
            rect.width = 1050;

            retCode = ScannerDLL.readImageDataAndOcr(mDeviceId, frontPtr, backPtr, ref rect, ref idcardImgOcrInfo);
            if (retCode != 0)
            {
                Utility.WriteLog("ID scanner condition : scanning photo failed", "step-action");
                return retCode = -138;
            }
            retCode = ScannerDLL.saveBmp(frontFile, frontPtr, ref rect);
            if (retCode != 0)
            {
                Utility.WriteLog("ID scanner condition : scanning photo failed", "step-action");
                return retCode = -138;
            }
            retCode = ScannerDLL.saveBmp(backFile, backPtr, ref rect);
            if (retCode != 0)
            {
                Utility.WriteLog("ID scanner condition : scanning photo failed", "step-action");
                return retCode = -138;
            }
            Utility.WriteLog("ID scanner condition : scanning photo success", "step-action");
            return retCode;
        }
        public int ReturnCard()
        {
            res = -999;
            int iRet = ScannerDLL.getDeviceStatus(mDeviceId);
            if (iRet != 1)
            {
                Utility.WriteLog("ID scanner condition : query device status failed", "step-action");
                return -133;
            }
            else
            {
                Utility.WriteLog("ID scanner condition : query device status success", "step-action");
            }
            buf = new byte[2];
            res = ScannerDLL.cisQuery(mDeviceId, buf);
            if (res == 0)
            {
                Utility.WriteLog("ID scanner condition : start exiting card", "step-action");
                buf[0] = 2;
                int retCode = ScannerDLL.goFrontPos(mDeviceId, buf);
                Thread.Sleep(5000);
                if (retCode != 0)
                {
                    Utility.WriteLog("ID scanner condition : exit card failed", "step-action");
                }
                else
                {
                    Utility.WriteLog("ID scanner condition : exit card success", "step-action");
                }
            }
            return res;
        }
        public int CloseCVRReader()
        {
            res = -999;
            try
            {
                isManualStop = true;
                isOpen = true;
                buf = new byte[2];
                res = ScannerDLL.frontSwallow(0, 0, buf);
                res = ScannerDLL.closeCardRead(mDeviceId);
                if (res == 0)
                {
                    Utility.WriteLog("ID scanner condition : close id card reader success", "step-action");
                }
                else
                {
                    Utility.WriteLog("ID scanner condition : close id card reader failed", "step-action");
                }
                return res;
            }
            catch
            {
                res = -131;
                Utility.WriteLog("ID scanner condition : close id card reader error", "step-action");
                return res;
            }
        }
    }
}
