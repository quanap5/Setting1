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
    static class CropImage
    {
        public static Image Crop(Image ori_image, Rectangle cropArea)
        {

            Bitmap bmp = new Bitmap(cropArea.Width, cropArea.Height);
            bmp.SetResolution(ori_image.HorizontalResolution, ori_image.VerticalResolution);
            Graphics grph = Graphics.FromImage(bmp);
            grph.DrawImage(ori_image, 0, 0, cropArea, GraphicsUnit.Pixel);
            grph.Dispose();
            return bmp;

        }


        public static string SaveAndRead(string filename, Rectangle cropArea)
        {
            //string bmstr = @"C:\Users\admin\source\repos\RGB2Gray\japan3.png";
            var bmstr = filename;
            var temFile = bmstr.Substring(bmstr.LastIndexOf('\\') + 1);
            var temFile2 = temFile.Substring(0, temFile.LastIndexOf('.'));
            string graystr = bmstr.Substring(0, bmstr.LastIndexOf('\\') + 1) + temFile2 + "Croped.png";
            Console.WriteLine(graystr);

            Image sourcebm;
            Image graybm;

            if (!File.Exists(graystr))
            {
                try
                {
                    Stopwatch stpWatch = Stopwatch.StartNew();
                    sourcebm = Image.FromFile(bmstr);
                    graybm = Crop(sourcebm, cropArea);
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
