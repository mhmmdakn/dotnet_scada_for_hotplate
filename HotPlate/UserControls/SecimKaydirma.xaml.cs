using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for SecimKaydirma.xaml
    /// </summary>
    public partial class SecimKaydirma : UserControl
    {
        public SecimKaydirma()
        {
            InitializeComponent();
        }

        SolidColorBrush solidColor = new SolidColorBrush();

        private bool secim;

        public bool Secim
        {
            get { return secim; }
            set
            {
                secim = value;
                if (secim)
                {
                    solidColor.Color = Colors.Lime;
                    kaydirma.Fill = solidColor;
                    kaydirma2.Visibility = Visibility.Hidden;
                    kaydirma3.Visibility = Visibility.Visible;
                }
                else if (!secim)
                {
                    solidColor.Color = Colors.LightGray;
                    kaydirma.Fill = solidColor;
                    kaydirma2.Visibility = Visibility.Visible;
                    kaydirma3.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            secim = true;
        }
    }
}
