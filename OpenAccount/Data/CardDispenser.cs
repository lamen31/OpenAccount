using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Globalization;

namespace OpenAccount.Data
{
    public class CardDispenser
    {
        [DllImport("CRT_591_H001.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 CRT591H001ROpen(string port);

        [DllImport("CRT_591_H001.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long CRT591H001ROpenWithBaut(string port, UInt32 baudrate);

        [DllImport("CRT_591_H001.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CRT591H001RClose(UInt32 CommHandle);

        [DllImport("CRT_591_H001.dll")]
        public static extern int USB_ExeCommand(UInt32 ComHandle, byte TxCmCode, byte TxPmCode, UInt16 TxDataLen, byte[] TxData, ref byte RxReplyType, ref byte RxStCode0, ref byte RxStCode1, ref byte RxStCode2, ref UInt16 RxDataLen, byte[] RxData);

        [DllImport("CRT_591_H001.dll")]
        public static extern int USB_ExeCommand(UInt32 ComHandle, UInt16 TxDataLen, byte[] TxData, ref UInt16 RxDataLen, byte[] RxData);

        [DllImport("CRT_591_H001.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RS232_ExeCommand(UInt32 ComHandle, byte TxCmCode, byte TxPmCode, UInt16 TxDataLen, byte[] TxData, ref byte RxReplyType, ref byte RxStCode0, ref byte RxStCode1, ref byte RxStCode2, ref UInt16 RxDataLen, byte[] RxData); // UInt32 ComHandle, UInt16 TxDataLen, byte[] TxData, ref UInt16 RxDataLen, byte[] RxData

        [DllImport("CRT_591_H001.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RS232_ExeCommand(UInt32 ComHandle, UInt16 TxDataLen, byte[] TxData, ref UInt16 RxDataLen, byte[] RxData);

        UInt32 Hndl = 0;
        public static UInt32 baudRate { get; set; }
        private string Device_Address = "00";
        private string com_serial = string.Empty;

        public void Dispenser(ref string p_errorCode, string p_com_Serial, string strbox, ref string p_message)
        {
            p_errorCode = string.Empty;
            p_message = string.Empty;
            com_serial = p_com_Serial;
            byte CBox = Convert.ToByte(strbox);

            try
            {
                uint x = 115200;
                Hndl = (UInt32)CRT591H001ROpenWithBaut(com_serial, x);
                if (Hndl != 0)
                {
                    p_message = "Comm. Port is Opened";
                    Utility.WriteLog("Card dispenser condition : com port is opened", "step-action");
                }
                else
                {
                    p_message = "Open Comm. Port Error [Port : " + com_serial + "]";
                    p_errorCode = "OCPE"; // Open Comm Port Error
                    Utility.WriteLog("Card dispenser condition : open com port error [port : " + com_serial + "]" , "step-action");
                }

                if (Hndl != 0)
                {
                    byte Addrs;
                    byte Cm, Pm;
                    byte RxRefType = 0;
                    UInt16 TxDataLen, RxDataLen;
                    byte[] TxData = new byte[1024];
                    byte[] Rxdata = new byte[1024];
                    byte Retype = 0;
                    byte St0, St1, St2;
                    Cm = 0x30; // 30
                    Pm = 0x33; // 33 : dont move card, 31 : move card backward
                    St0 = St1 = St2 = 0;
                    //TxDataLen = 0;
                    TxDataLen = 15;
                    TxData[0] = 0x43;//Default Parameter
                    TxData[1] = 0x30;//Default Parameter
                    TxData[2] = 0x33;//PM Parameter
                    TxData[3] = 0x33;//NoMeaning Parameter value from 0x30 - 0x34 to get Positive (OK) Response
                    TxData[4] = 0x32;//NoMeaning Parameter value from 0x30 - 0x34 to get Positive (OK) Response
                    TxData[5] = 0x34;//NoMeaning Parameter value from 0x30 - 0x34 to get Positive (OK) Response
                    TxData[6] = 0x31;//NoMeaning Parameter value from 0x30 - 0x34 to get Positive (OK) Response
                    TxData[7] = 0x30;//FM Parameter, always 0x30
                    TxData[8] = 0x30;//PD Parameter
                    TxData[9] = 0x30;//TY Parameter
                    TxData[10] = 0x30;//DS Parameter
                    TxData[11] = 0x30;//CC Parameter
                    TxData[12] = 0x30;//Default Parameter
                    TxData[13] = 0x30;//Default Parameter
                    TxData[14] = 0x30;//Default Parameter

                    RxDataLen = 0;
                    Addrs = (byte)(byte.Parse(Device_Address.Substring(0, 2), NumberStyles.Number));
                    //Addrs = (byte)(byte.Parse(Device_Address.Substring(0, 2), NumberStyles.Number));
                    int j = RS232_ExeCommand(Hndl, TxDataLen, TxData, ref RxDataLen, Rxdata);
                    //int i = DllFunction.RS232_ExeCommand(Hndl, TxDataLen, TxData, ref RxDataLen, Rxdata);
                    if (j == 0)
                    {
                        if (Rxdata[0] == 0x50)
                        {
                            p_message = "INITIALIZE OK";
                            Utility.WriteLog("Card dispenser condition : initialize ok", "step-action");
                        }
                        else
                        {
                            p_message = "INITIALIZE ERROR";
                            Utility.WriteLog("Card dispenser condition : initialize error", "step-action");
                        }
                    }
                    else
                    {
                        p_errorCode = "CE";
                        p_message = "Communication Error";
                        Utility.WriteLog("Card dispenser condition : communication error", "step-action");
                    }

                    TxData = new byte[1024];
                    Rxdata = new byte[1024];

                    TxDataLen = 3;
                    TxData[0] = 0x43;
                    TxData[1] = 0x31;
                    TxData[2] = 0x30;
                    RxDataLen = 0;

                    j = RS232_ExeCommand(Hndl, TxDataLen, TxData, ref RxDataLen, Rxdata);

                    if (j == 0)
                    {
                        if (Rxdata[0] == 0x50)
                        {
                            p_message = "CHECK STATUS 1 OK";
                            Utility.WriteLog("Card dispenser condition : check status 1 ok", "step-action");
                        }
                        else
                        {
                            p_message = "CHECK STATUS 1 ERROR";
                            Utility.WriteLog("Card dispenser condition : check status 1 error", "step-action");
                        }
                    }
                    else
                    {
                        p_errorCode = "CE";
                        p_message = "Communication Error";
                        Utility.WriteLog("Card dispenser condition : communication error", "step-action");
                    }

                    TxData = new byte[1024];
                    Rxdata = new byte[1024];

                    TxDataLen = 3;
                    TxData[0] = 0x43;
                    TxData[1] = 0x31;
                    TxData[2] = 0x31;
                    RxDataLen = 0;

                    j = RS232_ExeCommand(Hndl, TxDataLen, TxData, ref RxDataLen, Rxdata);

                    if (j == 0)
                    {
                        if (Rxdata[0] == 0x50)
                        {
                            p_message = "CHECK STATUS 2 OK";
                            Utility.WriteLog("Card dispenser condition : check status 2 ok", "step-action");
                        }
                        else
                        {
                            p_message = "CHECK STATUS 2 ERROR";
                            Utility.WriteLog("Card dispenser condition : check status 2 error", "step-action");
                        }
                    }
                    else
                    {
                        p_errorCode = "CE";
                        p_message = "Communication Error";
                        Utility.WriteLog("Card dispenser condition : communication error", "step-action");
                    }

                    TxData = new byte[1024];
                    Rxdata = new byte[1024];

                    if (strbox == "1")
                    {
                        CBox = 0x31;
                    }
                    else if (strbox == "2")
                    {
                        CBox = 0x32;
                    }

                    TxDataLen = 4;
                    TxData[0] = 0x43;
                    TxData[1] = 0x32;
                    TxData[2] = 0x32;
                    TxData[3] = CBox; //determine box 0x31=box#1; 0x32=box#2;
                    RxDataLen = 0;

                    j = RS232_ExeCommand(Hndl, TxDataLen, TxData, ref RxDataLen, Rxdata);

                    if (j == 0)
                    {
                        if (Rxdata[0] == 0x50)
                        {
                            p_message = "FEEDING OK";
                            Utility.WriteLog("Card dispenser condition : feeding ok", "step-action");
                        }
                        else
                        {
                            p_message = "FEEDING ERROR";
                            Utility.WriteLog("Card dispenser condition : feeding error", "step-action");
                        }
                    }
                    else
                    {
                        p_errorCode = "CE";
                        p_message = "Communication Error";
                        Utility.WriteLog("Card dispenser condition : communication error", "step-action");
                    }

                    TxData = new byte[1024];
                    Rxdata = new byte[1024];

                    TxDataLen = 3;
                    TxData[0] = 0x43;
                    TxData[1] = 0x33;
                    TxData[2] = 0x30;
                    RxDataLen = 0;

                    j = RS232_ExeCommand(Hndl, TxDataLen, TxData, ref RxDataLen, Rxdata);

                    if (j == 0)
                    {
                        if (Rxdata[0] == 0x50)
                        {
                            p_message = "EJECT OK";
                            Utility.WriteLog("Card dispenser condition : eject ok", "step-action");
                        }
                        else
                        {
                            p_message = "EJECT ERROR";
                            Utility.WriteLog("Card dispenser condition : eject error ok", "step-action");
                        }
                    }
                    else
                    {
                        p_errorCode = "CE";
                        p_message = "Communication Error";
                        Utility.WriteLog("Card dispenser condition : communication error", "step-action");
                    }

                }
                else
                {
                    p_errorCode = "CO";
                    p_message = "Comm. port is not Opened";
                    Utility.WriteLog("Card dispenser condition : com port is not opened", "step-action");
                }

                int i = CRT591H001RClose(Hndl);
                if (i == 0)
                    Utility.WriteLog("Card dispenser condition : close port ok", "step-action");
                else
                    Utility.WriteLog("Card dispenser condition : close port error", "step-action");

            }
            catch(Exception ex)
            {
                p_message = ex.Message;
                Utility.WriteLog("Card dispenser condition : " + p_message, "step-action");
            }
        }

        public int Initialize()
        {
            byte cm = 0x30;
            /* PM Byte Option :
                “0”(30h) If card is inside ICRW, move card
                to gate
                “1”(31h) If card is inside ICRW, capture card
                backward
                “2”(32h) If card is inside ICRW, retain the
                card inside of ICRW.
                “3”(33h) If card is inside ICRW, does not
                move the card
                “8”(38h) Output initialize parameter 
            */
            return 0;
        }
        public int StatusRequest()
        {
            byte cm = 0x31;
            /* PM Byte Option :
                “0”(30h) Report presence of card and it's position
                “1”(31h) Report presence of sensor status in detail
                “@”(40h) Report presence of sensor status in detail (Exp.) 
            */
            return 0;
        }
        public int Entry()
        {
            byte cm = 0x32;
            /* PM Byte Option :
                “0”(30h) Syn-wait for card inserting (mag-card detection is available)
                “2”(32h) Dispense the card from specified box to read/write card module 
            */
            return 0;
        }
        public int CardCaptureAndEject()
        {
            byte cm = 0x33;
            /* PM Byte Option :
                “0”(30h) Move card to Gate from inside ICRW
                “1”(31h) Capture card to rear side of ICRW 
            */
            return 0;
        }
        public int Retrieve()
        {
            byte cm = 0x34;
            /* PM Byte Option :
                “0”(30h) Retrieve card, which is in Gate position 
            */
            return 0;
        }
        public int LedControl()
        {
            byte cm = 0x35;
            /* PM Byte Option :
                “0”(30h) All color LED off
                “1”(31h) LED Green On
                “2”(32h) LED Red On
                “3”(33h) LED Orange On
                “<”(3Ch) External input terminal
                “[”(5Bh) External terminal 1 control
                “]”(5Dh) External terminal 2 control
                “{”(7Bh) External terminal 3 control
                “}”(7Dh) External terminal 4 control 
            */
            return 0;
        }
        public int MagTrackRead()
        {
            byte cm = 0x36;
            /* PM Byte Option :
                “1”(31h) ISO Track #1 reads Transmit read data
                “2”(32h) ISO Track #2 reads Transmit read data 
                “3”(33h) ISO Track #3 reads Transmit read data
                “5”(35h) Transmit All channel data
                “7”(37h) Status read data buffer
                “8”(38h) Re-swipe magnetic strip, transfer three track data
            */
            return 0;
        }
        public int AsyncCIEDC()//Asynchronously Card Insertion Enable/Disable Command
        {
            byte cm = 0x3A;
            /* PM Byte Option :
                “0”(30h) Enable card entry
                “1”(31h) Disable card entry
                “X”(58h) Jitter card controlling set
            */
            return 0;
        }
        public int ElectricTranferLevel()
        {
            byte cm = 0x3E;
            /* PM Byte Option :
                “0”(30h) Transmit the sensor A/D level
                “B”(42h) Read battery charge
                “V”(56h) Read power voltage
            */
            return 0;
        }
        public int ICCardMoveControl()
        {
            byte cm = 0x40;
            /* PM Byte Option :
                “0”(30h) Move card to IC contact position
                “2”(32h) Release IC contact
            */
            return 0;
        }
        public int Revision()
        {
            byte cm = 0x41;
            /* PM Byte Option :
                “1”(31h) Revision of User program code area
                “2”(32h) Revision of EMV2000 code area
                “3”(33h) Transmit the EMV approval number
                “4”(34h) Reserve
                “5”(35h) Transmit the IFM number of the EMV approval
                “A”(41h) Read dispenser module program version
            */
            return 0;
        }
        public int Counter()
        {
            byte cm = 0x43;
            /* PM Byte Option :
                “2”(32h) Inquire of card pass count
                “3”(33h) Inquire of card capture count
                “4”(34h) Set capture alert count and clear counter
                “5”(35h) Set capture alert count
                “6”(36h) Clear capture counter
            */
            return 0;
        }
        public int IcCardControl()
        {
            byte cm = 0x49;
            /* PM Byte Option :
                “0”(30h) Activate IC
                “1”(31h) Deactivate IC
                “2”(32h) Inquire of IC status
                “3”(33h) IC Communication T=0
                “4”(34h) IC Communication T=1
                “5”(35h) IC extended Communication 1
                “6”(36h) IC extended Communication 2
                “7”(37h) IC extended Communication 3
                “8”(38h) IC Warm reset
                “9”(39h) IC automatic communication
            */
            return 0;
        }
        public int SamControl()
        {
            byte cm = 0x49;
            /* PM Byte Option :
                “@”(40h) Activate SAM
                “A”(41h) Deactivate SAM
                “B”(42h) Inquire of SAM status
                “C”(43h) SAM Communication T=0
                “D”(44h) SAM Communication T=1
                “E”(45h) SAM extended Communication 1
                “F”(46h) SAM extended Communication 2
                “G”(47h) SAM extended Communication 3
                “H”(48h) SAM Warm reset
                “I”(49h) SAM Automatic Communication
                “P”(50h) Select SAM
                “m”(6Dh) Auto-get CPU card response
            */
            return 0;
        }
        public int Switch()
        {
            byte cm = 0x4B;
            /* PM Byte Option :
                “0”(30h) Switch to Supervisor program code area
            */
            return 0;
        }
        public int SiemensMemoryCardControl()
        {
            byte cm = 0x52;
            /* PM Byte Option :
                “0”(30h) Power Supply and Activate to Siemens card
                “1”(31h) Deactivate to Siements card
                “2”(32h) Inquire status of Siemens card
                “3”(33h) Exchange data for 4442 card
                “4”(34h) Exchange data for 4428 card
            */
            return 0;
        }
        public int I2CMemoryControl()
        {
            byte cm = 0x53;
            /* PM Byte Option :
                “0”(30h) To activate I2C and To close the shutter
                “1”(31h) To deactivate I2C
                “2”(32h) To inquire status of I2C
                “3”(33h) To exchange data between I2C
            */
            return 0;
        }
        public int RFCardCommand()
        {
            byte cm = 0x5A;
            /* PM Byte Option :
                “0”(30h) Activate contactless IC
                “1”(31h) Deactivate IC card
                “3”(33h) Mifare one read&write
                “4”(34h) Type A/B communication
                “5”(35h) Type A,B card extension 1
                “6”(36h) Type A,B card extension 2
                “7”(37h) Type A,B card extension communication
                “8”(38h) Contactless IC card interface reset
            */
            return 0;
        }
        public int ThirdPartyCOMDataSending()
        {
            byte cm = 0x5B;
            /* PM Byte Option :
                “\”(5Ch) Baudrate setting
                “]”(5Dh) Command transfer
            */
            return 0;
        }
        public int AutoTestCardType()
        {
            byte cm = 0x90;
            /* PM Byte Option :
                “0”(30h) Auto-test type of IC card in ICRW
                “2”(32h) Auto-test type of contactless in ICRW
            */
            return 0;
        }
        public int CounterOperatedMovingParts()
        {
            byte cm = 0xA1;
            /* PM Byte Option :
                “0”(30h) Read count of used part
                “1”(31h) Count of initialization
            */
            return 0;
        }
        public int GetSerialNumber()
        {
            byte cm = 0xA2;
            /* PM Byte Option :
                “0”(30h) Read S/N of ICRW
            */
            return 0;
        }
        public int GetFirmwareVersion()
        {
            byte cm = 0xA4;
            /* PM Byte Option :
                “0”(30h) Get firmware version
            */
            return 0;
        }
        public string ParseResponseInfo(byte[] data)
        {
            
            if (data[0] == 0x50)//
            {
                //This is a transmission format that ICRW informs HOST of the proper completion of command execution.
                //The first character should be "P"(= 50H).There are positive responses with data part and without data part.
                //In this format cm and pm returns the same values which were received with command transmission.
                //(pm: except for IC card control)
                return GetStatusInfo(data);

            }
            else if (data[0] == 0x4E)
            {
                //This is a transmission format that ICRW informs HOST of the abnormal completion of command execution.
                //The first character should be "N"(= 4EH).There are negative response with data part and without data part.
                //In this format cm and pm returns the same values which were received with command transmission.
                //(pm: except for IC card control)
                return GetErrorInfo(data);
            }
            return "Card Dispenser Return Invalid Data";
        }
        public string GetStatusInfo(byte[] data)
        {
            byte cm = data[1];
            byte pm = data[2];
            byte st0 = data[3];
            byte st1 = data[4];
            if (st0 == 0x30 && st1 == 0x30)
            {
                return "No card detected within ICRW";
            }
            else if(st0 == 0x30 && st1 == 0x31)
            {
                return "Card locates at card Gate";
            }
            else if (st0 == 0x30 && st1 == 0x32)
            {
                return "Card locates inside ICRW";
            }
            else if (st0 == 0x31)
            {
                return "There are still 1 packets left to receive (x ignores its status)";
            }
            else if (st0 == 0x32)
            {
                return "There are still 2 packets left to receive (x ignores its status)";
            }
            else if (st0 == 0x33)
            {
                return "There are still 3 packets left to receive (x ignores its status)";
            }
            else if (st0 == 0x34)
            {
                return "There are still 4 packets left to receive (x ignores its status)";
            }
            else if (st0 == 0x35)
            {
                return "There are still 5 packets left to receive (x ignores its status)";
            }
            else if (st0 == 0x30)
            {
                return "There are still 0 packets left to receive (x ignores its status)";
            }
            return "Card Dispenser Return Invalid Status Data";
        }
        public string GetErrorInfo(byte[] data)
        {
            byte cm = data[1];
            byte pm = data[2];
            byte e0 = data[3];
            byte e1 = data[4];
            if (e0 == 0x30 && e1 == 0x30)
            {
                return "A given command code is unidentified";
            }
            else if (e0 == 0x30 && e1 == 0x31)
            {
                return "Parameter is not correct";
            }
            else if (e0 == 0x30 && e1 == 0x32)
            {
                return "Command execution is impossible";
            }
            else if (e0 == 0x30 && e1 == 0x33)
            {
                return "Function is not implemented";
            }
            else if (e0 == 0x30 && e1 == 0x34)
            {
                return "Command data error";
            }
            else if (e0 == 0x30 && e1 == 0x35)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x30 && e1 == 0x36)
            {
                return "Key for decrypting is not received";
            }
            else if (e0 == 0x30 && e1 == 0x37)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x30 && e1 == 0x38)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x30 && e1 == 0x39)
            {
                return "Intake withdraw timeout";
            }
            else if (e0 == 0x31 && e1 == 0x30)
            {
                return "Card Jam";
            }
            else if (e0 == 0x31 && e1 == 0x31)
            {
                return "Shutter error";
            }
            else if (e0 == 0x31 && e1 == 0x32)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x31 && e1 == 0x33)
            {
                return "Irregular card length(long)";
            }
            else if (e0 == 0x31 && e1 == 0x34)
            {
                return "Irregular card length(short)";
            }
            else if (e0 == 0x31 && e1 == 0x35)
            {
                return "FLASH Memory Parameter Area CRC error";
            }
            else if (e0 == 0x31 && e1 == 0x36)
            {
                return "Card position Move(and pull out error)";
            }
            else if (e0 == 0x31 && e1 == 0x37)
            {
                return "Jam error at retrieve";
            }
            else if (e0 == 0x31 && e1 == 0x38)
            {
                return "Two card error";
            }
            else if (e0 == 0x31 && e1 == 0x39)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x32 && e1 == 0x30)
            {
                return "Read mag-card error (verifying faulty (VRC error))";
            }
            else if (e0 == 0x32 && e1 == 0x31)
            {
                return "Read mag-card error (start character error，end character error or LRC error)";
            }
            else if (e0 == 0x32 && e1 == 0x32)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x32 && e1 == 0x33)
            {
                return "Read mag-card error (no data，start character , end character and LRC only)";
            }
            else if (e0 == 0x32 && e1 == 0x34)
            {
                return "Read mag-card error (no mag-stripe or code)";
            }
            else if (e0 == 0x32 && e1 == 0x35)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x32 && e1 == 0x36)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x32 && e1 == 0x37)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x32 && e1 == 0x38)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x32 && e1 == 0x39)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x33 && e1 == 0x30)
            {
                return "Power down";
            }
            else if (e0 == 0x33 && e1 == 0x31)
            {
                return "DSR signal is OFF";
            }
            else if (e0 == 0x33 && e1 == 0x32)
            {
                return "Voltage is too high (high than 20%)";
            }
            else if (e0 == 0x33 && e1 == 0x33)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x33 && e1 == 0x34)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x33 && e1 == 0x35)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x33 && e1 == 0x36)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x33 && e1 == 0x37)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x33 && e1 == 0x38)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x33 && e1 == 0x39)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x34 && e1 == 0x30)
            {
                return "Pull out Error";
            }
            else if (e0 == 0x34 && e1 == 0x31)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x34 && e1 == 0x32)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x34 && e1 == 0x33)
            {
                return "IC Positioning Error";
            }
            else if (e0 == 0x34 && e1 == 0x34)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x34 && e1 == 0x35)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x35 && e1 == 0x30)
            {
                return "Capture Counter Overflow Error";
            }
            else if (e0 == 0x35 && e1 == 0x31)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x35 && e1 == 0x32)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x36 && e1 == 0x30)
            {
                return "Abnormal Vcc condition error of IC card or SAM";
            }
            else if (e0 == 0x36 && e1 == 0x31)
            {
                return "ATR communication error of IC card or SAM";
            }
            else if (e0 == 0x36 && e1 == 0x32)
            {
                return "Invalid ATR error to the selected activation for IC card or SAM";
            }
            else if (e0 == 0x36 && e1 == 0x33)
            {
                return "No response error on communication from IC card or SAM";
            }
            else if (e0 == 0x36 && e1 == 0x34)
            {
                return "Communication error to IC card or SAM(except for no response)";
            }
            else if (e0 == 0x36 && e1 == 0x35)
            {
                return "Not activated error of IC card or SAM";
            }
            else if (e0 == 0x36 && e1 == 0x36)
            {
                return "Not supported IC card or SAM error by ICRM(only for EMV activation)";
            }
            else if (e0 == 0x36 && e1 == 0x37)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x36 && e1 == 0x38)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x36 && e1 == 0x39)
            {
                return "Not supported IC card or SAM error by EMV2000(only for EMV activation)";
            }
            else if (e0 == 0x37 && e1 == 0x33)
            {
                return "EEPROM error";
            }
            else if (e0 == 0x41 && e1 == 0x30)
            {
                return "No card in cassette box";
            }
            else if (e0 == 0x41 && e1 == 0x31)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x41 && e1 == 0x32)
            {
                return "ICRW module communication time out";
            }
            else if (e0 == 0x41 && e1 == 0x33)
            {
                return "Exceed of re-check limited in ICRW module communication";
            }
            else if (e0 == 0x41 && e1 == 0x34)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x41 && e1 == 0x35)
            {
                return "Dispensing blocked";
            }
            else if (e0 == 0x41 && e1 == 0x36)
            {
                return "Ferry moving blocked";
            }
            else if (e0 == 0x41 && e1 == 0x37)
            {
                return "Hook mechanism move failure";
            }
            else if (e0 == 0x41 && e1 == 0x38)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x41 && e1 == 0x39)
            {
                return "Hook position is error (take out the box and re-power again or send initial command)";
            }
            else if (e0 == 0x41 && e1 == 0x41)
            {
                return "Unknown Error";
            }
            else if (e0 == 0x41 && e1 == 0x42)
            {
                return "Full of error card bin";
            }
            else if (e0 == 0x41 && e1 == 0x43)
            {
                return "Sensors is abnormal in dispensing module";
            }
            else if (e0 == 0x41 && e1 == 0x44)
            {
                return "Box has been removed, not placed on dispenser";
            }
            else if (e0 == 0x42 && e1 == 0x30)
            {
                return "Not received Initialize command";
            }
            else
            {
                return "Invalid Error";
            }
            return "Error Error Error";
        }

    }
}
