using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace FormSignPad
{
    public partial class Form1 : Form
    {
        static int complete_msg = 0x7ffe;
        static int cancel_msg = 0x7ffd;
        //private int pointerSaveImage = 0;
        //private DateTime dateTimeImage = DateTime.Now;
        private string strSignFile;
        //private string strSignFile1;

        //return value of interface function
        static int HW_eOk = 0;      //success
        static int HW_eDeviceNotFound = -1;     //no device
        static int HW_eFailedLoadModule = -2;   //failed to load module
        static int HW_eFailedInitModule = -3;   //failed to inti module 
        static int HW_eWrongImageFormat = -4;   //do not support this image format
        static int HW_eNoSignData = -5;         //no sign data
        static int HW_eInvalidInput = -6;       //invalid input parameter



        public Form1()
        {
            InitializeComponent();
            button1.PerformClick();
            strSignFile = Directory.GetCurrentDirectory().ToString() + "\\hwsign.png";
            axHWPenSign1.HWSetBkColor(0xE0F8E0);
            axHWPenSign1.HWSetCtlFrame(2, 0x000000);
            axHWPenSign1.HWSetFilePath(strSignFile);
            File.Delete(strSignFile);
            Int64[] hwnd = new Int64[1];
            hwnd[0] = this.Handle.ToInt32();
            IntPtr ptrWnd = Marshal.UnsafeAddrOfPinnedArrayElement(hwnd, 0);
            axHWPenSign1.HWSetExtWndHandleCSharp(ptrWnd);
            OpenDevice();
        }



        public void OpenDevice()
        {
            int res = axHWPenSign1.HWInitialize();
            if (res == HW_eOk)
            {
                Console.WriteLine("SIGN PAD FORM: OPEN DEVICE SUCCESS");
                Utility.WriteLog("Sign pad condition : device open success", "step-action");
            }
            else if (res == HW_eDeviceNotFound)
            {
                Console.WriteLine("SIGN PAD FORM: DEVICE NOT FOUND");
                Utility.WriteLog("Condition : device not found", "step-action");
            }
            else if (res == HW_eFailedLoadModule)
            {
                Console.WriteLine("SIGN PAD FORM: DEVICE FAILED LOAD MODULE");
                Utility.WriteLog("Sign pad condition : device failed load module", "step-action");
            }
            else if (res == HW_eFailedInitModule)
            {
                Console.WriteLine("SIGN PAD FORM: DEVICE FAILED INIT MODULE");
                Utility.WriteLog("Sign pad condition : device failed init module", "step-action");
            }
            else if (res == HW_eWrongImageFormat)
            {
                Console.WriteLine("SIGN PAD FORM: DEVICE WRONG IMAGE FORMAT");
                Utility.WriteLog("Sign pad condition : device wrong image format", "step-action");
            }
            else if (res == HW_eNoSignData)
            {
                Console.WriteLine("SIGN PAD FORM: NO SIGN DATA");
                Utility.WriteLog("Sign pad condition : no sign data", "step-action");
            }
            else if (res == HW_eInvalidInput)
            {
                Console.WriteLine("SIGN PAD FORM: DEVICE INVALID INPUT");
                Utility.WriteLog("Sign pad condition : device invalid output", "step-action");
            }
        }

        private void SaveImage()
        {
            int res;
            res = axHWPenSign1.HWSaveFile();
            if (res == 0)
            {
                Console.WriteLine("SIGN PAD FORM: SAVE IMAGE SUCCESS");
                Utility.WriteLog("Sign pad condition : save image success", "step-action");
            }
            else
            {
                Console.WriteLine("SIGN PAD FORM: SAVE IMAGE FAILED");
                Utility.WriteLog("Sign pad condition : save image failed", "step-action");
            }
            CloseDevice();
        }

        private void ClearImage()
        {
            int res;
            res = axHWPenSign1.HWClearPenSign();
            if (res == 0)
            {
                Console.WriteLine("SIGN PAD FORM: CLEAR IMAGE SUCCESS");
                Utility.WriteLog("Sign pad condition : clear image success", "step-action");
            }
            else
            {
                Console.WriteLine("SIGN PAD FORM: CLEAR IMAGE FAILED");
                Utility.WriteLog("Sign pad condition : clear image failed", "step-action");
            }
        }

        private void CloseDevice()
        {
            axHWPenSign1.HWFinalize();
            Console.WriteLine("SIGN PAD FORM: DEVICE HAS BEEN CLOSED");
            Utility.WriteLog("Sign pad condition : device has been closed", "step-action");
            this.Close();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == complete_msg)
                SaveImage();
            else if (m.Msg == cancel_msg)
                ClearImage();

            base.WndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Console.WriteLine("SIGN PAD FORM: DEVICE HIDE SUCCESS");
        }
    }
}
