using Hot_Plate_Lib;
using System;
using System.Collections.Generic;
using System.Windows.Media;
namespace HotPlate
{
    /// <summary>
    /// Interaction logic for StateLed.xaml
    /// </summary>
    public partial class StateLed_Multiple : PLC_BaseUserControl
    {
        public StateLed_Multiple()
        {
            InitializeComponent();
        }

        private object _value;

        public override object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                MultiState = Convert.ToInt32(_value);
            }
        }

        private Color ledRenk;

        public Color LedRenk
        {
            get { return ledRenk; }
            set
            {
                ledRenk = value;

            }
        }
        private List<Color> listColor = new List<Color>();

        public List<Color> ListColor
        {
            get { return listColor; }
            set
            {
                listColor = value;
                MultiState = state;
            }
        }



        private int state;

        public int MultiState
        {
            get { return state; }
            set
            {
                state = value;
                if (ListColor.Count > state)
                    state_color.Color = ListColor[state];
            }

        }
    }
}
