using FontAwesome.WPF;
using Hot_Plate_Lib;
using System;
using System.Collections.Generic;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for State_Awasome.xaml
    /// </summary>
    public partial class State_Awasome : PLC_BaseUserControl
    {
        public State_Awasome()
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
    }
}
