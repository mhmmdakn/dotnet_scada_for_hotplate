using Hot_Plate_Lib;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for BtnYuvarlak.xaml
    /// </summary>
    public partial class BtnPush : PLC_BaseUserControl
    {
        public BtnPush()
        {
            InitializeComponent();
        }



        private Color trueColor;

        public Color TrueColor
        {
            get { return trueColor; }
            set
            {
                trueColor = value;
                light_Color.Color = trueColor;
            }
        }

        private Color falseColor;

        public Color FalseColor
        {
            get { return falseColor; }
            set
            {
                falseColor = value;
                dark_Color.Color = falseColor;
            }
        }


        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                txt_Text.Text = text;
            }
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

        private bool state;

        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                if (state)
                {
                    BtnStateYuvarlak.Visibility = Visibility.Visible;
                    txt_Text.Visibility = Visibility.Hidden;
                }
                else
                {

                    BtnStateYuvarlak.Visibility = Visibility.Hidden;
                    txt_Text.Visibility = Visibility.Visible;

                }
            }
        }

        private void Viewbox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Main.PLCs[(int)PLCStationName].Write(PLCWriteAdress, false, DataType);

        }

        private void Viewbox_MouseLeave(object sender, MouseEventArgs e)
        {
            //  Main.PLCs[(int)PLCStationName].Write_Async(PLCWriteAdress, false);

        }

        private void Viewbox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.PLCs[(int)PLCStationName].Write(PLCWriteAdress, true, DataType);

        }
    }
}
