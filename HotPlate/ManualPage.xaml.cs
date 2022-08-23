using System.Windows;
using System.Windows.Controls;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for ManualPage.xaml
    /// </summary>
    public partial class ManualPage : Page
    {
        public ManualPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Main.updatePage((int)Sayfalar.Manuel, this);
        }
    }
}
