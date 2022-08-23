using Hot_Plate_Lib;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for GirisUnitesi.xaml
    /// </summary>
    public partial class GirisUnitesi : Page
    {
        public GirisUnitesi()
        {
            InitializeComponent();
        }

        private void BtnBeforePage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.frame.Content = MainWindow.servo;
        }

        private void BtnAfterPage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.frame.Content = MainWindow.cikisUnitesi;

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Main.updatePage((int)Sayfalar.Giris, this);
        }

        private void degerGir_Dok_Click(object sender, EventArgs e)
        {
            Main.PLCs[(int)StationName.AnaUnite].Write("Kaydet_Giris", true, TypeCode.Boolean);
        }
    }
}
