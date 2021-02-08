using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void GetCard(ref string p_errorCode, string p_com_Serial, ref string p_message)
        {
            byte Addrs;
            byte Cm, Pm;
            byte RxRefType = 0;
            UInt16 TxDataLen, RxDataLen;
            byte[] TxData;
            byte[] Rxdata;
            byte Retype = 0;
            byte St0, St1, St2;
            p_errorCode = string.Empty;
            p_message = string.Empty;
            com_serial = p_com_Serial;

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
                    Utility.WriteLog("Card dispenser condition : open com port error [port : " + com_serial + "]", "step-action");
                }

                if (Hndl != 0)
                {
                    TxData = new byte[1024];
                    Rxdata = new byte[1024];
                    Cm = 0x30; // 30
                    Pm = 0x33; // 33 : dont move card, 31 : move card backward
                    St0 = St1 = St2 = 0;
                    //TxDataLen = 0;
                    TxDataLen = 15;
                    TxData[0] = 0x43;
                    TxData[1] = 0x30;
                    TxData[2] = 0x33;
                    TxData[3] = 0x33;
                    TxData[4] = 0x32;
                    TxData[5] = 0x34;
                    TxData[6] = 0x31;
                    TxData[7] = 0x30;
                    TxData[8] = 0x30;
                    TxData[9] = 0x30;
                    TxData[10] = 0x30;
                    TxData[11] = 0x30;
                    TxData[12] = 0x30;
                    TxData[13] = 0x30;
                    TxData[14] = 0x30;

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

                    TxDataLen = 4;
                    TxData[0] = 0x43;
                    TxData[1] = 0x32;
                    TxData[2] = 0x30;
                    TxData[3] = 0x30;
                    RxDataLen = 0;

                    j = RS232_ExeCommand(Hndl, TxDataLen, TxData, ref RxDataLen, Rxdata);
                    if (j == 0)
                    {
                        if (Rxdata[0] == 0x50)
                        {
                            p_message = "ENTRY OK" + "\r\n" + "Status Code : " + (int)St0 + (int)St1 + (int)St2;
                            Console.WriteLine(p_message);

                        }
                        else
                        {
                            p_errorCode = "" + (char)St1 + (char)St2;
                            p_message = "ENTRY ERROR" + "\r\n" + "Error Code:  " + (int)St1 + (int)St2;
                            Console.WriteLine(p_message);

                        }
                    }
                    else
                    {
                        p_errorCode = "CE";
                        p_message = "Communication Error";
                        Console.WriteLine(p_message);

                    }

                    TxData = new byte[1024];
                    Rxdata = new byte[1024];

                    TxDataLen = 3;
                    TxData[0] = 0x43;
                    TxData[1] = 0x36;
                    TxData[2] = 0x32;
                    RxDataLen = 0;

                    j = RS232_ExeCommand(Hndl, TxDataLen, TxData, ref RxDataLen, Rxdata);
                    if (j == 0)
                    {
                        if (Rxdata[0] == 0x50)
                        {
                            p_message = "MAGNETIC TRACK READ OK" + "\r\n" + "Status Code : " + (int)St0 + (int)St1 + (int)St2;
                            Console.WriteLine(p_message);

                            string tempoutput = Encoding.ASCII.GetString(Rxdata);
                            Console.WriteLine("MAGNETIC OUTPUT READER " + tempoutput);
                            string cardNumber = tempoutput.Substring(5, 16);
                            Console.WriteLine("CARD NUMBER " + cardNumber);
                            string cardEXP = tempoutput.Substring(22, 4);
                            Console.WriteLine("CARD EXP " + cardEXP);
                            
                        }
                        else
                        {
                            p_errorCode = "" + (char)St1 + (char)St2;
                            p_message = "MAGNETIC TRACK READ ERROR" + "\r\n" + "Error Code:  " + (int)St1 + (int)St2;
                            Console.WriteLine(p_message);

                        }
                    }
                    else
                    {
                        p_errorCode = "CE";
                        p_message = "Communication Error";
                        Console.WriteLine(p_message);

                    }

                    TxData = new byte[1024];
                    Rxdata = new byte[1024];

                    TxDataLen = 4;
                    TxData[0] = 0x43;
                    TxData[1] = 0x33;
                    TxData[2] = 0x31;
                    TxData[3] = 0X31;
                    RxDataLen = 0;

                    j = RS232_ExeCommand(Hndl, TxDataLen, TxData, ref RxDataLen, Rxdata);
                    if (j == 0)
                    {
                        if (Rxdata[0] == 0x50)
                        {
                            p_message = "CAPTURE OK" + "\r\n" + "Status Code : " + (int)St0 + (int)St1 + (int)St2;
                            Console.WriteLine(p_message);

                        }
                        else
                        {
                            p_errorCode = "" + (char)St1 + (char)St2;
                            p_message = "CAPTURE ERROR" + "\r\n" + "Error Code:  " + (int)St1 + (int)St2;
                            Console.WriteLine(p_message);

                        }
                    }
                    else
                    {
                        p_errorCode = "CE";
                        p_message = "Communication Error";
                        Console.WriteLine(p_message);

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
            catch (Exception ex)
            {
                p_message = ex.Message;
                Utility.WriteLog("Card dispenser condition : " + p_message, "step-action");
            }
        
    }

        public void Dispenser(ref string p_errorCode, string p_com_Serial, string strbox, ref string p_message)
        {
            byte Addrs;
            byte Cm, Pm;
            byte RxRefType = 0;
            UInt16 TxDataLen, RxDataLen;
            byte[] TxData;
            byte[] Rxdata;
            byte Retype = 0;
            byte St0, St1, St2;
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
                    TxData = new byte[1024];
                    Rxdata = new byte[1024];
                    Cm = 0x30; // 30
                    Pm = 0x33; // 33 : dont move card, 31 : move card backward
                    St0 = St1 = St2 = 0;
                    //TxDataLen = 0;
                    TxDataLen = 15;
                    TxData[0] = 0x43;
                    TxData[1] = 0x30;
                    TxData[2] = 0x33;
                    TxData[3] = 0x33;
                    TxData[4] = 0x32;
                    TxData[5] = 0x34;
                    TxData[6] = 0x31;
                    TxData[7] = 0x30;
                    TxData[8] = 0x30;
                    TxData[9] = 0x30;
                    TxData[10] = 0x30;
                    TxData[11] = 0x30;
                    TxData[12] = 0x30;
                    TxData[13] = 0x30;
                    TxData[14] = 0x30;

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
    }
}
