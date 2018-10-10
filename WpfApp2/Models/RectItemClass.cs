using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApp2.Models
{
    public class RectItemClass
    {
        public SolidColorBrush Colr { get; set; }

        public Rectangle Rect { get; set; }

        public Visibility Visible { get; set; }

        public RectItemClass(SolidColorBrush colr, Rectangle a)
        {
            Rect = a;
            Colr = colr;
        }
        public RectItemClass(SolidColorBrush colr, Rectangle a, Visibility vis)
        {
            //Rect = new Rectangle((int)Math.Floor(a.X * 2.193548), (int)Math.Floor(a.Y * 2.193548), (int)Math.Floor(a.Width*2.193548), (int)Math.Floor(a.Height * 2.193548));
            Rect = a;
            Colr = colr;
            Visible = vis;
        }

    }

}
