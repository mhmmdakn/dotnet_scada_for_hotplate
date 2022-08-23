using Hot_Plate_Lib;
using System.Windows;
using System.Windows.Input;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for BtnYön.xaml
    /// </summary>
    public partial class BtnYon : PLC_BaseUserControl
    {
        public BtnYon()
        {
            InitializeComponent();
        }

        bool down = false;

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
                    BtnStateYukari1.Visibility = Visibility.Visible;
                    BtnStateYukari2.Visibility = Visibility.Visible;
                    BtnStateYukari3.Visibility = Visibility.Visible;

                }
                else
                {

                    BtnStateYukari1.Visibility = Visibility.Hidden;
                    BtnStateYukari2.Visibility = Visibility.Hidden;
                    BtnStateYukari3.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Viewbox_MouseUp(object sender, MouseButtonEventArgs e)
        {

            Main.PLCs[(int)PLCStationName].Write(PLCWriteAdress, false, DataType);
            down = false;

        }

        private void Viewbox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (down)
            {
                Main.PLCs[(int)PLCStationName].Write(PLCWriteAdress, false, DataType);
                down = false;
            }
        }

        private void Viewbox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.PLCs[(int)PLCStationName].Write(PLCWriteAdress, true, DataType);
            down = true;
        }


    }
}
