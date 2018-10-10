using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfApp2.Commands;
using WpfApp2.Models;
using System.Collections.ObjectModel;
using WpfApp2.Preprocessor;
using System.ComponentModel;
using WpfApp2;
using WpfApp2.Views;
using System.Drawing;
using System.Windows.Media;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Diagnostics;
using WpfApp2.Models;
using WpfApp2.ViewModels;

namespace WpfApp2.ViewModels
{
    /// <summary>
    /// ViewModel of Model-View-ViewModel (MVVM) design pattern
    /// 1. View: as UI
    /// 2. Models folder: contain Data Class
    /// 3. ViewModel: Glue code (Event handling, Binding, logical processing)
    /// </summary>
    public class OCRViewModel : ViewModelBase
    {
        /// <summary>
        /// LangClass ussed in bindding data on Menu
        /// </summary>
        public class Lang
        {
            public Boolean English { get; set; }
            public Boolean Korean { get; set; }
            public Boolean Japanese { get; set; }
            public Boolean Auto { get; set; }
            public Lang()
            {
                this.English = false;
                this.Korean = false;
                this.Japanese = false;
                this.Auto = false;

            }

        }

        System.Drawing.Point anchorPoint = new System.Drawing.Point(); //used in croping
        Rectangle cropedRect; //used in croping
        private bool isDragging = false; //used in croping
        private bool isSetting = false; //used to check setting window open or nor

        JsonOCRClass jsonObject; //Contain information of OCR

        public Lang _selectedLang;
        public Lang SelectedLang
        {
            get { return _selectedLang; }
            set
            {
                _selectedLang = value;
                OnPropertyChanged("SelectedLang");
                Console.WriteLine("update item lang selection");
                _tesseractOCR = new TesseractOCR(this);
            }
        }
        #region Commands
        public ICommand OpenImageCommand { get; set; }
        public ICommand StartOCRCommand { get; set; }
        public ICommand StartAllOCRCommand { get; set; }
        public ICommand RGB2GrayCommand { get; set; }
        public ICommand OpenImgInfoCommand { get; set; }
        public ICommand OpenCropSettingsCommand { get; set; }
        public ICommand ContrastAdjustCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand Save2JsonCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MouseLeftButtonDownCommand { get; set; }
        public ICommand MouseLeftButtonUpCommand { get; set; }
        public ICommand MouseMoveCommand { get; set; }
        public ICommand RunToGray1Command { get; set; }
        public ICommand RunToGray2Command { get; set; }
        public ICommand DeskewCommand { get; set; }


        private Dictionary<SolidColorBrush, List<Rectangle>> _ocrDetectedRegionVM;
        //For segmented region (char, word, line, para)
        private ObservableCollection<RectItemClass> _rectItems;
        public ObservableCollection<RectItemClass> RectItems
        {
            get { return _rectItems; }
            set
            {
                _rectItems = value;
                OnPropertyChanged("RectItems");
                Console.WriteLine("update drawing");
            }
        }
        //For cropped by hand using mouse
        private ObservableCollection<RectItemClass> _cropRect;
        public ObservableCollection<RectItemClass> CropRect
        {
            get { return _cropRect; }
            set
            {
                _cropRect = value;
                OnPropertyChanged("CropRect");
                Console.WriteLine("update draw croped rect");
            }
        }
        //For cropped by setting opption
        public ObservableCollection<RectItemClass> _cropRectSettings;
        public ObservableCollection<CropSettingsViewModel.Item> _curCropSetting;
        public ObservableCollection<RectItemClass> CropRectSettings
        {
            get
            {
                return _cropRectSettings;
            }
            set
            {
                _cropRectSettings = value;
                //_cropRect2curRect();
                OnPropertyChanged("CropRectSettings");
                Console.WriteLine("-----------------------");
                //_cropRect2curRect();


            }

           
        }

        private void _cropRect2curRect()
        {
            var item = _cropRectSettings[_cropRectSettings.Count()-1];
            
            _curCropSetting.Add(new CropSettingsViewModel.Item("Manual " + (_cropRectSettings.Count()).ToString(), item.Rect.X, item.Rect.Y, item.Rect.Width, item.Rect.Height));
            Debug.WriteLine("_curCropSetting: " + _curCropSetting.Count().ToString());
           
            
        }

        #endregion

        #region pathToTessData
        private TesseractOCR _tesseractOCR;
        private readonly string _pathTessData = Environment.CurrentDirectory + @"\tessdata";
        #endregion

