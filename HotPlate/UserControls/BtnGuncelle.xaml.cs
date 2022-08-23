using System.Windows.Controls;
using System.Windows.Media;

namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for BtnGuncelle.xaml
    /// </summary>
    public partial class BtnGuncelle : UserControl
    {
        public BtnGuncelle()
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
