using FontAwesome.WPF;
using Hot_Plate_Lib;
using System;
using System.Collections.Generic;
using System.Windows.Input;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for BtnImages.xaml
    /// </summary>
    public partial class BtnAwesome : PLC_BaseUserControl
    {
        public BtnAwesome()
        {
            InitializeComponent();

        }



        private List<string> listText = new List<string>();

        public List<string> ListText
        {
            get { return listText; }
            set
            {
                listText = value;
                MultiState = multiState;

            }
        }
        private List<ImageAwesome> listImage = new List<ImageAwesome>();

        public List<ImageAwesome> ListImage
        {
            get { return listImage; }
            set
            {
                listImage = value;
                MultiState = multiState;
            }
        }



        private int multiState = 0;

        public int MultiState
        {
            get { return multiState; }
            set
            {
                multiState = value;
                if (ListImage.Count > multiState)
                {
                    sta_awesome.Icon = ListImage[multiState].Icon;
                    sta_awesome.Foreground = ListImage[multiState].Foreground;
                    sta_awesome.Spin = ListImage[multiState].Spin;
                    sta_awesome.SpinDuration = ListImage[multiState].SpinDuration;
                    sta_awesome.Opacity = ListImage[multiState].Opacity;
                }


                if (ListText.Count > multiState)
                    txt_state.Text = ListText[multiState];
            }

        }

        private object _value;

        public override object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                MultiState = Convert.ToInt16(value);

            }
        }



        private bool reverse;

        public bool Reverse
        {
            get { return reverse; }
            set { reverse = value; }
        }




        private void img_button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (multiState == 0)
                Main.PLCs[(int)PLCStationName].Write(PLCWriteAdress, true, DataType);
            else
                Main.PLCs[(int)PLCStationName].Write(PLCWriteAdress, false, DataType);

        }


    }
}