        #region Properties
        private List<ImageClass> _imagesList;
        public List<ImageClass> ImagesList
        {
            get { return _imagesList; }
            set
            {
                _imagesList = value;
                OnPropertyChanged("ImagesList");
            }
        }

        //Single Image
        private ImageClass _imageOne;

        public ImageClass ImageOne
        {
            get { return _imageOne; }
            set
            {
                _imageOne = value;
                OnPropertyChanged("ImageOne");

            }
        }

        //List of Image
        private List<ImageClass> _imageList;
        public List<ImageClass> ImageList
        {
            get { return _imageList; }
            set
            {
                _imageList = value;
                OnPropertyChanged("ImageList");
            }
        }


        /// <summary>
        /// this is for current Image, this is being displayed on GUI
        /// </summary>
        private ImageClass _currentImage;
        public ImageClass CurrentImage
        {
            get { return _currentImage; }
            set
            {
                _currentImage = value;
                OnPropertyChanged("CurrentImage");
            }
        }

        #endregion

        private string _outPutText;
        public String OutPutText
        {
            get { return _outPutText; }
            set
            {
                _outPutText = value;
                OnPropertyChanged("OutPutText");
            }
        }

        private string _outTime;
        public CropSetting2 _cropSettings;
        public String OutTime
        {
            get { return _outTime; }
            set
            {
                _outTime = value;
                OnPropertyChanged("OutTime");
            }
        }

        /// <summary>
        /// RadioBut for select RGB2Gray
        /// </summary>
        /// <param name="ocr"></param>
        /// 

        //private Boolean _rgb2grayChecked = false;
        //private Boolean _rgb2grayChecked2 = false;
        //public Boolean RGB2GrayChecked
        //{
        //    get { return _rgb2grayChecked; }
        //    set
        //    {
        //        if (_rgb2grayChecked == false)
        //        {
        //            _rgb2grayChecked = value;
        //            RGB2GrayChecked2 = !(_rgb2grayChecked);
        //            OnPropertyChanged("RGB2GrayChecked");

        //        }
        //        else
        //        {
        //            _rgb2grayChecked = value;
        //            OnPropertyChanged("RGB2GrayChecked");
        //        }
        //        Console.WriteLine("_rgb2graychecked111: " + _rgb2grayChecked.ToString());

        //    }
        //}

        //public Boolean RGB2GrayChecked2
        //{
        //    get { return _rgb2grayChecked2; }
        //    set
        //    {
        //        if (_rgb2grayChecked2 == false)
        //        {
        //            _rgb2grayChecked2 = value;
        //            RGB2GrayChecked = !(_rgb2grayChecked2);
        //            OnPropertyChanged("RGB2GrayChecked2");
        //        }
        //        else
        //        {
        //            _rgb2grayChecked2 = value;
        //            OnPropertyChanged("RGB2GrayChecked2");
        //        }
        //        Console.WriteLine("_rgb2graychecked222: " + _rgb2grayChecked2.ToString());
        //    }
        //}

        //private Boolean _deskewChecked = false;
        //public Boolean DeskewChecked
        //{
        //    get
        //    {
        //        return _deskewChecked;
        //    }
        //    set
        //    {
        //        _deskewChecked = value;
        //        OnPropertyChanged("DeskewChecked");
        //        Console.WriteLine("_deskewChecked:" + _deskewChecked);
        //    }
        //}

        /// <summary>
        /// This is used to display region box including (charBox, wordBox, lineBox and ParagraphBox)
        /// </summary>
        #region Option for vivualization of the detected region
        private Boolean _charChecked = false;
        private Boolean _wordChecked = false;
        private Boolean _lineChecked = false;
        private Boolean _paraChecked = false;


        public Boolean CharChecked
        {
            get { return _charChecked; }
            set
            {
                _charChecked = value;
                OnPropertyChanged("CharChecked");
                DrawocrDetectedRegion();
            }
        }

        public Boolean WordChecked
        {
            get { return _wordChecked; }
            set
            {
                _wordChecked = value;
                OnPropertyChanged("WordChecked");
                DrawocrDetectedRegion();
            }
        }

        public Boolean LineChecked
        {
            get { return _lineChecked; }
            set
            {
                _lineChecked = value;
                OnPropertyChanged("LineChecked");
                DrawocrDetectedRegion();
            }
        }

