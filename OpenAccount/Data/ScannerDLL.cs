using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace OpenAccount.Data
{
    public class ScannerDLL
    {
        const string dllFile = "Crt7005.dll";
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short openDeviceList(ref int deviceNum);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short openDevice(int devIndex);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short getDeviceStatus(int devId);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short getFirewareVerInfo(int devId, byte[] bufVer);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short closeCardRead(int devId);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short cisQuery(int devId, byte[] buf);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short icsOut(int devId, byte[] buf);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short icsSwallow(int devId, byte[] buf);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short frontSwallow(int devId, int enable, byte[] buf);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short backSwallow(int devId, int enable, byte[] buf);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short goFrontPos(int devId, byte[] buf);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short goRadioPos(int devId, byte[] buf);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short goBackPos(int devId, byte[] buf);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short scan(int devId, short width, short height, short interval);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short saveBmp(string imgFile, byte[] data, ref CrtRect rect);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short readImageData(int devId, byte[] frontPtr, byte[] backPtr, ref CrtRect rect);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short getIdInfo(int devId, int type, IntPtr cardinfo);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short readImageDataAndOcr(int devId, byte[] frontPtr, byte[] backPtr, ref CrtRect rect, ref IDcardImgOcrInfo ocrInfo);
        [DllImport(dllFile, CallingConvention = CallingConvention.Cdecl)]
        public static extern short sendRfApdu(int devId, byte[] TxData, int TxDataLen, byte[] RxData, ref int RxDataLen);
    }
}
