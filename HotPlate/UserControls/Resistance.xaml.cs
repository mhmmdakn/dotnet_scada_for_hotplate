using System.Windows.Controls;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for Resistance.xaml
    /// </summary>
    public partial class Resistance : UserControl
    {


        public Resistance()
        {
            InitializeComponent();
            prog.Maximum = 0;
        }

        private int tolerans;

        public int Tolerans
        {
            get { return tolerans; }
            set
            {
                tolerans = value;
                prog.Minimum = -(set_value + tolerans + 60);
            }
        }

        private int set_value;

        public int Set_Value
        {
            get { return set_value; }
            set
            {
                set_value = value;
                prog.Minimum = -(set_value + tolerans + 60);

            }
        }

        private double _value;

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                prog.Value = -(_value);
            }
        }
    }
}
