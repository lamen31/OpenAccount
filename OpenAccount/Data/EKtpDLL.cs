using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace OpenAccount.Data
{
    public class EKtpDLL
    {
        [DllImport("CRT_603_CZ1.dll")]
        public static extern int InitializeContext(ref UInt16 ReaderCount);

        [DllImport("CRT_603_CZ1.dll")]
        public static extern int GetSCardReaderName(UInt16 ReaderSort, ref UInt16 RxDataLen, byte[] RxData);

        [DllImport("CRT_603_CZ1.dll")]
        public static extern int GetStatusChange(UInt16 ReaderSort);

        [DllImport("CRT_603_CZ1.dll")]
        public static extern int ConnectSCardReader(UInt16 ReaderSort);

        [DllImport("CRT_603_CZ1.dll")]
        public static extern int GetSCardReaderStatus(UInt16 ReaderSort, byte[] ReaderName, ref UInt16 ReaderNameLen, ref byte CardState, ref byte CardProtocol, byte[] ATR_Data, ref UInt16 ATR_DataLen);

        [DllImport("CRT_603_CZ1.dll")]
        public static extern int APDU_Transmit(UInt16 ReaderSort, UInt16 TxDataLen, byte[] TxData, ref UInt16 RxDataLen, byte[] RxData);

        [DllImport("CRT_603_CZ1.dll")]
        public static extern int GetSCardNuber(UInt16 ReaderSort, ref UInt16 RxDataLen, byte[] RxData);

        [DllImport("CRT_603_CZ1.dll")]
        public static extern int Extended_Transmit(UInt16 ReaderSort, UInt16 TxDataLen, byte[] TxData, ref UInt16 RxDataLen, byte[] RxData);

        [DllImport("CRT_603_CZ1.dll")]
        public static extern int DisconnectSCardReader(UInt16 ReaderSort);

        [DllImport("CRT_603_CZ1.dll")]
        public static extern int ReleaseContext();
    }
}