        public Boolean ParaChecked
        {
            get { return _paraChecked; }
            set
            {
                _paraChecked = value;
                OnPropertyChanged("ParaChecked");
                DrawocrDetectedRegion();
            }
        }

        #endregion

        public OCRViewModel()
        {
            //_tesseractOCR = new TesseractOCR(this);
            OpenImageCommand = new RelayCommand(OpenImage);
            //StartOCRCommand = new RelayCommand(StartOCR);
            StartOCRCommand = new RelayCommand(StartOCRwithmultiRegions);
            
            StartAllOCRCommand = new RelayCommand(StartAllOCR);
            CloseWindowCommand = new RelayCommand(CloseWindow);

            _selectedLang = new Lang();
            // MouseViewModel mouse = new MouseViewModel();
            //mouse = new MouseViewModel();
            MouseLeftButtonDownCommand = new RelayCommand2(para => MouseLeftButtonDown((MouseEventArgs)para));
            MouseLeftButtonUpCommand = new RelayCommand2(para => MouseLeftButtonUp((MouseEventArgs)para));
            MouseMoveCommand = new RelayCommand2(para => MouseMove((MouseEventArgs)para));

            //Preprocessing Command
            RunToGray1Command = new RelayCommand(RunToGray1);
            RunToGray2Command = new RelayCommand(RunToGray2);
            DeskewCommand = new RelayCommand(Deskew);


            OpenImgInfoCommand = new RelayCommand(OpenImgInfo);
            OpenCropSettingsCommand = new RelayCommand(OpenCropSettings);

            ContrastAdjustCommand = new RelayCommand(ContrastAdjust);
            Save2JsonCommand = new RelayCommand(Save2Json);
            ChangeLanguageCommand = new RelayCommand(ChangeLanguage);
            OutPutText = "Please open an image first";

            //Process List of Image
            ImageList = new List<ImageClass>();

            //Croped region by setting
            _curCropSetting = new ObservableCollection<CropSettingsViewModel.Item>();

            CropRectSettings = new ObservableCollection<RectItemClass>();
            //CropRectSettings.Add(new RectItemClass(new SolidColorBrush(Colors.Pink), new Rectangle(20,20,300,300), Visibility.Visible));
            //CropRectSettings.Add(new RectItemClass(new SolidColorBrush(Colors.Pink), new Rectangle(20, 20, 100, 100), Visibility.Visible));
            _cropSettings = new CropSetting2(this);
        }

        private void OpenCropSettings()
        {
            //for cropsettings1
            //_cropSettings = new CropSettings(this);
            //_cropSettings.Show();

            //for cropsettings2
            if (_cropSettings==null)
            {
                Debug.WriteLine("Ko ton tai _CropSettings");
                return;
            }
            _cropSettings.Show();
            isSetting = true;
            //Window myOwnedWindow = new Window();
            //myOwnedWindow.Owner = this;
            //myOwnedWindow.Show();


        }

        private void RunToGray1()
        {
            //string filename = RGB2Gray.SaveAndRead(ImageOne.FilePath);
            //UpdateProcessedImage(filename);
            Bitmap img = RGB2Gray.Convert2Grayscale(ImageConverter.BitmapImage2Bitmap(_currentImage.Image));
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage = ImageConverter.Bitmap2BitmapImage(img);
            CurrentImage = new ImageClass
            {
                FilePath = "Gray1",
                Image = bitmapImage
            };

            ImageList.Add(CurrentImage);
            Debug.WriteLine("The lenghth of list of Image: " + ImageList.Count().ToString());

        }
        private void RunToGray2()
        {
            //string filename = ConvertGray.SaveAndRead(ImageOne.FilePath);

            Image img = ConvertGray.ConvertGrays(ImageConverter.BitmapImage2Bitmap(_currentImage.Image));
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage = ImageConverter.Image2BitmapImage(img);
            CurrentImage = new ImageClass
            {
                FilePath = "Gray2",
                Image = bitmapImage
            };

            ImageList.Add(CurrentImage);
            Debug.WriteLine("The lenghth of list of Image: " + ImageList.Count().ToString());

            //CurrentImage = ImageOne;
            //UpdateProcessedImage(filename);
        }
        private void Deskew()
        {
            //string filename = gmseDeskew.SaveAndRead(ImageOne.FilePath);
            //UpdateProcessedImage(filename);
            Bitmap img2 = gmseDeskew.DeskewImage(ImageConverter.BitmapImage2Bitmap(_currentImage.Image));
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage = ImageConverter.Bitmap2BitmapImage(img2);
            CurrentImage = new ImageClass
            {
                FilePath = "Deskew",
                Image = bitmapImage
            };

            ImageList.Add(CurrentImage);
            Debug.WriteLine("The lenghth of list of Image: " + ImageList.Count().ToString());

        }


