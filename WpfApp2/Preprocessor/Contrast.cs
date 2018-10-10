using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Preprocessor
{
    static class Contrast
    {
        static Image ContrastPerform(Image bmp, float value)
        {
            Image contrsBmp = null;

            try
            {
                var matrix = new float[][] {
                    new float[] {value, 0, 0, 0, 0},
                    new float[] {0, value, 0, 0, 0},
                    new float[] {0, 0, value, 0, 0},
                    new float[] {0, 0, 0, 1f, 0},
                    //including the BLATANT FUDGE
                    new float[] {0.001f, 0.001f, 0.001f, 0, 1f}
                };

                ColorMatrix cm = new ColorMatrix(matrix);

                ImageAttributes ia = new ImageAttributes();
                ia.SetColorMatrix(cm);

                contrsBmp = new Bitmap(bmp.Width, bmp.Height);
                ((Bitmap)contrsBmp).SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
                using (Graphics g = Graphics.FromImage(contrsBmp))
                {
                    g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, ia);
                    ia.Dispose();
                }
            }
            catch
            {
                if (contrsBmp != null)
                {
                    contrsBmp.Dispose();
                    contrsBmp = null;
                }
            }

            return contrsBmp;
        }
    }
}
