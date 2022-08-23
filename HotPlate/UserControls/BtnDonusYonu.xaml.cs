using Hot_Plate_Lib;
using System.Windows;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for BtnDonusYonu.xaml
    /// </summary>
    public partial class BtnDonusYonu : PLC_BaseUserControl
    {
        public BtnDonusYonu()
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

        private bool state;

        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                if (state)
                {
                    btn_state.Visibility = Visibility.Visible;

                }
                else
                {

                    btn_state.Visibility = Visibility.Hidden;


                }
            }
        }
    }
}
