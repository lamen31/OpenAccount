using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Configuration;
using Aratek.TrustFinger;
using System.IO;
using Microsoft.Extensions.Logging;

namespace OpenAccount.Data
{
    public class EKtp
    {
        private string txtbxSAMCONF;
        private string txtbxSAMPCID;
        private string pMessage;
        private string msg = string.Empty;
        private bool SAMStatus1 = false;
        private bool SAMStatus2 = false;
        Config config = new Config();
        EKTP ektp = new EKTP();

        public void EKtpInitialize()
        {
            string SAMCONF = config.Read("EKTP", Config.PARAM_READER_SAMPCONF);
            string SAMPCID = config.Read("EKTP", Config.PARAM_READER_SAMPCID);
            try
            {
                txtbxSAMCONF = ConfigurationManager.AppSettings[SAMCONF];
                txtbxSAMPCID = ConfigurationManager.AppSettings[SAMPCID];
            }
            catch
            {
                pMessage = "Initialize Configuration Error.";
                Console.WriteLine(pMessage);
            }
            InitializeContext();
            ListReaders();
            SAMActivation();
        }

        private void InitializeContext()
        {
            try
            {
                int rc = -1;
                UInt16 n = 0;

                rc = EKtpDLL.InitializeContext(ref n);

                if (rc == 0 && n > 0)
                {
                    pMessage = "Find " + n.ToString("G") + " Reader";
                    Console.WriteLine(pMessage);
                }
                else
                {
                    pMessage = "No Reader is Found";
                    Console.WriteLine(pMessage);
                }
            }
            catch
            {
                pMessage = "Error = Initialize Card Reader";
                Console.WriteLine(pMessage);
            }
        }

        private void ListReaders()
        {
            int indx, rc = -1;
            string rName = "";
            try
            {
                for (int n = 0; n < 5; n++)
                {
                    rName = "";
                    byte[] ReadersName = new byte[255];
                    UInt16 ReadersNameLen = 0;
                    rc = EKtpDLL.GetSCardReaderName((UInt16)n, ref ReadersNameLen, ReadersName);
                    if (rc == 0)
                    {
                        rName = "";
                        indx = 0;
                        while (ReadersName[indx] != 0)
                        {
                            rName = rName + (char)ReadersName[indx];
                            indx = indx + 1;
                        }

                        // Add reader name to list
                        pMessage = rName;
                        Console.WriteLine(pMessage);
                    }
                }
            }
            catch
            {
                pMessage = "Error = Listing Card Reader";
                Console.WriteLine(pMessage);
                return;
            }
        }

        private void SAMActivation()
        {
            //Cek SAM Slot 1
            try
            {
                SAMStatus1 = SAMReaderSlot(ConfigurationManager.AppSettings["SLOT_1"]);
                if (SAMStatus1)
                {
                    pMessage = "SAM Slot 1 OK";
                    Console.WriteLine(pMessage);
                }
                else
                {
                    pMessage = "SAM Slot 1 NOT OK";
                    Console.WriteLine(pMessage);
                }
            }
            catch (Exception ex)
            {
                pMessage = "Error = SAM Slot 1";
                Console.WriteLine(pMessage);
                return;
            }

            //Cek SAM Slot 2
            try
            {
                SAMStatus2 = SAMReaderSlot(ConfigurationManager.AppSettings["SLOT_2"]);
                if (SAMStatus2)
                {
                    pMessage = "SAM Slot 2 OK";
                    Console.WriteLine(pMessage);
                }
                else
                {
                    pMessage = "SAM Slot 2 NOT OK";
                    Console.WriteLine(pMessage);
                }
            }
            catch (Exception ex)
            {
                pMessage = "Error = SAM Slot 2";
                Console.WriteLine(pMessage);
                return;
            }

            if (SAMStatus1)
            {
                SAMReaderSlot(ConfigurationManager.AppSettings["SLOT_1"]);
                pMessage = "SAM Card Slot 1 Open";
                Console.WriteLine(pMessage);
            }
            else if (SAMStatus2)
            {
                SAMReaderSlot(ConfigurationManager.AppSettings["SLOT_2"]);
                pMessage = "SAM Card Slot 2 Open";
                Console.WriteLine(pMessage);
            }
            else
            {
                pMessage = "Cannot Open SAM Card. Please Insert SAM Card.";
                Console.WriteLine(pMessage);
            }
        }

        private static bool SAMReaderSlot(string strData5)
        {
            Config config = new Config();
            int rc;
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));
            bool SAMStatus = false;
            UInt16 TxDataLen, RxDataLen;
            byte[] TxData = new byte[1024];
            byte[] RxData = new byte[1024];

            RxDataLen = 0;

            TxData[0] = 0x68;
            TxData[1] = 0x92;
            TxData[2] = 0x01;
            TxData[3] = 0x00;
            TxData[4] = 0x03;
            TxData[5] = byte.Parse(strData5, System.Globalization.NumberStyles.HexNumber); ;
            TxData[6] = 0x00;
            TxData[7] = 0x00;
            TxDataLen = 8;

            try
            {
                rc = EKtpDLL.Extended_Transmit((UInt16)SAMreader, TxDataLen, TxData, ref RxDataLen, RxData);

                if (RxData[RxDataLen - 2] == 0x90 && RxData[RxDataLen - 1] == 0x00)
                {
                    SAMStatus = true;
                }
                else
                {
                    SAMStatus = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                SAMStatus = false;
            }
            return SAMStatus;
        }

