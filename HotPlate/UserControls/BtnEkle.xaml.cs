using System.Windows.Controls;
using System.Windows.Media;

namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for BtnEkle.xaml
    /// </summary>
    public partial class BtnEkle : UserControl
    {
        public BtnEkle()
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
                background2.Stroke = iconBackground;
                background3.Stroke = iconBackground;
                background4.Foreground = iconBackground;
            }
        }

    }
}
