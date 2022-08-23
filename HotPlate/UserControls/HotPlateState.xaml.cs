using Hot_Plate_Lib;
using System.Windows;
using System.Windows.Controls;
namespace HotPlate
{
    /// <summary>
    /// Interaction logic for HotPlateState.xaml
    /// </summary>
    public partial class HotPlateState : PLC_BaseUserControl
    {
        public HotPlateState()
        {
            InitializeComponent();

        }

        private Image image1;

        public Image Image1
        {
            get { return image1; }
            set { image1 = value; }
        }

        private Image image2;

        public Image Image2
        {
            get { return image2; }
            set { image2 = value; }
        }

        private Image image3;

        public Image Image3
        {
            get { return image3; }
            set { image3 = value; }
        }

        private Image image4;

        public Image Image4
        {
            get { return image4; }
            set { image4 = value; }
        }

        private object _value = 1;

        //public object Value
        //{
        //    get { return _value; }
        //    set
        //    {
        //        _value = value;
        //        State = (int)(_value);
        //    }
        //}

        private int state = 1;

        public int State
        {
            get { return state; }
            set
            {
                state = value;
                if (state == 1 && image1 != null)
                {
                    ImgHotPlate.Source = image1.Source;
                }
                else if (state == 2 && image2 != null)
                {
                    ImgHotPlate.Source = image2.Source;
                }
                else if (state == 3 && image3 != null)
                {
                    ImgHotPlate.Source = Image3.Source;
                }
                else if (state == 4 && image4 != null)
                {
                    ImgHotPlate.Source = Image4.Source;
                }
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            State = 1;

        }
    }
}
