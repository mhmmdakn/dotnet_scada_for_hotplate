using System.Windows.Controls;
using System.Windows.Media;

namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for BtnIptal.xaml
    /// </summary>
    public partial class BtnIptal : UserControl
    {
        public BtnIptal()
        {
            InitializeComponent();
        }

        private Brush iconBackground;

        public Brush IconBackground
        {
            get { return iconBackground; }
            set
            {
                iconBackground = value;
                background.Fill = iconBackground;
            }
        }


    }
}
