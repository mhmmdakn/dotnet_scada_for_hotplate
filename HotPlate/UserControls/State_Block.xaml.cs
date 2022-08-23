using Hot_Plate_Lib;
using System;
using System.Collections.Generic;
using System.Windows.Media;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for State_Block.xaml
    /// </summary>
    public partial class State_Block : PLC_BaseUserControl
    {
        public State_Block()
        {
            InitializeComponent();
        }
        private int width;

        public int TextWidth
        {
            get { return width; }
            set
            {
                width = value;
                txt_block.Width = width;
            }
        }
        private int height;

        public int TextHeight
        {
            get { return height; }
            set
            {
                height = value;
                txt_block.Height = height;
            }
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
        private List<Color> listColor = new List<Color>();

        public List<Color> ListColor
        {
            get { return listColor; }
            set
            {
                listColor = value;
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
                if (ListColor.Count > multiState)
                    txt_block.Background = new SolidColorBrush(ListColor[multiState]);

                if (ListText.Count > multiState)
                    txt_block.Text = ListText[multiState];
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
