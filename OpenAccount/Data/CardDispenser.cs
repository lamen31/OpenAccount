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
                    Console.WriteLine("Comm. Port is Opened");
                }
                else
                {
                    p_message = "Open Comm. Port Error [Port : " + com_serial + "]";
                    p_errorCode = "OCPE"; // Open Comm Port Error
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
                            Console.WriteLine(p_message);
                        }
                        else
                        {
                            p_message = "INITIALIZE ERROR";
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
                    TxData[1] = 0x31;
                    TxData[2] = 0x30;
                    RxDataLen = 0;

                    j = RS232_ExeCommand(Hndl, TxDataLen, TxData, ref RxDataLen, Rxdata);

                    if (j == 0)
                    {
                        if (Rxdata[0] == 0x50)
                        {
                            p_message = "CHECK STATUS 1 OK";
                            Console.WriteLine(p_message);
                        }
                        else
                        {
                            p_message = "CHECK STATUS 1 ERROR";
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
                    TxData[1] = 0x31;
                    TxData[2] = 0x31;
                    RxDataLen = 0;

                    j = RS232_ExeCommand(Hndl, TxDataLen, TxData, ref RxDataLen, Rxdata);

                    if (j == 0)
                    {
                        if (Rxdata[0] == 0x50)
                        {
                            p_message = "CHECK STATUS 2 OK";
                            Console.WriteLine(p_message);
                        }
                        else
                        {
                            p_message = "CHECK STATUS 2 ERROR";
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
                            Console.WriteLine(p_message);
                        }
                        else
                        {
                            p_message = "FEEDING ERROR";
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
                    TxData[1] = 0x33;
                    TxData[2] = 0x30;
                    RxDataLen = 0;

                    j = RS232_ExeCommand(Hndl, TxDataLen, TxData, ref RxDataLen, Rxdata);

                    if (j == 0)
                    {
                        if (Rxdata[0] == 0x50)
                        {
                            p_message = "EJECT OK";
                            Console.WriteLine(p_message);
                        }
                        else
                        {
                            p_message = "EJECT ERROR";
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
                    Console.WriteLine(p_message);
                }

                int i = CRT591H001RClose(Hndl);
                if (i == 0)
                    Console.WriteLine("CLOSE PORT OK");
                else
                    Console.WriteLine("CLOSE PORT ERROR");

            }
            catch(Exception ex)
            {
                p_message = ex.Message;
                Console.WriteLine(p_message);
            }
        }
    }
}
