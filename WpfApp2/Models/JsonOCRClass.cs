using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static WpfApp2.ViewModels.ImageInfoViewModel;

namespace WpfApp2.Models
{
    class JsonOCRClass
    {
        public ImageClass jsonInput { get; set; }
        public ImageInfoClass jsonInfo { get; set; }
        public Dictionary<SolidColorBrush, List<Rectangle>> jsonOutputRegion { get; set; }

        public JsonOCRClass()
        {

        }

        public JsonOCRClass(ImageClass inp, ImageInfoClass inf, Dictionary<SolidColorBrush, List<Rectangle>> reg)
        {
            jsonInput = inp;
            jsonInfo = inf;
            jsonOutputRegion = reg;

        }



    }
}
