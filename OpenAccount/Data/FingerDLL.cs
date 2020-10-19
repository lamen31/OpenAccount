using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace OpenAccount.Data
{
    public abstract class FingerDLL
    {
        /************************************************************************************************************
          Function  : This function opens the device of Fingerprint Recognition Module
          Arguments : 
          Return    : 1   - successed
                  others  - failed
          **************************************************************************************************************/
        [DllImport("FpStdP41M1.dll")]
        public static extern int FpStdP41M1_OpenDevice();


        /************************************************************************************************************
        Function  : This function closes the device of Fingerprint Recognition Module
        Arguments : [in] Device serial number (starting from 0)
        Return    : none
        *************************************************************************************************************/
        [DllImport("FpStdP41M1.dll")]
        public static extern void FpStdP41M1_CloseDevice(int device);

        /*************************************************************************************************************
        Function  : This function captures fingerprint image from device and outputs it.
        Arguments : device - [in] Device serial number (starting from 0)
                    image  - [out]  fingerprint image buffer
        Return    : 1      - successed
                    others - failed
        ***************************************************************************************************************/
        [DllImport("FpStdP41M1.dll")]
        public static extern int FpStdP41M1_GetImage(int device, Byte[] image);

        /**************************************************************************
            Function:  convert the original image data to BMP format data
            Parameter: pBmp - Output, BMP format data (size: image data size + 1078)
                       pRaw - Output ，original image data
                       X    - Input，Image Width
                       Y    - Input，Image Height 
            Return:    1    - Successed
                      others- Failed
        ***************************************************************************/
        [DllImport("FpStdP41M1.dll")]
        public static extern int zzRaw2Bmp(Byte[] pBmp, Byte[] pRaw, int X, int Y);

        /**************************************************************************
            Function:  Save original image data to bmp
            Parameter: strFileName - Input, Save Image path
                       pImage        - Output ，original image data
                       Width       - Input，Image Width
                       Height      - Input，Image Height 
            Return:    1           - Successed
                      others       - Failed
        ***************************************************************************/
        [DllImport("FpStdP41M1.dll")]
        public static extern int SaveBMP(string strFileName, Byte[] pImage, int Width, int Height);


        /**************************************************************************
            Function:  Detect if the sensor has fingers
            Parameter: device - [in] Device serial number (starting from 0)
                       pImage  - Intput ,original image data    
            Return:    Image Area score,>45 -Has finger,else - No finger
        ***************************************************************************/
        [DllImport("FpStdP41M1.dll")]
        public static extern int FpStdP41M1_IsFinger(int device, Byte[] pImage);
    }

    public abstract class Miaxis
    {
        /*********************************************************************************
       Function: Calculate the quality of fingerprint image
       Paramete: pFingerImgBuf - Image data     
                 pQScore       - Quality score
       Return      1           -  successed，
                  others       - failed
       *************************************************************************/
        [DllImport("fingerprint256x360.dll")]
        public static extern int zzFP_GetQualityScore256x360(Byte[] pFingerImgBuf, Byte[] pQScore);


        /************************************************************************
          Function : Get ANSI Feature
          Paramete:  input - Image data
		             tz    - Fingerprint feature pointer, ANSI format feature,length: 1024 bytes
          Return:	 1     - Successed；
                    Others - Failed
        ************************************************************************/
        [DllImport("fingerprint256x360.dll")]
        public static extern int zzGetTz256x360_ANSI(Byte[] input, Byte[] tz);

        /************************************************************************
        Function : Get ISO Feature
        Paramete:  input  - Image data
		            tz    - Fingerprint feature pointer, ISO format feature, length: 1024 bytes
        Return:	    1     - Successed；
                 Others   - Failed 
        ************************************************************************/
        [DllImport("fingerprint256x360.dll")]
        public static extern int zzGetTz_ISO(Byte[] input, Byte[] tz);

        /**************************************************************************
        Function:  Compare ANSI the input fingerprint  features with the fingerprint template
        Paramete:  pTemplate  - Pointer to fingerprint template(ANSI format), length = 1024               bytes
                   pFeature   - Pointer to fingerprint feature(ANSI format), length = 1024                bytes
                   SafeLevel - Safety level, 1-5, generally 3
        Return:	    1        - Successed；
                   Others    - Failed 
        **************************************************************************/
        [DllImport("fingerprint256x360.dll")]
        public static extern int zzVerifyFingerPrint_ANSI(Byte[] pTemplate, Byte[] pFeature, int SafeLevel);

        /**************************************************************************
       Function:  Compare ISO the input fingerprint features with the fingerprint template
       Paramete:  pTemplate  - Pointer to fingerprint template(ISO format), length = 1024                bytes
                  pFeature   - Pointer to fingerprint feature(ISO format), length = 1024                 bytes
		          SafeLevel  - Safety level, 1-5, generally 3
       Return:	  1          - Successed；
                  Others     - Failed 	
        **************************************************************************/
        [DllImport("fingerprint256x360.dll")]
        public static extern int zzVerifyFingerPrint_ISO(Byte[] pTemplate, Byte[] pFeature, int SafeLevel);

        /********************************************************************************************
	     Function：Compress fingerprint image
         Parameter：ImgX,ImgY,ImgDPI  - Input parameters, fingerprint image width, height, image DPI
                    pFingerImgBuf     - Input parameter, fingerprint image data pointer, fingerprint image is raw format
                    nCompressRatio    - Input parameters, image compression ratio (5-15)
                    pCompressedImgBuf - Output parameter to store the pointer of compressed image data.
                                        The caller should allocate 20 bytes of memory for pcompressedimgbuf before calling this function.
                    strBuf            - If an error occurs in the compressed image, strbuf fills  in the error message
                    nLength           - Output parameter,Compress wsp length
        Return：    1                - successed 
                    others            - failed
       ********************************************************************************************/
        [DllImport("fingerprint256x360.dll")]
        public static extern int zzFP_Compress(int ImgX, int ImgY, int ImgDPI,
                       Byte[] pFingerImgBuf, int nCompressRatio, Byte[] pCompressedImgBuf, Byte[] strBuf, int[] nLength);



        /**********************************************************************************
        Function: Reproduction of fingerprint compressed image data
        Parameter:pCompressedImgBuf  - Input parameters, compressed image data pointer, 	                                
                                        compressed  data length 20K bytes
                  pFingerImgBuf      - The output parameter is the data pointer of the duplicated fingerprint image. 
                                        The fingerprint image is in raw format.Before calling this function, 
                                        the caller should allocate  the memory of imgx * imgy bytes to pfingerimgbuf.
                  strBuf             - If an error occurs in the Decompress image, strbuf fills in the error message
                  ImgDPI,ImgX, ImgY  -  Input parameters, DPI,image width, height of the                                    
                                        original image corresponding to the compressed image       
        Return：   1                 - successed 
                  others              - failed
        **************************************************************************************/
        [DllImport("fingerprint256x360.dll")]
        public static extern int zzFP_Decompress(Byte[] pCompressedImgBuf, Byte[] pFingerImgBuf, Byte[] strBuf,
            int[] ImgDPI, int[] ImgX, int[] ImgY);
    }
}
