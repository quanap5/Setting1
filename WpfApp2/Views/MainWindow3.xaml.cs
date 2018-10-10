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
using System.Windows.Shapes;
using WpfApp2.ViewModels;

namespace WpfApp2.Views
{
    /// <summary>
    /// Interaction logic for MainWindow3.xaml
    /// </summary>
    public partial class MainWindow3 : Window
    {
        public Boolean isDragging = false;
        Point anchorPoint = new Point();
        Rectangle selectionRectangle;
        public MainWindow3()
        {
            InitializeComponent();
            this.DataContext = new OCRViewModel();
            //this.DataContext = new MouseViewModel();
           

        }
        private void About(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.xisom.com/en-us/php/home.php");
        }

        //private void LoadedImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (isDragging == false)
        //    {
        //        anchorPoint.X = e.GetPosition(quanGrid).X;
        //        anchorPoint.Y = e.GetPosition(quanGrid).Y;
        //        Canvas.SetZIndex(selectionRectangle, quanGrid.Children.Count);
        //        isDragging = true;
        //        quanGrid.Cursor = Cursors.Cross;
        //        Console.WriteLine("This is mouse down"+ e.GetPosition(quanGrid).X+"-----" +e.GetPosition(quanGrid).Y);
        //    }

        //}
        //private void LoadedImage_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (isDragging)
        //    {
        //        double x = e.GetPosition(quanGrid).X;
        //        double y = e.GetPosition(quanGrid).Y;
        //        selectionRectangle.SetValue(Canvas.LeftProperty, Math.Min(x, anchorPoint.X));
        //        selectionRectangle.SetValue(Canvas.TopProperty, Math.Min(y, anchorPoint.Y));
        //        selectionRectangle.Width = Math.Abs(x - anchorPoint.X);
        //        selectionRectangle.Height = Math.Abs(y - anchorPoint.Y);

        //        if (selectionRectangle.Visibility != Visibility.Visible)
        //            selectionRectangle.Visibility = Visibility.Visible;
        //    }
        //}

        //private void LoadedImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (isDragging)
        //    {
        //        isDragging = false;
        //        if (selectionRectangle.Width > 0)
        //        {
        //           // Crop.IsEnabled = true;
        //           // Cut.IsEnabled = true;
        //            quanGrid.Cursor = Cursors.Arrow;
        //        }
        //    }
        //}
    }
}
