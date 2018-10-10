using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tesseract;

namespace WpfApp2.Models
{
    public class TesseractOCR
    {
        private string executedTime;
        private ViewModels.OCRViewModel taskViewModel;
        private Boolean IsSelectedLanguage;

        #region Mode of language
        private TesseractEngine curOcr;
        #endregion

        private Dictionary<SolidColorBrush, List<Rectangle>> ocrDetectedRegion;
        /// <summary>
        /// Loading tesseractMode based on language parametter
        /// </summary>
        /// <param name="vm">context of OCRViewmodel</param>
        public TesseractOCR(ViewModels.OCRViewModel vm)
        {
            taskViewModel = vm;

            ///Check language selected to perform respective TesseractEnginee
            if (taskViewModel._selectedLang.English == true)
            {
                curOcr = new TesseractEngine("./tessdata", "eng", EngineMode.Default);
                IsSelectedLanguage = true;
                return;
            }
            if (taskViewModel._selectedLang.Korean == true)
            {
                curOcr = new TesseractEngine("./tessdata", "kor", EngineMode.Default);
                IsSelectedLanguage = true;
                return;
            }
            if (taskViewModel._selectedLang.Japanese == true)
            {
                curOcr = new TesseractEngine("./tessdata", "jpn", EngineMode.Default);
                IsSelectedLanguage = true;
                return;
            }
            if (taskViewModel._selectedLang.Auto == true)
            {
                curOcr = new TesseractEngine("./tessdata", "eng+kor+jpn", EngineMode.Default);
                IsSelectedLanguage = true;
                return;
            }
            else
            {
                IsSelectedLanguage = false;
            }
          

        }

       /// <summary>
       /// Get time consuming for running
       /// </summary>
       /// <returns></returns>
        public String getTime()
        {
            return this.executedTime;
        }
        public List<string> ListImageOCR(List<ImageClass> list)
        {
            throw new NotImplementedException();
        }

        public string OneImageOCR(ImageClass one)
        {

            return runningOCR(one);
        }         
        /// <summary>
        /// Click run button to start the OCR function
        /// </summary>
        /// <param name="currentImg">Is image we are processing it</param>
        /// <returns></returns>
        private string runningOCR(ImageClass currentImg)
        {
            
            try
            {
                if (currentImg == null)
                {
                    //return "please open at least one image";
                    MessageBox.Show("Please open at least one image");
                    return null;
                }

                // Check the language if it be selected or not
                if (!IsSelectedLanguage)
                {
                    MessageBox.Show("Please select language for recognizing");
                    return "Waiting for Select language";
                }
                else
                {
                    //taskViewModel.OutPutText = "OCR is running....";
                
                    {
                        Stopwatch stopW = Stopwatch.StartNew();
                        //Pix.LoadFromFile
                        using (var img = PixConverter.ToPix(ImageConverter.BitmapImage2Bitmap(currentImg.Image)))
                        {
                            using (var page = curOcr.Process(img))
                            {
                                var resultText = page.GetText();
                                if (!String.IsNullOrEmpty(resultText))
                                {
                                    stopW.Stop();
                                    var time_dur = stopW.Elapsed.TotalMilliseconds.ToString();
                                    this.executedTime = time_dur;

                                    List<Rectangle> _charBoxs = page.GetSegmentedRegions(PageIteratorLevel.Symbol);
                                    List<Rectangle> _wordBoxs = page.GetSegmentedRegions(PageIteratorLevel.Word);
                                    List<Rectangle> _lineBoxs = page.GetSegmentedRegions(PageIteratorLevel.TextLine);
                                    List<Rectangle> _paraBoxs = page.GetSegmentedRegions(PageIteratorLevel.Para);
                                   
                                    Console.WriteLine("Number of Character is: "+ _charBoxs.Count);
                                    Console.WriteLine("Number of Word is: " + _wordBoxs.Count);
                                    Console.WriteLine("Number of Line is: " + _lineBoxs.Count);
                                    Console.WriteLine("Number of Paragraph is: " + _paraBoxs.Count);

                                    Console.WriteLine("Coordinate x of first word:" + _charBoxs[0].X + "y: " + _charBoxs[0].Y + "width: " + _charBoxs[0].Width + "Heigh: " + _charBoxs[0].Height);

                                    Console.WriteLine("Coordinate x of first word:"+ _wordBoxs[0].X +"y: "+_wordBoxs[0].Y + "width: "+_wordBoxs[0].Width + "Heigh: "+_wordBoxs[0].Height);

                                    Console.WriteLine("Coordinate x of first word:" + _lineBoxs[0].X + "y: " + _lineBoxs[0].Y + "width: " + _lineBoxs[0].Width + "Heigh: " + _lineBoxs[0].Height);

                                    Console.WriteLine("Coordinate x of first Para:" + _paraBoxs[0].X + "y: " + _paraBoxs[0].Y + "width: " + _paraBoxs[0].Width + "Heigh: " + _paraBoxs[0].Height);
                                    float HOCRT = page.GetMeanConfidence();
                                    Console.WriteLine(HOCRT);

                                    Console.WriteLine(time_dur);
                                    // Save to property member
                                    ocrDetectedRegion= new Dictionary<SolidColorBrush, List<Rectangle>>();
                                    ocrDetectedRegion.Add(new SolidColorBrush(Colors.Violet), _charBoxs);
                                    ocrDetectedRegion.Add(new SolidColorBrush(Colors.Yellow), _wordBoxs);
                                    ocrDetectedRegion.Add(new SolidColorBrush(Colors.Green), _lineBoxs);
                                    ocrDetectedRegion.Add(new SolidColorBrush(Colors.Red), _paraBoxs);

                                    return resultText;
                                }
                            }
                        }
                    }


                    //textEditor.Text = page.GetText();
                }

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        public Dictionary<SolidColorBrush, List<Rectangle>> GetocrDetectedRegion()
        {
            return this.ocrDetectedRegion;
        }

    }

     
}
