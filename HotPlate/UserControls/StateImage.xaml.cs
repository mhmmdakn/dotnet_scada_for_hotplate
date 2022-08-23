using Hot_Plate_Lib;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
namespace HotPlate
{
    /// <summary>
    /// Interaction logic for StateImage.xaml
    /// </summary>
    public partial class StateImage : PLC_BaseUserControl
    {
        public StateImage()
        {
            InitializeComponent();
        }

        private List<TextBlock> listText = new List<TextBlock>();

        public List<TextBlock> ListText
        {
            get { return listText; }
            set
            {
                listText = value;
                MultiState = multiState;

            }
        }
        private List<Image> listImage = new List<Image>();

        public List<Image> ListImage
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
                    img_state.Source = ListImage[multiState].Source;

                if (ListText.Count > multiState)
                {
                    txt_state.Text = ListText[multiState].Text;
                    txt_state.Width = ListText[multiState].Width;
                    txt_state.Height = ListText[multiState].Height;
                    txt_state.Foreground = ListText[multiState].Foreground;
                    txt_state.FontSize = ListText[multiState].FontSize;
                    txt_state.FontStyle = ListText[multiState].FontStyle;
                    txt_state.FontFamily = ListText[multiState].FontFamily;
                    txt_state.Margin = ListText[multiState].Margin;



                }


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
