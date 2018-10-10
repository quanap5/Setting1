using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Tesseract;
using System.Drawing;
using System.Diagnostics;
using WpfApp2.ViewModels;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //protected string img_Src;
        //protected TesseractEngine ocr;
        public MainWindow()
        {
            InitializeComponent();
           // this.DataContext = new OCRViewModel();
        }



        //private void mnuNew_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("New");
        //}

        //private void mnuOpen_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Title = "Select a image for OCR";
        //    openFileDialog.Filter = "All supported graphics |*.jpg; *.jpeg;*.png|" +
        //        "JPEG(*.jpg;*.jpeg)|*.jpg; *.jpeg|" +
        //        "Portable Network Graphic (*.png)|*.png";
        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        inputImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
        //        //textEditor.Text = openFileDialog.FileName;
        //        textEditor.Text = "Click Run button to see the results";
        //        img_Src = openFileDialog.FileName;
        //        ocr = new TesseractEngine("./tessdata", "eng", EngineMode.TesseractAndCube);
        //        //btnRun.IsEnabled = true;
        //        toolRun.IsEnabled = true;

        //        //var img = new Bitmap(openFileDialog.FileName);
        //        //var ocr = new TesseractEngine("./tessdata", "eng" , EngineMode.Default);
        //        //var page = ocr.Process(img);
        //        //textEditor.Text = page.GetText();


        //    }
        //}

        //private void runOCR(object sender, RoutedEventArgs e)
        //{
        //    btnRun.IsEnabled = false;
        //    toolRun.IsEnabled = false;
        //    textEditor.Text = "The OCR is running ...";

        //    Stopwatch stopW = Stopwatch.StartNew();
        //    var img = new Bitmap(img_Src);
        //    var page = ocr.Process(img);
        //    textEditor.Text = page.GetText();
        //    stopW.Stop();
        //    textEditorTime.Text = stopW.Elapsed.TotalMilliseconds.ToString() + " ms for Running";

        //}

        //private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = true;
        //}

        //private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        //{
        //    //txtEditor.Text = "";
        //}

        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        //private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //Color selectionColor = (Color)(cm)
        //    // this.Background = Brushes.LightPink;
        //}

        private void About(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.xisom.com/en-us/php/home.php");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
