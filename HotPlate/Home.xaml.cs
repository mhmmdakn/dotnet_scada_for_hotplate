using System.Windows;
using System.Windows.Controls;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Main.updatePage((int)Sayfalar.Referans, this);
        }


    }
}
