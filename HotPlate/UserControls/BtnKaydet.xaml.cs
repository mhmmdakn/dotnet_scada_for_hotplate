using System.Windows.Controls;
using System.Windows.Media;

namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for BtnKaydet.xaml
    /// </summary>
    public partial class BtnKaydet : UserControl
    {
        public BtnKaydet()
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
                color1.Fill = iconBackground;
                color2.Fill = iconBackground;
                color3.Stroke = iconBackground;
                color4.Fill = iconBackground;
                color5.Fill = iconBackground;
                color6.Fill = iconBackground;
            }
        }

    }
}