        public void EKtpReader()
        {
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));

            Stopwatch sw = Stopwatch.StartNew();
            ConnectRF();
            Console.WriteLine("Time taken ALL1: {0}ms", sw.Elapsed.TotalMilliseconds);
            ConnectSAM();
            Console.WriteLine("Time taken ALL2: {0}ms", sw.Elapsed.TotalMilliseconds);
            UIDA();
            Console.WriteLine("Time taken ALL3: {0}ms", sw.Elapsed.TotalMilliseconds);
            SelDF();
            Console.WriteLine("Time taken ALL4: {0}ms", sw.Elapsed.TotalMilliseconds);
            EFPhoto();
            Console.WriteLine("Time taken ALL5: {0}ms", sw.Elapsed.TotalMilliseconds);
            ReadPhoto();
            Console.WriteLine("Time taken ALL6: {0}ms", sw.Elapsed.TotalMilliseconds);
            UIDB();
            Console.WriteLine("Time taken ALL7: {0}ms", sw.Elapsed.TotalMilliseconds);
            EFCC();
            Console.WriteLine("Time taken ALL8: {0}ms", sw.Elapsed.TotalMilliseconds);
            READCC();
            Console.WriteLine("Time taken ALL9: {0}ms", sw.Elapsed.TotalMilliseconds);
            RESETSAM();
            Console.WriteLine("Time taken ALL10: {0}ms", sw.Elapsed.TotalMilliseconds);
            GETCHALLENGE();
            Console.WriteLine("Time taken ALL11: {0}ms", sw.Elapsed.TotalMilliseconds);
            CALCCHALLENGE();
            Console.WriteLine("Time taken ALL12: {0}ms", sw.Elapsed.TotalMilliseconds);
            EXTAUTH();
            Console.WriteLine("Time taken ALL13: {0}ms", sw.Elapsed.TotalMilliseconds);
            INTAUTH();
            Console.WriteLine("Time taken ALL14: {0}ms", sw.Elapsed.TotalMilliseconds);
            SELECTEFDS();
            Console.WriteLine("Time taken ALL15: {0}ms", sw.Elapsed.TotalMilliseconds);
            DIGITALSIGNATURE();
            Console.WriteLine("Time taken ALL16: {0}ms", sw.Elapsed.TotalMilliseconds);
            BIODATA();
            Console.WriteLine("Time taken ALL17: {0}ms", sw.Elapsed.TotalMilliseconds);
            RETPHOTO();
            Console.WriteLine("Time taken ALL18: {0}ms", sw.Elapsed.TotalMilliseconds);
            AutoDecip();
            Console.WriteLine("Time taken ALL19: {0}ms", sw.Elapsed.TotalMilliseconds);
            Signature();
            Console.WriteLine("Time taken ALL20: {0}ms", sw.Elapsed.TotalMilliseconds);
            Minutiae1();
            Console.WriteLine("Time taken ALL21: {0}ms", sw.Elapsed.TotalMilliseconds);
            Minutiae2();
            Console.WriteLine("Time taken ALL22: {0}ms", sw.Elapsed.TotalMilliseconds);
            EKtpDLL.DisconnectSCardReader((UInt16)SAMreader);
            EKtpDLL.DisconnectSCardReader((UInt16)RFreader);
        }

        private void ConnectRF()
        {
            Stopwatch sw = Stopwatch.StartNew();
            bool RFStatus = true;
            try
            {
                RFStatus = ConnectRFReader();
            }
            catch
            {
                pMessage = "Error = Cannot Connect to Card Reader";
                Console.WriteLine(pMessage);
                return;
            }

            if (RFStatus)
            {
                pMessage = "RF Reader Connected";
                Console.WriteLine(pMessage);
            }
            else
            {
                pMessage = "RF Reader Error";
                Console.WriteLine(pMessage);
            }
            sw.Stop();
            Console.WriteLine("Time taken RF: {0}ms", sw.Elapsed.TotalMilliseconds);
        }

        private static bool ConnectRFReader()
        {
            int rc;
            bool RFStatus = false;
            try
            {
                rc = EKtpDLL.ConnectSCardReader(0);
                if (rc == 0)
                {
                    RFStatus = true;
                }
                else
                {
                    RFStatus = false;
                }
            }
            catch (Exception ex)
            {
                string pMessage = ex.ToString();
                Console.WriteLine(pMessage);
                RFStatus = false;
            }
            return RFStatus;
        }

        private void ConnectSAM()
        {
            Stopwatch sw = Stopwatch.StartNew();
            byte[] byteSAM = NEW_XOR(strIns(txtbxSAMPCID, ","), strIns(txtbxSAMCONF, ","));
            UInt16 RxDataLen = 0;
            byte[] RxData = new byte[1024];
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));

            byte[] byteCOM = new byte[Config.OPENSAM.Length + byteSAM.Length];
            Buffer.BlockCopy(Config.OPENSAM, 0, byteCOM, 0, 5);
            Buffer.BlockCopy(byteSAM, 0, byteCOM, 5, 32);
            try
            {
                int rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                sw.Stop();
                msg = "Open SAM KTP-el";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);
            }
            catch
            {
                pMessage = "Error = Cannot Open SAM KTP-el";
                Console.WriteLine(pMessage);
                return;
            }
            Console.WriteLine("Time taken SAM: {0}ms", sw.Elapsed.TotalMilliseconds);
        }

        public static byte[] NEW_XOR(string strpcid, string strconf)
        {
            byte[] bytepcid = new byte[16];
            bytepcid = StringToByteArray(strpcid);
            byte[] byteconf = new byte[32];
            byteconf = StringToByteArray(strconf);
            byte[] apdu = new byte[32];

            for (int i = 0; i < 16; i++)
            {
                apdu[i] = (byte)(bytepcid[i] ^ byteconf[i]);
            }

            for (int i = 0; i < 16; i++)
            {
                apdu[16 + i] = (byte)(bytepcid[i] ^ byteconf[16 + i]);
            }

            return apdu;
        }

        private static byte[] StringToByteArray(string p_str)
        {
            byte[] result = null;

            try
            {
                p_str = p_str.Replace(Environment.NewLine, "");
                p_str = p_str.Replace("0x", "");
                p_str = p_str.Replace(" ", "");

                string[] x_bytes = p_str.Split(',');

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
                Console.WriteLine(ex.ToString());
                result = null;
            }

            return result;
        }

        private static string ByteArrayToString(byte[] p_bytes, int pbytes)
        {
            string result = string.Empty;

            try
            {
                for (int i = 0; i < pbytes; i++)
                {
                    result = result + p_bytes[i].ToString("X02");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return result;
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
                Console.WriteLine(ex.ToString());
            }
            return strRet;
        }

        private void UIDA()
        {
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                byte[] byteCOM = Config.UIDA;
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];

                int rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                if (RxData[RxDataLen - 2] == 0x90 && RxData[RxDataLen - 1] == 0x00)
                {
                    ektp.UID[0] = 0x80;
                    Buffer.BlockCopy(RxData, 0, ektp.UID, 1, RxDataLen - 2);
                }

                sw.Stop();
                msg = "Get UID Type A";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);
                Console.WriteLine("Time taken UIDA: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Get UID Type A");
                return;
            }
        }

        private void SelDF()
        {
            Stopwatch sw = Stopwatch.StartNew();
            UInt16 RxDataLen = 0;
            byte[] RxData = new byte[1024];
            byte[] byteCOM = Config.DFEKTP;
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
        Repeat1:
            try
            {
                int rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                if (RxData[RxDataLen - 2] != 0x90 && RxData[RxDataLen - 1] != 0x00)
                {
                    goto Repeat1;
                }

                sw.Stop();
                msg = "Select DF KTP-el";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Select DF KTP-el");
                return;
            }
            Console.WriteLine("Time taken DF: {0}ms", sw.Elapsed.TotalMilliseconds);
        }
        private void EFPhoto()
        {
            Stopwatch sw = Stopwatch.StartNew();
            UInt16 RxDataLen = 0;
            byte[] RxData = new byte[1024];
            byte[] byteCOM = Config.EFPHOTO;
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));

            try
            {
                int rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                sw.Stop();
                msg = "Select EF Photograph";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Select EF Photograph");
                return;
            }
            Console.WriteLine("Time taken EF: {0}ms", sw.Elapsed.TotalMilliseconds);
        }
        private void ReadPhoto()
        {
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 len_total;
                UInt16 k = 8;
                int l = 2;

                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                //APDU Command P1, P2 And Length
                byte[] byteCOM = new byte[5] { 0x00, 0x00, 0x00, 0x00, 0x08 };


                Buffer.BlockCopy(Config.READRF, 0, byteCOM, 0, Config.READRF.Length);
                int rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "Read Photograph #1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byte[] bytePhoto = new byte[RxDataLen - 1];
                bytePhoto[0] = 0x08;
                Buffer.BlockCopy(RxData, 0, bytePhoto, 1, RxDataLen - 2);
                ektp.lbPhoto.Add(bytePhoto);

                Array.Copy(RxData, 2, ektp.bytePhoto, 0, RxDataLen - 4);
                ektp.photolen = RxDataLen - 4;
                len_total = (UInt16)((RxData[0] * 256) + RxData[1] + 2);
                int loopData = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_LOOP));
                while (k < len_total)
                {
                    UInt16 a = 0;
                    byteCOM[2] = (byte)(k / 256);
                    byteCOM[3] = (byte)(k % 256);
                    if ((len_total - k) / (UInt16)loopData <= 0)
                        a = (UInt16)(len_total - k);
                    else
                        a = (UInt16)loopData;
                    bytePhoto = new byte[a + 1];
                    byteCOM[4] = (byte)a;
                    rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                    bytePhoto[0] = (byte)a;
                    Buffer.BlockCopy(RxData, 0, bytePhoto, 1, RxDataLen - 2);
                    ektp.lbPhoto.Add(bytePhoto);
                    Array.Copy(RxData, 0, ektp.bytePhoto, ektp.photolen, RxDataLen - 2);
                    ektp.photolen = ektp.photolen + RxDataLen - 2;

                    k = (UInt16)(k + a);

                    msg = "Read Photograph #" + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);

                    l += 1;
                }
                //msg= ("PHOTO = " + OurUtility.strIns(OurUtility.ByteArrayToString(ektp.bytePhoto, ektp.photolen), " "));
                //Loglistbox.Items.Add(msg);
                sw.Stop();
                Console.WriteLine("Time taken PHOTO: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Read Photo");
                return;
            }
        }
        private void UIDB()
        {
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = Config.UIDB;

                int rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "Get UID Type B";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                if (RxData[RxDataLen - 2] == 0x90 && RxData[RxDataLen - 1] == 0x00)
                {
                    Buffer.BlockCopy(RxData, 0, ektp.UID, 0, RxDataLen - 2);
                }

                sw.Stop();
                Console.WriteLine("Time taken UIDB: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Get UID Type B");
            }
        }
        private void EFCC()
        {
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = Config.EFCC;

                int rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "Select EF Card Control";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                sw.Stop();
                Console.WriteLine("Time taken CC: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Select EF Card Cotrol");
                return;
            }
        }
        private void READCC()
        {
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = Config.CC;

                int rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "Card Contol";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                ektp.CC_LEN = (UInt16)((RxData[0] * 256) + RxData[1]);
                ektp.CC = new byte[ektp.CC_LEN];
                Buffer.BlockCopy(RxData, 2, ektp.CC, 0, ektp.CC_LEN);
                sw.Stop();
                Console.WriteLine("Time taken CC: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Get Card Control");
                return;
            }
        }
        private void RESETSAM()
        {
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = Config.SAMRESET;

                int rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "Reset SAM";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                sw.Stop();
                Console.WriteLine("Time taken RESET: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Reset SAM");
                return;
            }
        }
        private void GETCHALLENGE()
        {
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = Config.GETCHAL;

                int rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "Get Challenge";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                ektp.GETCHAL_LEN = (UInt16)(RxDataLen - 2);
                ektp.GETCHAL = new byte[ektp.GETCHAL_LEN];
                Buffer.BlockCopy(RxData, 0, ektp.GETCHAL, 0, ektp.GETCHAL_LEN);
                sw.Stop();
                Console.WriteLine("Time taken GETC: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Get Challenge");
                return;
            }
        }
        private void CALCCHALLENGE()
        {
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                ektp.CALCHAL_LEN = (UInt16)(ektp.CC_LEN + ektp.UID.Length + ektp.GETCHAL_LEN);
                ektp.CALCHAL = new byte[ektp.CALCHAL_LEN];
                Buffer.BlockCopy(ektp.CC, 0, ektp.CALCHAL, 0, ektp.CC_LEN);
                Buffer.BlockCopy(ektp.UID, 0, ektp.CALCHAL, ektp.CC_LEN, ektp.UID.Length);
                Buffer.BlockCopy(ektp.GETCHAL, 0, ektp.CALCHAL, ektp.CC_LEN + ektp.UID.Length, ektp.GETCHAL_LEN);
                byte[] byteCOM = new byte[Config.CALCHAL1D.Length + 1 + ektp.CALCHAL_LEN];
                if (ektp.CC_LEN == 51)
                    Buffer.BlockCopy(Config.CALCHAL33, 0, byteCOM, 0, Config.CALCHAL33.Length);
                else
                    Buffer.BlockCopy(Config.CALCHAL1D, 0, byteCOM, 0, Config.CALCHAL1D.Length);
                byteCOM[Config.CALCHAL1D.Length] = (byte)ektp.CALCHAL_LEN;
                Buffer.BlockCopy(ektp.CALCHAL, 0, byteCOM, Config.CALCHAL1D.Length + 1, ektp.CALCHAL_LEN);
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];

                int rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "SAM Calculate Challenge";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                Config.PARAM_EXT_AUTH = ByteArrayToString(RxData, RxDataLen - 2);
                Config.PARAM_EXT_AUTH_LEN = RxDataLen - 2;

                ektp.EXTAUTH_LEN = (UInt16)(RxDataLen - 2);
                ektp.EXTAUTH = new byte[ektp.EXTAUTH_LEN];
                Buffer.BlockCopy(RxData, 0, ektp.EXTAUTH, 0, ektp.EXTAUTH_LEN);

                sw.Stop();
                Console.WriteLine("Time taken CALC: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Calculate Challenge");
                return;
            }
        }
        private void EXTAUTH()
        {
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = new byte[Config.EXTAUTH.Length + ektp.EXTAUTH_LEN];
                Buffer.BlockCopy(Config.EXTAUTH, 0, byteCOM, 0, Config.EXTAUTH.Length);
                Buffer.BlockCopy(ektp.EXTAUTH, 0, byteCOM, Config.EXTAUTH.Length, ektp.EXTAUTH_LEN);

                int rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "External Authenticate";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                Config.PARAM_INT_AUTH = ByteArrayToString(RxData, RxDataLen - 2);
                Config.PARAM_INT_AUTH_LEN = RxDataLen - 2;
                ektp.INTAUTH_LEN = (UInt16)(RxDataLen - 2);
                ektp.INTAUTH = new byte[ektp.INTAUTH_LEN];
                Buffer.BlockCopy(RxData, 0, ektp.INTAUTH, 0, ektp.INTAUTH_LEN);

                sw.Stop();
                Console.WriteLine("Time taken EXTA: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot External Authenticate");
                return;
            }
        }
        private void INTAUTH()
        {
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = new byte[Config.INTAUTH.Length + 1 + ektp.INTAUTH_LEN];
                Buffer.BlockCopy(Config.INTAUTH, 0, byteCOM, 0, Config.INTAUTH.Length);
                byteCOM[Config.INTAUTH.Length] = (byte)ektp.INTAUTH_LEN;
                Buffer.BlockCopy(ektp.INTAUTH, 0, byteCOM, Config.INTAUTH.Length + 1, ektp.INTAUTH_LEN);

                int rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "Internal Authenticate";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                sw.Stop();
                Console.WriteLine("Time taken INTA: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Internal Authenticate");
                return;
            }
        }
        private void SELECTEFDS()
        {
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = Config.EFDS;

                int rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "SAM Encrypt Select EF Digital Signature";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[RxDataLen - 2];
                Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "Ek (Select EF Digital Signature)";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "SAM Decrypt(Ek(Status #1))";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                sw.Stop();
                Console.WriteLine("Time taken EFDS: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Select EF Digital Signature");
                return;
            }
        }
        private void DIGITALSIGNATURE()
        {
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = Config.READDS;

                int rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken DS1: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Encrypt Digital Signature";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[RxDataLen - 2];
                Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken DS2: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "Ek(Digital Signature)";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken DS3: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Decrypt(Ek(Digital Signature))";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                ektp.RETDS_LEN = (UInt16)(RxDataLen - 4);
                ektp.RETDS = new byte[ektp.RETDS_LEN];
                Buffer.BlockCopy(RxData, 0, ektp.RETDS, 0, ektp.RETDS_LEN);

                byteCOM = new byte[Config.AUTOVERIF.Length];
                Buffer.BlockCopy(Config.AUTOVERIF, 0, byteCOM, 0, Config.AUTOVERIF.Length);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken DS4: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Start Digital Signature Automatic Verification";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[Config.RETDS.Length + 1 + ektp.RETDS_LEN];
                Buffer.BlockCopy(Config.RETDS, 0, byteCOM, 0, Config.RETDS.Length);
                byteCOM[Config.RETDS.Length] = (byte)ektp.RETDS_LEN;
                Buffer.BlockCopy(ektp.RETDS, 0, byteCOM, Config.RETDS.Length + 1, ektp.RETDS_LEN);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "SAM Retrieve Digital Signature";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                sw.Stop();
                Console.WriteLine("Time taken DS5: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Retrieve Digital Signature");
                return;
            }
        }
        private void BIODATA()
        {
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = Config.EFBIO;

                int rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken BIO1: {0}ms", sw.Elapsed.TotalMilliseconds);
                int k = 8;
                int l = 2;
                int len_total;

                msg = "SAM Encrypt Select EF Biodata";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[RxDataLen - 2];
                Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken BIO2: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "Ek(Select EF Biodata)";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken BIO3: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Decrypt(Ek(Status #2))";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byte[] byteCOMRead = new byte[Config.ENCRYPT.Length + Config.SAMREAD.Length + 3];
                Buffer.BlockCopy(Config.ENCRYPT, 0, byteCOMRead, 0, Config.ENCRYPT.Length);
                Buffer.BlockCopy(Config.SAMREAD, 0, byteCOMRead, Config.ENCRYPT.Length, Config.SAMREAD.Length);
                byteCOMRead[byteCOMRead.Length - 3] = 0x00;
                byteCOMRead[byteCOMRead.Length - 2] = 0x00;
                byteCOMRead[byteCOMRead.Length - 1] = 0x08;

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOMRead.Length, byteCOMRead, ref RxDataLen, RxData);
                Console.WriteLine("Time taken BIO4: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Encrypt Biodata #1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[RxDataLen - 2];
                Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken BIO5: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "Ek(Biodata) #1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken BIO6: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Decrypt(Ek(Biodata)) #1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                UInt16 a = 0;
                Buffer.BlockCopy(RxData, 2, ektp.byteBio, 0, RxDataLen - 6);
                ektp.biolen = RxDataLen - 6;

                len_total = (RxData[0] * 256) + RxData[1] + 2;
                int loopData2 = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_LOOP2));
                while (k < len_total)
                {
                    byteCOMRead[byteCOMRead.Length - 3] = Convert.ToByte(k / 256);
                    byteCOMRead[byteCOMRead.Length - 2] = Convert.ToByte(k % 256);
                    if ((len_total - k) / (UInt16)loopData2 <= 0)
                    {
                        a = (UInt16)(len_total - k);
                    }
                    else
                        a = (UInt16)loopData2;
                    byteCOMRead[byteCOMRead.Length - 1] = (byte)a;

                    Array.Clear(RxData, 0, RxData.Length);
                    rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOMRead.Length, byteCOMRead, ref RxDataLen, RxData);
                    Console.WriteLine("Time taken BIO7: {0}ms", sw.Elapsed.TotalMilliseconds);

                    msg = "SAM Encrypt Biodata #" + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOMRead, byteCOMRead.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);

                    k = k + a;

                    byteCOM = new byte[RxDataLen - 2];
                    Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                    Array.Clear(RxData, 0, RxData.Length);
                    rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                    Console.WriteLine("Time taken BIO8: {0}ms", sw.Elapsed.TotalMilliseconds);

                    msg = "Ek(Biodata) #" + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);

                    byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                    Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                    byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                    Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                    Array.Clear(RxData, 0, RxData.Length);
                    rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                    Console.WriteLine("Time taken BIO9: {0}ms", sw.Elapsed.TotalMilliseconds);

                    msg = "SAM Decrypt(Ek(Biodata)) #" + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);

                    Array.Copy(RxData, 0, ektp.byteBio, ektp.biolen, RxDataLen - 4);
                    ektp.biolen = ektp.biolen + RxDataLen - 4;

                    l += 1;
                }
                //msg = ("BIODATA = " + OurUtility.strIns(OurUtility.ByteArrayToString(ektp.byteBio, ektp.biolen), " "));
                //Loglistbox.Items.Add(msg);
                sw.Stop();
                Console.WriteLine("Time taken BIO10: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Retrieve Biodata");
                return;
            }
        }
        private void RETPHOTO()
        {
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                int rc = 0;
                int l = 0;
                foreach (byte[] bytePhoto in ektp.lbPhoto)
                {
                    //RETP1
                    l += 1;
                    UInt16 RxDataLen = 0;
                    byte[] RxData = new byte[1024];
                    byte[] byteCOM = new byte[Config.RETPHOTO.Length + bytePhoto.Length];
                    Buffer.BlockCopy(Config.RETPHOTO, 0, byteCOM, 0, Config.RETPHOTO.Length);
                    Console.WriteLine("Time taken RETP1: {0}ms", sw.Elapsed.TotalMilliseconds);

                    //RETP2
                    Buffer.BlockCopy(bytePhoto, 0, byteCOM, Config.RETPHOTO.Length, bytePhoto.Length);
                    Console.WriteLine("Time taken RETP2: {0}ms", sw.Elapsed.TotalMilliseconds);

                    //RETP3
                    rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                    Console.WriteLine("Time taken RETP3: {0}ms", sw.Elapsed.TotalMilliseconds);

                    msg = "SAM Retrieve Photograph #" + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);
                }

                sw.Stop();
                Console.WriteLine("Time taken RETP4: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Retrieve Photo");
                return;
            }
        }
        private void AutoDecip()
        {
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = Config.AUTODECIP;

                int rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);

                msg = "SAM Perform Automatic Deciphering";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                sw.Stop();
                Console.WriteLine("Time taken AUTOD: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Perform Automatic Deciphering");
                return;
            }
        }
        private void Signature()
        {
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = Config.SIGNATURE;

                int rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken SIGNA1: {0}ms", sw.Elapsed.TotalMilliseconds);
                int k = 8;
                int l = 2;
                int len_total;

                msg = "SAM Encrypt Select EF Signature";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[RxDataLen - 2];
                Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken SIGNA2: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "Ek(Select EF Signature)";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken SIGNA3: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Decrypt(Ek(Status #3))";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byte[] byteCOMRead = new byte[Config.ENCRYPT.Length + Config.SAMREAD.Length + 3];
                Buffer.BlockCopy(Config.ENCRYPT, 0, byteCOMRead, 0, Config.ENCRYPT.Length);
                Buffer.BlockCopy(Config.SAMREAD, 0, byteCOMRead, Config.ENCRYPT.Length, Config.SAMREAD.Length);
                byteCOMRead[byteCOMRead.Length - 3] = 0x00;
                byteCOMRead[byteCOMRead.Length - 2] = 0x00;
                byteCOMRead[byteCOMRead.Length - 1] = 0x08;

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOMRead.Length, byteCOMRead, ref RxDataLen, RxData);
                Console.WriteLine("Time taken SIGNA4: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Encrypt Read Signature #1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOMRead, byteCOMRead.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[RxDataLen - 2];
                Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken SIGNA5: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "Ek(Read Signature) #1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken SIGNA6: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Decrypt(Ek(Signature)) #1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                Array.Copy(RxData, 2, ektp.byteSignature, 0, RxDataLen - 6);
                ektp.signlen = RxDataLen - 6;

                len_total = (RxData[0] * 256) + RxData[1] + 2;
                int loopData2 = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_LOOP2));
                while (k < len_total)
                {
                    UInt16 a = 0;
                    byteCOMRead[byteCOMRead.Length - 3] = (byte)(k / 256);
                    byteCOMRead[byteCOMRead.Length - 2] = (byte)(k % 256);

                    if ((len_total - k) / (UInt16)loopData2 <= 0)
                    {
                        a = (UInt16)(len_total - k);
                        if (a % 8 > 0)
                            a = (UInt16)(((a / 8) + 1) * 8);
                    }
                    else
                        a = (UInt16)loopData2;
                    byteCOMRead[byteCOMRead.Length - 1] = (byte)a;

                    Array.Clear(RxData, 0, RxData.Length);
                    rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOMRead.Length, byteCOMRead, ref RxDataLen, RxData);
                    Console.WriteLine("Time taken SIGNA7: {0}ms", sw.Elapsed.TotalMilliseconds);
                    k = k + a;

                    msg = "SAM Encrypt Read Signature #" + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOMRead, byteCOMRead.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);

                    byteCOM = new byte[RxDataLen - 2];
                    Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                    Array.Clear(RxData, 0, RxData.Length);
                    rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                    Console.WriteLine("Time taken SIGNA8: {0}ms", sw.Elapsed.TotalMilliseconds);

                    msg = "Ek(Read Signature) #" + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);

                    byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                    Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                    byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                    Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                    Array.Clear(RxData, 0, RxData.Length);
                    rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                    Console.WriteLine("Time taken SIGNA9: {0}ms", sw.Elapsed.TotalMilliseconds);

                    msg = "SAM Decrypt(Ek(Signature)) #" + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);

                    Array.Copy(RxData, 0, ektp.byteSignature, ektp.signlen, RxDataLen - 4);
                    ektp.signlen = ektp.signlen + RxDataLen - 4;

                    l += 1;
                }
                //msg = ("SIGNATURE = " + OurUtility.strIns(OurUtility.ByteArrayToString(ektp.byteSignature, ektp.signlen), " "));
                //Loglistbox.Items.Add(msg);
                sw.Stop();
                Console.WriteLine("Time taken SIGNA10: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Read Signature");
                return;
            }
        }
        private void Minutiae1()
        {
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = Config.MINUTIAE1;

                int rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken 1MINU1: {0}ms", sw.Elapsed.TotalMilliseconds);
                int k = 8;
                int l = 2;
                int len_total;

                msg = "SAM Encrypt Select EF Minutiae #1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[RxDataLen - 2];
                Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken 1MINU2: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "Ek(Select EF Minutiae #1)";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken 1MINU3: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Decrypt(Ek(Status #4))";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byte[] byteCOMRead = new byte[Config.ENCRYPT.Length + Config.SAMREAD.Length + 3];
                Buffer.BlockCopy(Config.ENCRYPT, 0, byteCOMRead, 0, Config.ENCRYPT.Length);
                Buffer.BlockCopy(Config.SAMREAD, 0, byteCOMRead, Config.ENCRYPT.Length, Config.SAMREAD.Length);
                byteCOMRead[byteCOMRead.Length - 3] = 0x00;
                byteCOMRead[byteCOMRead.Length - 2] = 0x00;
                byteCOMRead[byteCOMRead.Length - 1] = 0x08;

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOMRead.Length, byteCOMRead, ref RxDataLen, RxData);
                Console.WriteLine("Time taken 1MINU4: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Encrypt Read Minutiae #1 - 1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOMRead, byteCOMRead.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[RxDataLen - 2];
                Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken 1MINU5: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "Ek(Read Minutiae #1) - 1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken 1MINU6: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Decrypt(Ek(Minutiae #1)) - 1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                Array.Copy(RxData, 2, ektp.byteMinu1, 0, RxDataLen - 6);
                ektp.minu1len = ektp.minu1len + RxDataLen - 6;

                len_total = (RxData[0] * 256) + RxData[1] + 2;
                int loopData2 = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_LOOP2));
                while (k < len_total)
                {
                    UInt16 a = 0;
                    byteCOMRead[byteCOMRead.Length - 3] = (byte)(k / 256);
                    byteCOMRead[byteCOMRead.Length - 2] = (byte)(k % 256);

                    if ((len_total - k) / (UInt16)loopData2 <= 0)
                    {
                        a = (UInt16)(len_total - k);
                        if (a % 8 > 0)
                            a = (UInt16)(((a / 8) + 1) * 8);
                    }
                    else
                        a = (UInt16)loopData2;
                    byteCOMRead[byteCOMRead.Length - 1] = (byte)a;

                    Array.Clear(RxData, 0, RxData.Length);
                    rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOMRead.Length, byteCOMRead, ref RxDataLen, RxData);
                    Console.WriteLine("Time taken 1MINU7: {0}ms", sw.Elapsed.TotalMilliseconds);
                    k = k + a;

                    msg = "SAM Encrypt Read Minutiae #1 - " + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOMRead, byteCOMRead.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);

                    byteCOM = new byte[RxDataLen - 2];
                    Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                    Array.Clear(RxData, 0, RxData.Length);
                    rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                    Console.WriteLine("Time taken 1MINU8: {0}ms", sw.Elapsed.TotalMilliseconds);

                    msg = "Ek(Read Minutiae #1) - " + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);

                    byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                    Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                    byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                    Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                    Array.Clear(RxData, 0, RxData.Length);
                    rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                    Console.WriteLine("Time taken 1MINU9: {0}ms", sw.Elapsed.TotalMilliseconds);

                    msg = "SAM Decrypt(Ek(Minutiae #1) - " + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);

                    Array.Copy(RxData, 0, ektp.byteMinu1, ektp.minu1len, RxDataLen - 4);
                    ektp.minu1len = ektp.minu1len + RxDataLen - 4;

                    l += 1;
                }
                msg = ("Minutiae 1 = " + strIns(ByteArrayToString(ektp.byteMinu1, ektp.minu1len), " "));
                Console.WriteLine(msg);
                sw.Stop();
                Console.WriteLine("Time taken 1MINU10: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Read Minutiae 1");
                return;
            }
        }
        private void Minutiae2()
        {
            int SAMreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_SAM));
            int RFreader = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_RF));
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                UInt16 RxDataLen = 0;
                byte[] RxData = new byte[1024];
                byte[] byteCOM = Config.MINUTIAE2;

                int rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken 2MINU1: {0}ms", sw.Elapsed.TotalMilliseconds);
                int k = 8;
                int l = 2;
                int len_total;

                msg = "SAM Encrypt Select EF Minutiae #2";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[RxDataLen - 2];
                Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken 2MINU2: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "Ek(Select EF Minutiae #2)";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken 2MINU3: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Decrypt(Ek(Status #5))";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byte[] byteCOMRead = new byte[Config.ENCRYPT.Length + Config.SAMREAD.Length + 3];
                Buffer.BlockCopy(Config.ENCRYPT, 0, byteCOMRead, 0, Config.ENCRYPT.Length);
                Buffer.BlockCopy(Config.SAMREAD, 0, byteCOMRead, Config.ENCRYPT.Length, Config.SAMREAD.Length);
                byteCOMRead[byteCOMRead.Length - 3] = 0x00;
                byteCOMRead[byteCOMRead.Length - 2] = 0x00;
                byteCOMRead[byteCOMRead.Length - 1] = 0x08;

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOMRead.Length, byteCOMRead, ref RxDataLen, RxData);
                Console.WriteLine("Time taken 2MINU4: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Encrypt Read Minutiae #2 - 1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOMRead, byteCOMRead.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[RxDataLen - 2];
                Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken 2MINU5: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "Ek(Read Minutiae #2) - 1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken 2MINU6: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Decrypt(Ek(Minutiae #2)) - 1";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                Array.Copy(RxData, 2, ektp.byteMinu2, 0, RxDataLen - 6);
                ektp.minu2len = ektp.minu2len + RxDataLen - 6;

                len_total = (RxData[0] * 256) + RxData[1] + 2;
                int loopData2 = Convert.ToInt32(config.Read("EKTP", Config.PARAM_READER_LOOP2));
                while (k < len_total)
                {
                    UInt16 a = 0;
                    byteCOMRead[byteCOMRead.Length - 3] = (byte)(k / 256);
                    byteCOMRead[byteCOMRead.Length - 2] = (byte)(k % 256);

                    if ((len_total - k) / (UInt16)loopData2 <= 0)
                    {
                        a = (UInt16)(len_total - k);
                        if (a % 8 > 0)
                            a = (UInt16)(((a / 8) + 1) * 8);

                        byteCOM = new byte[Config.STOPAUTOVERIF.Length];
                        byteCOM = Config.STOPAUTOVERIF;

                        Array.Clear(RxData, 0, RxData.Length);
                        rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                        Console.WriteLine("Time taken 2MINU7: {0}ms", sw.Elapsed.TotalMilliseconds);

                        msg = "SAM Stop Digital Signature Automatic Verification";
                        Console.WriteLine(msg);
                        msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                        Console.WriteLine(msg);
                        msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                        Console.WriteLine(msg);
                    }
                    else
                        a = (UInt16)loopData2;
                    byteCOMRead[byteCOMRead.Length - 1] = (byte)a;

                    Array.Clear(RxData, 0, RxData.Length);
                    rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOMRead.Length, byteCOMRead, ref RxDataLen, RxData);
                    Console.WriteLine("Time taken 2MINU8: {0}ms", sw.Elapsed.TotalMilliseconds);
                    k = k + a;

                    msg = "SAM Encrypt Read Minutiae #2 - " + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOMRead, byteCOMRead.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);

                    byteCOM = new byte[RxDataLen - 2];
                    Buffer.BlockCopy(RxData, 0, byteCOM, 0, RxDataLen - 2);

                    Array.Clear(RxData, 0, RxData.Length);
                    rc = EKtpDLL.APDU_Transmit((UInt16)RFreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                    Console.WriteLine("Time taken 2MINU9: {0}ms", sw.Elapsed.TotalMilliseconds);

                    msg = "Ek(Read Minutiae #2) - " + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> KTP-el : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " KTP-el --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);

                    byteCOM = new byte[Config.DECRYPT.Length + 1 + RxDataLen - 2];
                    Buffer.BlockCopy(Config.DECRYPT, 0, byteCOM, 0, Config.DECRYPT.Length);
                    byteCOM[Config.DECRYPT.Length] = (byte)(RxDataLen - 2);
                    Buffer.BlockCopy(RxData, 0, byteCOM, Config.DECRYPT.Length + 1, RxDataLen - 2);

                    Array.Clear(RxData, 0, RxData.Length);
                    rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                    Console.WriteLine("Time taken 2MINU10: {0}ms", sw.Elapsed.TotalMilliseconds);

                    msg = "SAM Decrypt(Ek(Minutiae #2) - " + l;
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                    Console.WriteLine(msg);
                    msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                    Console.WriteLine(msg);

                    Array.Copy(RxData, 0, ektp.byteMinu2, ektp.minu2len, RxDataLen - 4);
                    ektp.minu2len = ektp.minu2len + RxDataLen - 4;

                    l += 1;
                }
                msg = ("Minutiae 2 = " + strIns(ByteArrayToString(ektp.byteMinu2, ektp.minu2len), " "));
                Console.WriteLine(msg);

                byteCOM = new byte[Config.VERIFYDF.Length];
                byteCOM = Config.VERIFYDF;

                Array.Clear(RxData, 0, RxData.Length);
                rc = EKtpDLL.APDU_Transmit((UInt16)SAMreader, (UInt16)byteCOM.Length, byteCOM, ref RxDataLen, RxData);
                Console.WriteLine("Time taken 2MINU11: {0}ms", sw.Elapsed.TotalMilliseconds);

                msg = "SAM Verify Digital Signature";
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " MC --> SAM : " + strIns(ByteArrayToString(byteCOM, byteCOM.Length), " "));
                Console.WriteLine(msg);
                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                msg = (DateTime.Now.ToString() + " SAM --> MC : " + strIns(ByteArrayToString(RxData, RxDataLen), " "));
                Console.WriteLine(msg);

                sw.Stop();
                Console.WriteLine("Time taken 2MINU12: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            catch
            {
                Console.WriteLine("Error = Cannot Read Minutiae 2");
                return;
            }
        }
    }
}
