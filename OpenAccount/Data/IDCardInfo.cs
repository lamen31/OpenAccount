using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenAccount.Data
{
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct IDcardImgOcrInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public Byte[] idNum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public Byte[] dateLimit;
    }
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct S_IdCardInfo
    {

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public Byte[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Byte[] sex;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public Byte[] nation;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Byte[] bornDay;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 140)]
        public Byte[] address;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        public Byte[] iDNum;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public Byte[] issued;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Byte[] beginValidity;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Byte[] endValidity;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 38808)]
        public Byte[] img;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public Byte[] finger;
    }
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct S_GAT_Residence_permit
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public Byte[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Byte[] sex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Byte[] bornDay;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 140)]
        public Byte[] address;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        public Byte[] iDNum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public Byte[] issued;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Byte[] beginValidity;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Byte[] endValidity;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public Byte[] passNum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Byte[] issuedNum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Byte[] type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 38808)]
        public Byte[] img;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public Byte[] finger1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public Byte[] finger2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Byte[] circuitNum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Byte[] cardNum;
    }
    public enum CARD_TYPE
    {
        ID_CARD_TYPE,

        GAT_LIVE_CARD_TYPE,
        NONE_TYPE
    }
    public enum PICTURE_TYPE
    {
        PICTURE_FRONT,
        PICTURE_NEGATIVE,
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct unioninfo
    {
        [FieldOffset(0)]
        public S_IdCardInfo idCardInfo;
        [FieldOffset(0)]
        public S_GAT_Residence_permit gAT_Residence_permit;
    }
    public struct S_CardInfo
    {
        public CARD_TYPE type;
        public int isIncludeFingerInfo;
        public unioninfo un;
    }
    public struct CrtRect
    {
        public int width;
        public int height;
    }
    public class IDCardInfo
    {
        private string myIdCardCode;
        public string IdCardCode
        {
            get { return myIdCardCode; }
            set { myIdCardCode = value; }
        }

        private string myIdCardName;
        public string IdCardName
        {
            get { return myIdCardName; }
            set { myIdCardName = value; }
        }

        private string myBirthday;
        public string Birthday
        {
            get { return myBirthday; }
            set { myBirthday = value; }
        }

        private string myAddress;
        public string Address
        {
            get { return myAddress; }
            set { myAddress = value; }
        }

        private string myNation;
        public string Nation
        {
            get { return myNation; }
            set { myNation = value; }
        }

        private string myNation_Code;
        public string Nation_Code
        {
            get { return myNation_Code; }
            set { myNation_Code = value; }
        }
        private string mySex;
        public string Sex
        {
            get { return mySex; }
            set { mySex = value; }
        }

        private string myDepartment;
        public string Department
        {
            get { return myDepartment; }
            set { myDepartment = value; }
        }
        private string myIssueDate;
        public string IssueDate
        {
            get { return myIssueDate; }
            set { myIssueDate = value; }
        }

        private string myValidate;
        public string Validate
        {
            get { return myValidate; }
            set { myValidate = value; }
        }

        [JsonIgnore]
        private byte[] myPhoto;
        [JsonIgnore]
        public byte[] Photo
        {
            get { return myPhoto; }
            set { myPhoto = value; }
        }

        [JsonIgnore]
        private byte[] finger;
        [JsonIgnore]
        public byte[] Finger
        {
            get { return finger; }
            set { finger = value; }
        }
        public string photoBase64;
        public string finger64;
        public string frontPictureBase64;
        public string backPictureBase64;
        public string ocrIDnumber;
        public string ocrValiddate;
    }
    public class EEInfo
    {
        public string mrz;
        public string no;
        public string name;
        public string borth;
        public string validDate;
        public string photoBase64;
        public string frontPictureBase64;
        public string backPictureBase64;
    }
}
