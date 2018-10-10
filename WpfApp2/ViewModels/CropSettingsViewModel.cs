using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp2.Commands;
using WpfApp2.Models;

namespace WpfApp2.ViewModels
{
    public class CropSettingsViewModel: ViewModelBase
    {
       
        public OCRViewModel oCr;
        public ICommand ApplyCropSettingsCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand SaveEditCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        //private ItemHandler _itemHandler;

        private string _cropID;
        private int _cropXPosition;
        private int _cropYPosition;
        private int _cropWidth;
        private int _cropHeight;
        

        private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get
            {
               
                //return _items;
                return oCr._curCropSetting=_items;
            }
            set
            {
                _items = value;
                oCr._curCropSetting = value;
                //oCr._curCropSetting = value;
                OnPropertyChanged("Items");
    
            }
        }

        private ObservableCollection<Item> TranferOrc2_item()
        {
            var temp = new ObservableCollection<Item>();
            foreach (var item in oCr.CropRectSettings)
            {
                temp.Add(new Item("Region " + (_items.Count() + 1).ToString(), item.Rect.X, item.Rect.Y, item.Rect.Width, item.Rect.Height));

            }
            return temp;
        }

        private Item _selectedItem;
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value !=null)
                {
                    _selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                    Console.WriteLine("click" + _selectedItem.Name);
                    CropXPosition = _selectedItem.x1;
                    CropYPosition = _selectedItem.x2;
                    CropWidth = _selectedItem.x3;
                    CropHeight = _selectedItem.x4;
                    CropID = _selectedItem.Name;

                }
                
            }
        }


        public string CropID
        {
            get { return _cropID; }
            set
            {
                _cropID = value;
                OnPropertyChanged("CropID");
            }
        }

        public int CropXPosition
        {
            get { return _cropXPosition; }
            set
            {
                _cropXPosition = value;
                OnPropertyChanged("CropXPosition");
                RealTimeUpdate();
            }
        }
        public int CropYPosition
        {
            get { return _cropYPosition; }
            set
            {
                _cropYPosition = value;
                OnPropertyChanged("CropYPosition");
                RealTimeUpdate();
            }
        }
        public int CropWidth
        {
            get { return _cropWidth; }
            set
            {
                _cropWidth = value;
                OnPropertyChanged("CropWidth");
                RealTimeUpdate();
            }
        }
        public int CropHeight
        {
            get { return _cropHeight; }
            set
            {
                _cropHeight = value;
                OnPropertyChanged("CropHeight");
                RealTimeUpdate();
            }
        }

        public CropSettingsViewModel(OCRViewModel ocr)
        {
            
            oCr = ocr;
            //_cropXPosition = ocr.CropRectSettings[0].Rect.X;
            //_cropYPosition = ocr.CropRectSettings[0].Rect.Y;
            //_cropWidth = ocr.CropRectSettings[0].Rect.Width;
            //_cropHeight = ocr.CropRectSettings[0].Rect.Height;
            ApplyCropSettingsCommand = new RelayCommand(ApplyCropSettings);
            RemoveCommand = new RelayCommand(Remove);
            AddCommand = new RelayCommand(Add);
            SaveEditCommand = new RelayCommand(SaveEdit);
            CloseCommand = new RelayCommand(CloseSetting);

            Items = new ObservableCollection<Item>();
            foreach (var item in oCr.CropRectSettings)
            {
                _items.Add(new Item("Region " + (_items.Count()+1).ToString(), item.Rect.X, item.Rect.Y, item.Rect.Width, item.Rect.Height));

            }

        }

        private void CloseSetting()
        {
            Environment.Exit(0);
        }

        private void SaveEdit()
        {
            if ((_items.FirstOrDefault(c => c.Name == _cropID) == null && _selectedItem !=null ) || (_selectedItem != null && _cropID == SelectedItem.Name ))
            {
                var editedItem = new Item(_cropID, _cropXPosition, _cropYPosition, _cropWidth,
                _cropHeight);
                var found = _items.FirstOrDefault(i => i.Name == _selectedItem.Name);
                if (found != null)
                {
                    int it = _items.IndexOf(found);
                    _items[it] = editedItem;
                }
                else
                {
                    Debug.WriteLine("hien tai KO co item click");
                }

            }
            else
            {
                MessageBox.Show("Name is not available or you have to add first");
            }
            

        }

        public void Add()
        {
            if (_cropID!=null)
            {
                if (_items.FirstOrDefault(c => c.Name == _cropID) == null)
                {
                    //_items.Add(new Item("Region " + (_items.Count()+1).ToString(), 20, 20, 20, 20));
                    _items.Add(new Item(_cropID, _cropXPosition, _cropYPosition, _cropWidth,_cropHeight));
                    return;
                }
                MessageBox.Show("Add different name");
                
            }
            else
            {
                MessageBox.Show("Add name for your initial region");
            }
            

        }

        private void Remove()
        {
            Debug.WriteLine("chuan bi xoa "+ _cropID);
            Debug.WriteLine("Chieu dai list hien tai: " + _items.Count().ToString() );
            var itemwillRemove = _items.SingleOrDefault(r => r.Name == _cropID);
            if (itemwillRemove != null)
            {
               
                _items.Remove(itemwillRemove);
                Debug.WriteLine("Xoa " + _cropID +"thanh cong");
                Debug.WriteLine("Chieu dai list sau khi xoa:" + _items.Count().ToString());

            }
            else
            {
                Debug.WriteLine("Khong ton tai " + _cropID + "trong list");
            }
        }

        //public ObservableCollection<Item> Items
        //{
        //    get { return _itemHandler.Items; }
        //}

        private void ApplyCropSettings()
        {
            //oCr.CropRectSettings[0].Rect.X = CropXPosition;
            //oCr.CropRectSettings[0].Rect.Y = CropYPosition;
            //ocr.CropRectSettings[0].Rect.Width = CropWidth;
            //ocr.CropRectSettings[0].Rect.Height = CropHeight;
            int index = 0;
            oCr.CropRectSettings = new ObservableCollection<RectItemClass>();
            foreach (var item in _items)
            {
                SolidColorBrush color;
                if (index % 3 == 0) color = new SolidColorBrush(Colors.Red);
                else if (index % 3 == 1) color = new SolidColorBrush(Colors.Green);
                else color = new SolidColorBrush(Colors.Yellow);
                index=index +1;
                oCr.CropRectSettings.Add(new RectItemClass(color, new Rectangle(item.x1, item.x2, item.x3, item.x4), Visibility.Visible));
                //oCr.CropRectSettings.Add(new RectItemClass(color, new Rectangle(item.x1*2, item.x2*2, item.x3-10, item.x4-10), Visibility.Visible));
                Console.WriteLine("da cai dat may hinh crop: " +oCr.CropRectSettings.Count().ToString());
                
            }

            //Debug.WriteLine("Setting parametters: ");
            //Debug.WriteLine("           " + _cropXPosition.ToString());
            //Debug.WriteLine("           " + _cropYPosition.ToString());
            //Debug.WriteLine("           " + _cropWidth.ToString());
            //Debug.WriteLine("           " + _cropHeight.ToString());

        }

        public void RealTimeUpdate()
        {
            oCr.CropRectSettings = new ObservableCollection<RectItemClass>();
            oCr.CropRectSettings.Add(new RectItemClass(new SolidColorBrush(Colors.Green), new Rectangle(CropXPosition, CropYPosition, CropWidth, CropHeight), Visibility.Visible));
            //oCr.CropRectSettings.Add(new RectItemClass(new SolidColorBrush(Colors.Green), new Rectangle(CropXPosition-5, CropYPosition -5, CropWidth +10, CropHeight+10), Visibility.Visible));
        }



        /////////////////////////////////////
        ///
        public class Item
        {
            public string Name { get; set; }
            public int x1 { get; set; }
            public int x2 { get; set; }
            public int x3 { get; set; }
            public int x4 { get; set; }
            public Item(string name, int x1, int x2, int x3, int x4)
            {
                Name = name;
                this.x1 = x1;
                this.x2 = x2;
                this.x3 = x3;
                this.x4 = x4; ;
            }
        }

        //public class ItemHandler
        //{
        //    public ItemHandler()
        //    {
        //        Items = new ObservableCollection<Item>();
        //    }

        //    public ObservableCollection<Item> Items { get; private set; }
        //    public void Add(Item item)
        //    {
        //        Items.Add(item);
        //    }
        //}
    }
}
