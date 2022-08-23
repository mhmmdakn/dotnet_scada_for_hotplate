using Hot_Plate_Lib;
using System.Windows.Media;
namespace HotPlate
{
    /// <summary>
    /// Interaction logic for StateLed.xaml
    /// </summary>
    public partial class StateLed : PLC_BaseUserControl
    {
        public StateLed()
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
                State = (bool)value;
            }
        }


        private Color trueColor;

        public Color TrueColor
        {
            get { return trueColor; }
            set
            {
                trueColor = value;

            }
        }

        private Color falseColor;

        public Color FalseColor
        {
            get { return falseColor; }
            set
            {
                falseColor = value;

            }
        }

        private bool state;

        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                if (state)
                {
                    state_color.Color = trueColor;
                }
                else
                {

                    state_color.Color = falseColor;
                }
            }

        }


    }
}
