using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace OpenAccount.Data
{
    public class IDScanner
    {
        private byte[] buf;
        private bool isTimeOut;
        private bool isManualStop = false;
        private bool isOpen = false;
        private bool mIsRunning;
        private int deviceNum;
        private int mDeviceId = 0;
        private int res;
        private int overTime;
        private TimeSpan span;
        Config config = new Config();
        byte[] status;

        public void FrontSwallow()
        {
            buf = new byte[6];
            isTimeOut = false;
            isManualStop = false;
            mIsRunning = false;

            deviceNum = 0;
            res = -1;
            overTime = 300000;

            IDCardInfo idcardinfo = null;
            DateTime startTime = DateTime.Now;
            status = new byte[2];
            res = ScannerDLL.openDeviceList(ref deviceNum);
            res = ScannerDLL.openDevice(0);
            res = ScannerDLL.getDeviceStatus(0);
            res = ScannerDLL.backSwallow(0, 0, buf);
            res = ScannerDLL.frontSwallow(0, 1, buf);

            while (!isManualStop)
            {
                try
                {
                    span = DateTime.Now - startTime;
                    if (overTime > 0 && span.TotalMilliseconds > overTime)
                    {
                        res = -132;
                        isTimeOut = true;
                        break;
                    }
                    int iRet = ScannerDLL.getDeviceStatus(0);
                    if (iRet != 1)
                    {
                        continue;
                    }
                    iRet = ScannerDLL.cisQuery(0, status);
                    if (iRet == 0)
                    {
                        if (status[0] == 50)
                        {
                            iRet = ScannerDLL.goRadioPos(0, status);
                            if (iRet != 0) continue;
                            Thread.Sleep(500);
                            iRet = ScannerDLL.cisQuery(0, status);
                            Thread.Sleep(200);
                            if (status[0] == 51)
                            {
                                res = 0;
                                break;
                            }
                        }
                        else if (status[0] == 51)
                        {
                            res = 0;
                            break;
                        }
                    }
                }
                catch
                {
                    idcardinfo = null;
                }
            }
            mIsRunning = false;
            ScannerDLL.frontSwallow(0, 0, buf);
        }
        public void BackSwallow()
        {
            buf = new byte[6];
            res = ScannerDLL.goBackPos(0, buf);
        }
        public void RadioPos()
        {
            buf = new byte[6];
            res = ScannerDLL.goRadioPos(0, buf);
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
        public void ScanKTP()
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
        }
        private int ScanIDCard(byte[] frontPtr, byte[] backPtr, string frontFile, string backFile, ref CrtRect rect, ref IDcardImgOcrInfo idcardImgOcrInfo)
        {
            buf = new byte[6];
            int retCode = ScannerDLL.scan(mDeviceId, 648, 1050, 10);
            if (retCode != 0)
            {
                Console.WriteLine("SCANNING PHOTO FAILED");
            }
            rect.height = 648;
            rect.width = 1050;

            retCode = ScannerDLL.readImageDataAndOcr(mDeviceId, frontPtr, backPtr, ref rect, ref idcardImgOcrInfo);
            if (retCode != 0)
            {
                Console.WriteLine("SCANNING PHOTO FAILED");
                return retCode = -138;
            }
            retCode = ScannerDLL.saveBmp(frontFile, frontPtr, ref rect);
            if (retCode != 0)
            {
                Console.WriteLine("SCANNING PHOTO FAILED");
                return retCode = -138;
            }
            retCode = ScannerDLL.saveBmp(backFile, backPtr, ref rect);
            if (retCode != 0)
            {
                Console.WriteLine("SCANNING PHOTO FAILED");
                return retCode = -138;
            }
            Console.WriteLine("SCANNING PHOTO SUCCESS");
            return retCode;
        }
        public int ReturnCard()
        {
            res = -999;
            int iRet = ScannerDLL.getDeviceStatus(mDeviceId);
            if (iRet != 1)
            {
                Console.WriteLine("QUERY DEVICE STATUS FAILED");
                return -133;
            }
            buf = new byte[2];
            res = ScannerDLL.cisQuery(mDeviceId, buf);
            if (res == 0 && (buf[0] == 51 || buf[0] == 53 || buf[0] == 52 || buf[0] == 1))
            {
                Console.WriteLine("START EXITING CARD");
                buf[0] = 2;
                int retCode = ScannerDLL.goFrontPos(mDeviceId, buf);
                Thread.Sleep(5000);
                if (retCode != 0)
                {
                    Console.WriteLine("EXIT CARD FAILED");
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
                    Console.WriteLine("CLOSE ID CARD READER SUCCESSED");
                }
                else
                {
                    Console.WriteLine("CLOSE ID CARD READER FAILED");
                }
                return res;
            }
            catch
            {
                res = -131;
                Console.WriteLine("CLOSE ID CARD READER ERROR");
                return res;
            }
        }
    }
}