        private void MouseLeftButtonDown(MouseEventArgs e)
        {
            if (isDragging == false)
            {
                Console.WriteLine("This is mouse down: " + e.GetPosition((IInputElement)e.Source));
                isDragging = true;
                anchorPoint.X = (int)e.GetPosition((IInputElement)e.Source).X;
                anchorPoint.Y = (int)e.GetPosition((IInputElement)e.Source).Y;
                cropedRect = new Rectangle();


            }
        }

        private void MouseMove(MouseEventArgs e)
        {
            if (isDragging)
            {
                Console.WriteLine("This is mouse Moving: " + e.GetPosition((IInputElement)e.Source));
                double x = e.GetPosition((IInputElement)e.Source).X;
                double y = e.GetPosition((IInputElement)e.Source).Y;
                cropedRect.X = (int)Math.Min(x, anchorPoint.X);
                cropedRect.Y = (int)Math.Min(y, anchorPoint.Y);
                cropedRect.Width = (int)Math.Abs(x - anchorPoint.X);
                cropedRect.Height = (int)Math.Abs(y - anchorPoint.Y);
                Console.WriteLine("The WIDTH: " + Math.Abs(x - anchorPoint.X));
                Console.WriteLine("The HEIGHT: " + Math.Abs(y - anchorPoint.Y));
                CropRect = new ObservableCollection<RectItemClass>();
                CropRect.Add(new RectItemClass(new SolidColorBrush(Colors.Red), cropedRect, Visibility.Visible));

            }
        }

        private void MouseLeftButtonUp(MouseEventArgs e)
        {
            if (isDragging)
            {
                Console.WriteLine("This is mouse up: " + e.GetPosition((IInputElement)e.Source));
                isDragging = false;
                if (cropedRect.Width > 0 && cropedRect.Height > 0)
                {
                    CropRect = new ObservableCollection<RectItemClass>();
                    Console.WriteLine("Finish crop");
                    //CropRect.Add(new RectItemClass(new SolidColorBrush(Colors.Red), cropedRect, Visibility.Visible));
                    //string filename = CropImage.SaveAndRead(ImageOne.FilePath, cropedRect);

                    //UpdateProcessedImage(filename);
                    //BitmapImage2Bitma
                    Image img = CropImage.Crop(ImageConverter.BitmapImage2Bitmap(_currentImage.Image), cropedRect);
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage = ImageConverter.Image2BitmapImage(img);

                    if (isSetting)
                    {
                        CropRectSettings.Add(new RectItemClass(new SolidColorBrush(Colors.Black), cropedRect, Visibility.Visible));
                        //_cropSettings.a.Items.Add(new CropSettingsViewModel.Item("Region " + (_cropSettings.a.Items.Count() + 1).ToString(), cropedRect.X, cropedRect.Y, cropedRect.Width, cropedRect.Height));
                        _cropRect2curRect();
                        return;
                        
                    }
                    CurrentImage = new ImageClass
                    {
                        FilePath = "Deskew",
                        Image = bitmapImage
                    };

                    ImageList.Add(CurrentImage);
                    Debug.WriteLine("The lenghth of list of Image: " + ImageList.Count().ToString());

                    
                }

            }
        }


