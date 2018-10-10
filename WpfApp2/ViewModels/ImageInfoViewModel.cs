using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.ViewModels
{
    public class ImageInfoViewModel: ViewModelBase
    {
        public class ImageInfoClass
        {
            public string Name { get; set; }
            public string Extension { get; set; }
            public string Directory { get; set; }
            public string Dimension { get; set; }
            public string Size { get; set; }
            public string CreateOn{ get; set; }

            public float GlobalScale { get; set; }
            

            public ImageInfoClass()
            {

            }

        }

        //private OCRViewModel ocrVM { get; set; }
        public ImageClass _imgHandler;
        public ImageClass ImgHandler
        {
            get
            {
                return _imgHandler;
            }
            set
            {
                _imgHandler = value;
                OnPropertyChanged("ImgHandler");
            }
        }

        public ImageInfoClass _imgInfoClass;
        public ImageInfoClass ImgInfoClass
        {
            get { return _imgInfoClass; }
            set
            {
                _imgInfoClass = value;
                OnPropertyChanged("ImgInfoClass");
                Console.WriteLine("Image Name is: "+ _imgInfoClass.Name);

            }
        }

        public ImageInfoViewModel()
        {
            
            Console.WriteLine("Jump to constructor 1");
            
            ImgInfoClass = LoadInfo2(_imgHandler);
        }

        private ImageInfoClass LoadInfo2(ImageClass im)
        {
            ImageInfoClass info = new ImageInfoClass();
            info.Name = "TEST NAME";
            info.Extension = "TEST NAME2";
            
            info.Directory = "TEST NAME3";

            info.Dimension = "TEST NAME4";
            info.Size = "TEST NAME5";
            info.CreateOn = "TEST NAME6";
            return info;

        }

        public ImageInfoViewModel(ImageClass im)
        {
            Console.WriteLine("Jump to constructor 2");
            ImgHandler = im;
            ImgInfoClass = LoadInfo(_imgHandler);
        }
        public ImageInfoClass LoadInfo(ImageClass imgHandler)
        {
            ImageInfoClass info = new ImageInfoClass();
            FileInfo fileInfo = new FileInfo(imgHandler.FilePath);
            info.Name = fileInfo.Name.Replace(fileInfo.Extension, "");
            info.Extension = fileInfo.Extension;
            string _label13_ori = fileInfo.DirectoryName;
            if (_label13_ori.Length > 50)
                _label13_ori = _label13_ori.Substring(0, 10) + "..." +
                    _label13_ori.Substring(_label13_ori.LastIndexOf("\\"));
            info.Directory = _label13_ori;

            info.Dimension = (int)imgHandler.Image.Width + "x" + (int)imgHandler.Image.Height;
            info.Size = (fileInfo.Length / 1024.0).ToString("0.0") + "KB";
            info.CreateOn = fileInfo.CreationTime.ToString("dddd MMMM, yyyy");
            if ( imgHandler.Image.Width > imgHandler.Image.Height)
            {
                info.GlobalScale = 340 / (float)imgHandler.Image.Height;

            }
            else
            {
                info.GlobalScale = 340 / (float)imgHandler.Image.Width;
            }
            
            return info;
        }
    }
}
