using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp2
{
    public class ImageClass
    {
        
        public string FilePath { get; set; }

        public BitmapImage Image { get; set; }


        public ImageClass()
        {
            Image = new BitmapImage();
        }
    }
}