        /// <summary>
        /// Used for Change Language use Icommand parametter
        /// </summary>
        /// <param name="lang">To determine which language was selected</param>
        private void ChangeLanguage(object lang)
        {
            //there is a bit confuse here
            if (lang.ToString().Equals("eng"))
            {
                if (_selectedLang.English == false)
                {
                    Console.WriteLine("Unchecked English");
                    _selectedLang.English = false;
                    SelectedLang = _selectedLang;
                }
                else
                {
                    Console.WriteLine("Checked English");
                    _selectedLang.English = true;
                    _selectedLang.Korean = _selectedLang.Japanese = _selectedLang.Auto = false;
                    SelectedLang = _selectedLang;
                }
                return;
            }

            if (lang.ToString().Equals("kor"))
            {
                if (_selectedLang.Korean == false)
                {
                    Console.WriteLine("Unchecked Korea");
                    _selectedLang.Korean = false;
                    SelectedLang = _selectedLang;

                }
                else
                {
                    Console.WriteLine("Checked Korean");
                    _selectedLang.Korean = true;
                    _selectedLang.English = _selectedLang.Japanese = _selectedLang.Auto = false;
                    SelectedLang = _selectedLang;
                }
                return;
            }
            if (lang.ToString().Equals("jpn"))
            {
                if (_selectedLang.Japanese == false)
                {
                    Console.WriteLine("Unchecked Japanese");
                    _selectedLang.Japanese = false;
                    SelectedLang = _selectedLang;

                }
                else
                {
                    Console.WriteLine("Checked Japnese");
                    SelectedLang.Japanese = true;
                    _selectedLang.English = _selectedLang.Korean = _selectedLang.Auto = false;
                    SelectedLang = _selectedLang;
                }
                return;
            }
            if (lang.ToString().Equals("auto"))
            {
                if (SelectedLang.Auto == false)
                {
                    Console.WriteLine("Unchecked Auto");
                    _selectedLang.Auto = false;
                    SelectedLang = _selectedLang;
                }
                else
                {
                    Console.WriteLine("Checked Auto");
                    _selectedLang.Auto = true;
                    _selectedLang.English = _selectedLang.Korean = _selectedLang.Japanese = false;
                    SelectedLang = _selectedLang;

                }
                return;
            }

        }
        //Close application
        private void CloseWindow()
        {
            Environment.Exit(0);
        }
        //Save output to Json file for further performance
        private void Save2Json()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "text Document",
                DefaultExt = ".txt",
                Filter = "Text document (.txt)|*.txt"
            };

