using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Preprocessor
{
    public class gmseDeskew
    {
        /// <summary>
        /// Representation of a line in the image
        /// </summary>
        public class HougLine
        {
            //Count of points in line 
            public int Count;
            //Index in matrix
            public int Index;
            //The  line is represented as all x,y that solve y*cos(alpha)-x*sin(alpha)=d
            public double Alpha;
            public double d;
        }

        //The Bitmap
        Bitmap cBmp;
        //The range of angles to search for line
        double cAlphaStart = -20;
        double cAlphaStep = 0.2;
        int cSteps = 40 * 5;
        //PreCalculation of sin and cos
        double[] cSinA;
        double[] cCosA;
        //Range of d
        double cDMin;
        double cDStep = 1;
        int cDCount;
        //Count of points that fit in a line
        int[] cHMatrix;

        //Calculate the skew angle of the image cBmp

        public double GetSkewAngle()
        {
            HougLine[] hl;
            //int i;
            double sum = 0;
            int count = 0;

            //Hough Transformation
            Calc();
            //Top 20 of the dedetected lines in the image
            hl = GetTop(20);
            //Average angle of the lines
            for (int i = 0; i < 19; i++)
            {
                sum += hl[i].Alpha;
                count += 1;

            }
            return sum / count;
        }

        //Calculate the Count lines in the image with most points
        private HougLine[] GetTop(int Count)
        {
            HougLine[] hl;
            int j;
            HougLine tmp;
            int AlphaIndex, dIndex;
            hl = new HougLine[Count];
            for (int i = 0; i < Count; i++)
            {
                hl[i] = new HougLine();

            }

            for (int i = 0; i < cHMatrix.Length-1; i++)
            {
                if (cHMatrix[i]>hl[Count-1].Count)
                {
                    hl[Count - 1].Count = cHMatrix[i];
                    hl[Count - 1].Index = i;
                    j = Count - 1;
                    while (j>0 && hl[j].Count > hl[j-1].Count)
                    {
                        tmp = hl[j];
                        hl[j] = hl[j - 1];
                        hl[j - 1] = tmp;
                        j -= 1;
                    }
                }
            }

            for (int i = 0; i < Count; i++)
            {
                dIndex = hl[i].Index / cSteps;
                AlphaIndex = hl[i].Index - dIndex * cSteps;
                hl[i].Alpha = GetAlpha(AlphaIndex);
                hl[i].d = dIndex + cDMin;
            }
            return hl;


        }

        public gmseDeskew(Bitmap bmp)
        {
            cBmp = bmp;
        }

        //Hough Transformation

        private void Calc()
        {
            int x;
            int y;
            int hMin = cBmp.Height/4;
            int hMax = cBmp.Height * 3 / 4;
            Init();
            for (y = hMin; y<hMax  ; y++)
            {
                for (x = 1; x < cBmp.Width -2; x++)
                {
                    //Only lower edges are considered
                    if (IsBlack(x,y) == true)
                    {
                        if (IsBlack(x,y+1) == false)
                        {
                            Calc(x, y);
                        }
                    }
                }
            }
        }

        //Calculate all lines through the point (x,y)
        private void Calc(int x, int y)
        {
            double d;
            int dIndex;
            int Index;
            for (int alpha =  0; alpha < cSteps-1; alpha++)
            {
                d = y * cCosA[alpha] - x * cSinA[alpha];
                dIndex = (int)CalcDIndex(d);
                Index = dIndex * cSteps + alpha;

                try
                {
                    cHMatrix[Index] += 1;

                }
                catch (Exception e)
                {

                    Debug.WriteLine(e.ToString());
                }

            }
        }

        private double CalcDIndex(double d)
        {
            return Convert.ToInt32(d - cDMin);
        }
        private bool IsBlack(int x,int y)
        {
            Color c;
            double luminance;
            c = cBmp.GetPixel(x, y);
            luminance = (c.R * 0.299) + (c.G * 0.587) + (c.B * 0.114);
            return luminance < 140;
        }

        private void Init()
        {
            double angle;
            //Precalculation of sin and cos
            cSinA = new double[cSteps - 1];
            cCosA = new double[cSteps - 1];
            for (int i = 0; i < cSteps-1; i++)
            {
                angle = GetAlpha(i) *Math.PI / 180.0;
                cSinA[i] = Math.Sin(angle);
                cCosA[i] = Math.Cos(angle);

            }
            //Range of d
            cDMin = -cBmp.Width;
            cDCount = (int) (2*(cBmp.Width +cBmp.Height)/cDStep);
            cHMatrix = new int[cDCount * cSteps];

        }
        public double GetAlpha(int Index)
        {
            return cAlphaStart + Index * cAlphaStep;
        }
        public static Bitmap RotateImage(Bitmap bmp, double angle)
        {
            Graphics g;
            Bitmap tmp = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format24bppRgb);
            tmp.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
            g = Graphics.FromImage(tmp);
            try
            {
                g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
                g.RotateTransform((float)angle);
                g.DrawImage(bmp, 0, 0);

            }
            finally
            {
                g.Dispose();
            }
            return tmp;
        }

        public static Bitmap RotateImage2(Bitmap bmp, double angle)
        {
            Graphics g;
            Bitmap tmp = new Bitmap(bmp.Width, bmp.Height);
            tmp.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
            g = Graphics.FromImage(tmp);
            try
            {
                //g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
                //move rotation point to center of original image
                g.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
                //rotation
                g.RotateTransform((float)angle);
                //move image back
                g.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);
                //darw paseed in image onto graphics object
                g.DrawImage(bmp, 0, 0);

            }
            finally
            {
                g.Dispose();
            }
            return tmp;
        }
        /// <summary>
        /// Save and read Image Deskew
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        //public static string SaveAndRead(string filename)
        //{
        //    const double MINIMUM_DESKEW_THRESHOLD = 0.05d;
        //    //string bmstr = @"C:\Users\admin\source\repos\RGB2Gray\japan3.png";
        //    var bmstr = filename;
        //    var temFile = bmstr.Substring(bmstr.LastIndexOf('\\') + 1);
        //    var temFile2 = temFile.Substring(0, temFile.LastIndexOf('.'));
        //    string graystr = bmstr.Substring(0, bmstr.LastIndexOf('\\') + 1) + temFile2 + "Deskew2.png";
        //    Console.WriteLine(graystr);

        //    Bitmap sourcebm = null;
        //    Bitmap graybm = null;

        //    if (!File.Exists(graystr))
        //    {
        //        try
        //        {
        //            Stopwatch stpWatch = Stopwatch.StartNew();
        //            sourcebm = new Bitmap(bmstr);
        //            //Using deskew algorithm here
        //            gmseDeskew skew= new gmseDeskew(sourcebm);
        //            double imageSkewAngle = skew.GetSkewAngle();
        //            if (imageSkewAngle > MINIMUM_DESKEW_THRESHOLD || imageSkewAngle <-(MINIMUM_DESKEW_THRESHOLD))
        //            {
        //                Console.WriteLine("imageSkewAngle: " + imageSkewAngle);
        //                graybm = RotateImage2((Bitmap)skew.cBmp, -imageSkewAngle);


        //                graybm.Save(graystr);
        //                stpWatch.Stop();
        //                var t = stpWatch.Elapsed.TotalMilliseconds.ToString();
        //                Console.WriteLine("Convert RGB to Gray sucessfully within {0} ms", t);
        //                return graystr;

        //            }
        //            return null;
                    
        //        }
        //        catch (Exception e)
        //        {

        //            Console.WriteLine("Error message" + e.Message);
        //            return null;
        //        }
        //        finally
        //        {
        //            if (sourcebm != null)
        //            {
        //                sourcebm.Dispose();
        //            }
        //            if (graybm != null)
        //            {
        //                graybm.Dispose();
        //            }

        //        }


        //    }
        //    return graystr;



        //}

        // Return deskewedImage
        public static Bitmap DeskewImage(Image currentImg)
        {
            const double MINIMUM_DESKEW_THRESHOLD = 0.05d;
            //string bmstr = @"C:\Users\admin\source\repos\RGB2Gray\japan3.png";
            //var bmstr = filename;
            //var temFile = bmstr.Substring(bmstr.LastIndexOf('\\') + 1);
            //var temFile2 = temFile.Substring(0, temFile.LastIndexOf('.'));
            //string graystr = bmstr.Substring(0, bmstr.LastIndexOf('\\') + 1) + temFile2 + "Deskew2.png";
            //Console.WriteLine(graystr);

            Bitmap sourcebm = null;
            Bitmap graybm = null;

            sourcebm = new Bitmap(currentImg);
            //Using deskew algorithm here
            gmseDeskew skew = new gmseDeskew(sourcebm);
            double imageSkewAngle = skew.GetSkewAngle();
            if (imageSkewAngle > MINIMUM_DESKEW_THRESHOLD || imageSkewAngle < -(MINIMUM_DESKEW_THRESHOLD))
            {
                Console.WriteLine("imageSkewAngle: " + imageSkewAngle);
                graybm = RotateImage2((Bitmap)skew.cBmp, -imageSkewAngle);
                return graybm;

            }
            else
                return sourcebm;


        }



        }
    }
