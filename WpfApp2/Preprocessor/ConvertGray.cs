using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Preprocessor
{
    static class ConvertGray
    {
        /// <summary>
        /// Convert an image to 8-bit indexed gray
        /// </summary>
        /// <param name="imgInput"></param>
        /// <returns></returns>
        public static Image ConvertGrays(Image imgInput)
        {
            
            if (imgInput.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
                return imgInput; //Check the input image and return itself if it being desired format
            }
            {
                byte[] gray = GrayBMP_File.CreateGrayBitmapArray(imgInput);
                Image result = ImageConverter.byteArrayToImage(gray);
                Bitmap tempResult = (Bitmap)result;
                tempResult.SetResolution(imgInput.HorizontalResolution, imgInput.VerticalResolution);

                return tempResult;
            }



        }

        public static string SaveAndRead(string filename)
        {
            //string bmstr = @"C:\Users\admin\source\repos\RGB2Gray\japan3.png";
            var bmstr = filename;
            var temFile = bmstr.Substring(bmstr.LastIndexOf('\\') + 1);
            var temFile2 = temFile.Substring(0, temFile.LastIndexOf('.'));
            string graystr = bmstr.Substring(0, bmstr.LastIndexOf('\\') + 1) + temFile2 + "BMPGray.png";
            Console.WriteLine(graystr);

            Image sourcebm;
            Image graybm;

            if (!File.Exists(graystr))
            {
                try
                {
                    Stopwatch stpWatch = Stopwatch.StartNew();
                    sourcebm = Image.FromFile(bmstr);
                    graybm = ConvertGrays(sourcebm);
                    graybm.Save(graystr);
                    stpWatch.Stop();
                    var t = stpWatch.Elapsed.TotalMilliseconds.ToString();
                    Console.WriteLine("Convert RGB to Gray sucessfully within {0} ms", t);
                    return graystr;


                }
                catch (Exception e)
                {

                    Console.WriteLine("Error message" + e.Message);
                    return null;
                }
              

            }
            return graystr;



        }





    }
}