            string path = dlg.ShowDialog() != true ? null : dlg.FileName;


            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine("DO NOT Save");
                return;
            }
            else
            {
                Console.WriteLine("SAVE ok ok");
                using (StreamWriter file = File.CreateText(path))
                {
                    jsonObject = new JsonOCRClass(ImageOne, null, _ocrDetectedRegionVM);
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, jsonObject);
                }

            }

        }

        //Contrast Adjusting
        private void ContrastAdjust()
        {
            ContrasAdjust _contrastAdjust = new ContrasAdjust();
            _contrastAdjust.Show();
            Console.WriteLine("Open image Infro");
        }

        //Window show ImageProperty 
        private void OpenImgInfo()
        {
            if (ImageOne == null)
            {
                MessageBox.Show("Please open at least one image");
                return;
            }

            ShowInfo _showInfo = new ShowInfo(ImageOne);

            _showInfo.Show();
            Console.WriteLine("Open image Infro");
        }
        /// <summary>
        /// StartOCR run when we click menu item run 
        /// It perform on one image
        /// </summary>
        private void StartOCR()
        {

            Console.WriteLine("Executing StartOCR");
            Console.WriteLine(_pathTessData);
            if (!Directory.Exists(_pathTessData))
            {
                MessageBox.Show("You dont have Tess data. OCR can not Run");
            }

            if (_tesseractOCR == null)
            {
                MessageBox.Show("Please select one language for loadding respective model");
            }
            else
            {
                var tem_Text = _tesseractOCR.OneImageOCR(_currentImage);

                if (tem_Text == null)
                {
                    OutPutText = "No answer";
                    OutTime = _tesseractOCR.getTime() + " ms for running";

                }
                else
                {
                    OutPutText = tem_Text;
                    OutTime = _tesseractOCR.getTime() + " ms for running";
                    _ocrDetectedRegionVM = _tesseractOCR.GetocrDetectedRegion();
                    DrawocrDetectedRegion();
                }
            }


        }

        private void StartOCRwithmultiRegions()
        {
            Console.WriteLine("Executing StartOCRwithmultiRegions");
            if (_cropRectSettings.Count>0)
            {
                ImageList = new List<ImageClass>();
                foreach (var item in _cropRectSettings)
                {
                    Image img = CropImage.Crop(ImageConverter.BitmapImage2Bitmap(_currentImage.Image), item.Rect);
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage = ImageConverter.Image2BitmapImage(img);
                    var SubImage = new ImageClass
                    {
                        FilePath = "SubImage",
                        Image = bitmapImage
                    };

                    ImageList.Add(SubImage);
                }
                StartAllOCR();
                
                return;
            }
            MessageBox.Show("No region was selected");

        }

        /// <summary>
        /// StartAllOCR when we click menu item run All
        /// </summary>
        private void StartAllOCR()
        {
            Console.WriteLine("Executing StartAllOCR");
            Console.WriteLine(_pathTessData);
            if (!Directory.Exists(_pathTessData))
            {
                MessageBox.Show("You dont have Tess data. ALLOCR can not Run");
            }

            if (_tesseractOCR == null)
            {
                MessageBox.Show("Please select one language for loadding respective model");
            }
            else
            {
                try
                {


                    OutTime = ""; //reset
                    OutPutText = ""; //reset textbox
                    var index = 0;

                    var outputTextList = new List<string>();
                    foreach (var item in _imageList)
                    {
                        var tem_Text = _tesseractOCR.OneImageOCR(item);
                        outputTextList.Add(tem_Text);
                        OutTime = string.Concat(OutTime, _tesseractOCR.getTime() + "(ms); ");

                        OutPutText = string.Concat(_outPutText, "===============" + "Img " + index.ToString() + "===============\n");
                        this.OutPutText = string.Concat(_outPutText, tem_Text);
                        index += 1;
                    }

                    //OutPutText = ""; //reset textbox
                    //var index = 0;
                    //foreach (var item in outputTextList)
                    //{

                    //    OutPutText = string.Concat(_outPutText, "==============="+"Img "+index.ToString()+"===============\n");
                    //    OutPutText = string.Concat(_outPutText, item);
                    //    index+=1;

                    //}

                }
                catch (Exception e)
                {
                    //throw;
                    Debug.WriteLine(e);
                }
            }

        }
        /// <summary>
        /// This is used to update rectangle box to ObservableCollect and binding to UI
        /// </summary>
        private void DrawocrDetectedRegion()
        {
            RectItems = new ObservableCollection<RectItemClass>();
            if (_ocrDetectedRegionVM != null)
            {
                foreach (SolidColorBrush colr in _ocrDetectedRegionVM.Keys)
                {

                    if (colr.Color == Colors.Violet && _charChecked == true)
                    {
                        foreach (Rectangle rect in _ocrDetectedRegionVM[colr])
                        {
                            RectItems.Add(new RectItemClass(colr, rect, Visibility.Visible));
                        }

                    }

                    //word
                    if (colr.Color == Colors.Yellow && _wordChecked == true)
                    {
                        foreach (Rectangle rect in _ocrDetectedRegionVM[colr])
                        {
                            RectItems.Add(new RectItemClass(colr, rect, Visibility.Visible));
                        }

                    }
                    //line
                    if (colr.Color == Colors.Green && _lineChecked == true)
                    {
                        foreach (Rectangle rect in _ocrDetectedRegionVM[colr])
                        {
                            RectItems.Add(new RectItemClass(colr, rect, Visibility.Visible));
                        }

                    }
                    //para
                    if (colr.Color == Colors.Red && _paraChecked == true)
                    {
                        foreach (Rectangle rect in _ocrDetectedRegionVM[colr])
                        {
                            RectItems.Add(new RectItemClass(colr, rect, Visibility.Visible));
                        }

                    }

                }
            }
        }

        /// <summary>
        /// This is specificed Command
        /// </summary>
        private void OpenImage()
        {
            //RectItems = new ObservableCollection<RectItemClass>();
            //RectItems.Add(new Rectangle(40, 40, 100, 50));
            //RectItems.Add(new Rectangle(40, 40, 30, 30));

            Console.WriteLine("Executing OpenImage");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a image for OCR";
            openFileDialog.Filter = "All supported graphics |*.jpg; *.jpeg;*.png|" +
                "JPEG(*.jpg;*.jpeg)|*.jpg; *.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                //Invisible detected reegion of previous image when new image was openned
                CharChecked = false; WordChecked = false;
                LineChecked = false; ParaChecked = false;

                string filename = openFileDialog.FileName;
                UpdateProcessedImage(filename);
                OutPutText = "Click RUN button to start OCR demo";

            }
        }

        private void UpdateProcessedImage(string filename)
        {
            var bitmap = new BitmapImage(new Uri(filename));

            ImageOne = new ImageClass
            {
                FilePath = filename,
                Image = bitmap
            };

            CurrentImage = ImageOne;

            ImageList.Add(CurrentImage);
            Debug.WriteLine("The lenghth of list of Image: " + ImageList.Count().ToString());

        }

    }
}
