using Hot_Plate_Lib;
using System.Windows;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for StateOk2.xaml
    /// </summary>
    public partial class StateOk2 : PLC_BaseUserControl
    {
        public StateOk2()
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
                    sta_state.Visibility = Visibility.Visible;
                }
                else
                {

                    sta_state.Visibility = Visibility.Hidden;


                }
            }

        }


    }
}
