using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenAccount.Data
{
    public class ImageHelper
    {
        public static Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
        public static bool IsSignImageValid(string base64, Color seperateColor, double quality)
        {
            try
            {
                Bitmap bitmap = new Bitmap(Base64ToImage(base64));
                long vaildPixelNum = 0;
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        Color color = bitmap.GetPixel(i, j);
                        if (bitmap.GetPixel(i, j).R < seperateColor.R && bitmap.GetPixel(i, j).G < seperateColor.G && bitmap.GetPixel(i, j).B < seperateColor.B)
                        {
                            vaildPixelNum++;
                        }
                    }
                }
                double realQuality = (double)vaildPixelNum / (bitmap.Width * bitmap.Height);
                if (realQuality > quality) return true;
            }
            catch
            {
                return false;
            }
            return false;
        }
        public static Bitmap ReadImageFile(string path)
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            fs.Dispose();
            Bitmap bit = new Bitmap(result);
            return bit;
        }
        public static Image ZoomSamllerImage(Image SourceImage, int smallerLevel)
        {
            return ZoomImage(SourceImage, SourceImage.Width / smallerLevel, SourceImage.Height / smallerLevel);
        }
        public static Image ZoomImage(Image sourceImage, int targetWidth, int targetHeight)
        {
            try
            {
                int width;
                int height;

                System.Drawing.Imaging.ImageFormat format = sourceImage.RawFormat;
                Bitmap targetPicture = new Bitmap(targetWidth, targetHeight);
                Graphics g = Graphics.FromImage(targetPicture);
                g.Clear(Color.White);
                if (sourceImage.Width > targetWidth && sourceImage.Height <= targetHeight)
                {
                    width = targetWidth;
                    height = (width * sourceImage.Height) / sourceImage.Width;
                }
                else if (sourceImage.Width <= targetWidth && sourceImage.Height > targetHeight)
                {
                    height = targetHeight;
                    width = (height * sourceImage.Width) / sourceImage.Height;
                }
                else if (sourceImage.Width <= targetWidth && sourceImage.Height <= targetHeight)
                {
                    width = sourceImage.Width;
                    height = sourceImage.Height;
                }
                else
                {
                    width = targetWidth;
                    height = (width * sourceImage.Height) / sourceImage.Width;
                    if (height > targetHeight)
                    {
                        height = targetHeight;
                        width = (height * sourceImage.Width) / sourceImage.Height;
                    }
                }
                g.DrawImage(sourceImage, (targetWidth - width) / 2, (targetHeight - height) / 2, width, height);
                sourceImage.Dispose();
                return targetPicture;
            }
            catch (Exception ee)
            { throw ee; }
        }
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH, int Mode)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }
        public static string BitmapToBase64(Bitmap bmp)
        {
            string dst = "";
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Jpeg);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                ms.Close();
                dst = Convert.ToBase64String(byteImage);
                byteImage = null;
                ms.Dispose();
            }
            return dst;
        }
        public static Bitmap Base64StringToBitmap(string inputStr)
        {
            Bitmap bmp = null;
            try
            {
                byte[] arr = Convert.FromBase64String(inputStr);
                MemoryStream ms = new MemoryStream(arr);
                bmp = new Bitmap(ms);
                GC.Collect();
            }
            catch
            {
                Console.WriteLine("Base64StringToImage FAILED");
            }
            return bmp;
        }
        public static string GetBase64FromImage(string imagefile)
        {
            string strbaser64 = "";
            try
            {
                if (!File.Exists(imagefile))
                {
                    return "";
                }
                Bitmap bmp = (Bitmap)Image.FromFile(imagefile);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                strbaser64 = Convert.ToBase64String(arr);
            }
            catch
            {
                Console.WriteLine("Base64StringToImage FAILED");
            }
            return strbaser64;
        }
    }
}
