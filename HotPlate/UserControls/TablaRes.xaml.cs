using Hot_Plate_Lib;
using System.Windows.Controls;
namespace HotPlate
{
    /// <summary>
    /// Interaction logic for TablaRes.xaml
    /// </summary>
    public partial class TablaRes : UserControl
    {

        private Tool tm2;
        public TablaRes()
        {
            InitializeComponent();




        }



        public Tabla Tabla { get; set; } = Tabla.Alt_Tabla;












        private double _value;

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;

            }
        }


    }


}
