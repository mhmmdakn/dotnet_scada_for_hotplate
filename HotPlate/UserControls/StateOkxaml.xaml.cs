using Hot_Plate_Lib;
using System.Windows.Media;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for StateOkxaml.xaml
    /// </summary>
    public partial class StateOkxaml : PLC_BaseUserControl
    {
        public StateOkxaml()
        {
            InitializeComponent();
        }

        private Color colorTrue;

        public Color ColorTrue
        {
            get { return colorTrue; }
            set
            {
                colorTrue = value;
                ok_renk1.Color = colorTrue;
                ok_renk2.Color = colorTrue;
            }
        }

        private Color colorFalse;

        public Color ColorFalse
        {
            get { return colorFalse; }
            set
            {
                colorFalse = value;
                ok_renk1.Color = colorFalse;
                ok_renk2.Color = colorFalse;
            }
        }

    }
}
