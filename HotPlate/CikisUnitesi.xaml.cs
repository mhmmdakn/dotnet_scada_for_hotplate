using Hot_Plate_Lib;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for CikisUnitesi.xaml
    /// </summary>
    public partial class CikisUnitesi : Page
    {
        public CikisUnitesi()
        {
            InitializeComponent();
        }

        private void BtnBeforePage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.frame.Content = MainWindow.girisUnitesi;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Main.updatePage((int)Sayfalar.Cikis, this);
        }

        private void degerGir_Menzel_Click(object sender, EventArgs e)
        {
            Main.PLCs[(int)StationName.AnaUnite].Write("Kaydet_Cikis", true, TypeCode.Boolean);
        }
    }
}
