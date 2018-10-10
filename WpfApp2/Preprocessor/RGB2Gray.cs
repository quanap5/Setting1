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
    static class RGB2Gray
    {
        public static Bitmap Convert2Grayscale(Bitmap source)
        {
            Bitmap bm = new Bitmap(source.Width, source.Height);
            for (int indexY = 0; indexY < bm.Height; indexY++)
            {
                for (int indexX = 0; indexX < bm.Width; indexX++)
                {
                    Color c = source.GetPixel(indexX, indexY);
                    int average = (Convert.ToInt32(c.R) + Convert.ToInt32(c.G) + Convert.ToInt32(c.B)) / 3;
                    bm.SetPixel(indexX, indexY, Color.FromArgb(average, average, average));

                }
            }
            return bm;

        }

       //public static string SaveAndRead(string filename)
       // {
       //     //string bmstr = @"C:\Users\admin\source\repos\RGB2Gray\japan3.png";
       //     var bmstr = filename;
       //     var temFile = bmstr.Substring(bmstr.LastIndexOf('\\') + 1);
       //     var temFile2 = temFile.Substring(0, temFile.LastIndexOf('.'));
       //     string graystr = bmstr.Substring(0, bmstr.LastIndexOf('\\')+1) + temFile2 + "Gray.jpg";
       //     Console.WriteLine(graystr);

       //     Bitmap sourcebm = null;
       //     Bitmap graybm = null;

       //     if ( !File.Exists(graystr))
       //     {
       //         try
       //         {
       //             Stopwatch stpWatch = Stopwatch.StartNew();
       //             sourcebm = new Bitmap(bmstr);
       //             graybm = Convert2Grayscale(sourcebm);
       //             graybm.Save(graystr);
       //             stpWatch.Stop();
       //             var t = stpWatch.Elapsed.TotalMilliseconds.ToString();
       //             Console.WriteLine("Convert RGB to Gray sucessfully within {0} ms", t);
       //             return graystr;


       //         }
       //         catch (Exception e)
       //         {

       //             Console.WriteLine("Error message" + e.Message);
       //             return null;
       //         }
       //         finally
       //         {
       //             if (sourcebm != null)
       //             {
       //                 sourcebm.Dispose();
       //             }
       //             if (graybm != null)
       //             {
       //                 graybm.Dispose();
       //             }

       //         }


       //     }
       //     return graystr;



       // }
    }
}
