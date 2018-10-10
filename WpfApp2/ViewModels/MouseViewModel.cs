using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp2.Commands;

namespace WpfApp2.ViewModels
{
    public class MouseViewModel: ViewModelBase
    {
        private bool isDragging = false;
        private Point anchorPoint = new Point();
        public ICommand MouseLeftButtonDownCommand { get; set; }

        

        public MouseViewModel()
        {
            MouseLeftButtonDownCommand = new RelayCommand(MouseLeftButtonDown);
        }

        private void MouseLeftButtonDown()
        {
            if (isDragging == false)
            {
                Console.WriteLine("This is mouse down");
                //isDragging = true;

            }
        }
    }
}
