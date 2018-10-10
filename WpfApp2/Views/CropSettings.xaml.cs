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
    /// Interaction logic for CropSettings.xaml
    /// </summary>
    public partial class CropSettings : Window
    {
        public CropSettings(ViewModels.OCRViewModel oCRViewModel)
        {
            InitializeComponent();
             this.DataContext = new CropSettingsViewModel(oCRViewModel);
        }
    }
}
