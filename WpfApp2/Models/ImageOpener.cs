using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp2
{
    class ImageOpener
    {
        public List<ImageClass> OpenedImage(List<string> pths)
        {
            var imagesList = new List<ImageClass>();

            foreach (var item in pths)
            {
                try
                {
                    var uri = new Uri(item);
                    var bitmap = new BitmapImage(uri);
                    var tem_imClass = new ImageClass{
                    FilePath = item,
                    Image = bitmap};

                    imagesList.Add(tem_imClass);


                }
                catch (Exception)
                {

                    throw;
                }

            }

            return imagesList;

        }
    }
}
