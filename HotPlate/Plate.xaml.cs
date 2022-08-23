using Hot_Plate_Lib;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for LineerCetvel.xaml
    /// </summary>
    public partial class Plate : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        public Plate()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
        }

        private void BtnBeforePage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.frame.Content = MainWindow.auto;
        }

        private void BtnAfterPage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.frame.Content = MainWindow.servo;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Main.updatePage((int)Sayfalar.Plate, this);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Main.PLCs[(int)StationName.AnaUnite].Write(txt_Baski_Sayisi.PLCWriteAdress, 0, TypeCode.UInt16);

        }
        private void degerGir_Baski_Offset_Click(object sender, EventArgs e)
        {
            Main.PLCs[(int)StationName.AnaUnite].Write("SAYKIL_BITIR", true, TypeCode.Boolean);
        }
        private void txt_Baski_Sayisi_MouseLeave(object sender, MouseEventArgs e)
        {
            timer.Stop();
        }

        private void txt_Baski_Sayisi_MouseDown(object sender, MouseButtonEventArgs e)
        {
            timer.Start();
        }

        private void txt_Baski_Sayisi_MouseUp(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
        }
        private void DegerGir_Text_Baski_Suresi_Click(object sender, EventArgs e)
        {
            Main.PLCs[(int)StationName.AnaUnite].Write("SAYKIL_BITIR", true, TypeCode.Boolean);
        }


        private void DegerGir_Digital_Baski_Suresi_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DegerGir_Text_Baski_Suresi.PerformClick();
        }


    }
}
