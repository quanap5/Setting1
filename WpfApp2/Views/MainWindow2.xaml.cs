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
    /// Interaction logic for MainWindow2.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            //InitializeComponent();

            //this.DataContext = new OCRViewModel();

            //Rectangle rect;
            //rect = new Rectangle();
            //rect.Stroke = new SolidColorBrush(Colors.Black);
            ////rect.Fill = new SolidColorBrush(Colors.Black);
            //rect.Width = 28;
            //rect.Height = 31;
            //Canvas.SetLeft(rect, 75);
            //Canvas.SetTop(rect, 39);

            //Rectangle rect2;
            //rect2 = new Rectangle();
            //rect2.Stroke = new SolidColorBrush(Colors.Red);
            ////rect2.Fill = new SolidColorBrush(Colors.Black);
            //rect2.Width = 62;
            //rect2.Height = 33;
            //Canvas.SetLeft(rect2, 75);
            //Canvas.SetTop(rect2, 38);

            //Rectangle rect3;
            //rect3 = new Rectangle();
            //rect3.Stroke = new SolidColorBrush(Colors.GhostWhite);
            ////rect2.Fill = new SolidColorBrush(Colors.Black);
            //rect3.Width = 231;
            //rect3.Height = 36;
            //Canvas.SetLeft(rect3, 75);
            //Canvas.SetTop(rect3, 35);

            //Rectangle rect4;
            //rect4 = new Rectangle();
            //rect4.Stroke = new SolidColorBrush(Colors.Violet);
            ////rect2.Fill = new SolidColorBrush(Colors.Black);
            //rect4.Width = 244;
            //rect4.Height = 201;
            //Canvas.SetLeft(rect4, 72);
            //Canvas.SetTop(rect4, 35);

            //front_canvas1.Children.Add(rect);
            //front_canvas1.Children.Add(rect2);
            //front_canvas1.Children.Add(rect3);
            //front_canvas1.Children.Add(rect4);
        }


        private void About(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.xisom.com/en-us/php/home.php");
        }

    }
}
