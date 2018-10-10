using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp2.Models
{
    public interface ITesseractOCR
    {
        string OneImageOCR(ImageClass one);
        List<string> ListImageOCR(List<ImageClass> list);

        String getTime();
        Dictionary<SolidColorBrush, List<Rectangle>> GetocrDetectedRegion();

        void SelectLang(string selectedLang);
    }
}
