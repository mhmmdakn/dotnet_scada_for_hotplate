using Hot_Plate_Lib;
using System;
using System.Windows.Controls;
using System.Windows.Input;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for BtnImages.xaml
    /// </summary>
    public partial class BtnImages : PLC_BaseUserControl
    {
        public BtnImages()
        {
            InitializeComponent();

        }

        private Image image1 = new Image();

        public Image Image1
        {
            get { return image1; }
            set { image1 = value; }
        }

        private Image image2 = new Image();

        public Image Image2
        {
            get { return image2; }
            set { image2 = value; }
        }

        private object _value;

        public override object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                State = Convert.ToBoolean(value);

            }
        }

        private bool reverse;

        public bool Reverse
        {
            get { return reverse; }
            set { reverse = value; }
        }


        private bool state;

        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                if (state != reverse)
                {
                    img_button.Source = image2.Source;
                }
                else
                {
                    img_button.Source = image1.Source;
                }
            }
        }

        private void img_button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Main.PLCs[(int)PLCStationName].Write(PLCWriteAdress, !state, DataType);
        }


    }
}
