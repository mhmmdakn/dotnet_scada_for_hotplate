using Hot_Plate_Lib;
using System.Windows;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for LoadingGif.xaml
    /// </summary>
    public partial class LoadingGif : PLC_BaseUserControl
    {
        public LoadingGif()
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
                State = (bool)_value;
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

                    usercontrol.Visibility = Visibility.Visible;
                }
                else
                {

                    usercontrol.Visibility = Visibility.Hidden;
                }
            }
        }


    }
}
