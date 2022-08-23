using Hot_Plate_Lib;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
namespace HotPlate
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        public MainPage()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;

        }

        private void MainPage_AlarmStateChanged(object sender, EventArgs e)
        {
            sta_Alarm.State = ((S71200_Main)sender).AlarmState;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Main.PLCs[(int)StationName.AnaUnite].Write(txt_Baski_Sayisi.PLCWriteAdress, 0, TypeCode.UInt16);

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

        private void btn_Otomatik_Click(object sender, RoutedEventArgs e)
        {

            Main.PLCs[(int)StationName.AnaUnite].Write("Giden_Veri.Work_Mode", 2, TypeCode.UInt16);
        }

        private void btn_Manuel_Click(object sender, RoutedEventArgs e)
        {
            if (Main.WorkMode == 2)
            {


                MessageBoxResult messageBoxResult = MessageBox.Show("Dikkat Otomatik Moddan Çıkmak Üzeresiniz!!! Devam etmek istiyor musunuz?", "Uyarı", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.No)
                    return;
            }
            Main.PLCs[(int)StationName.AnaUnite].Write("Giden_Veri.Work_Mode", 3, TypeCode.UInt16);

        }

        private void btn_Referans_Click(object sender, RoutedEventArgs e)
        {
            if (Main.WorkMode == 2)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Dikkat Otomatik Moddan Çıkmak Üzeresiniz!!! Devam etmek istiyor musunuz?", "Uyarı", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.No)
                    return;
            }
            Main.PLCs[(int)StationName.AnaUnite].Write("Giden_Veri.Work_Mode", 1, TypeCode.UInt16);

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ((S71200_Main)Main.PLCs[(int)StationName.AnaUnite]).AlarmStateChanged += MainPage_AlarmStateChanged;
        }

        private void btn_Alarmlar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.frame.Content = MainWindow.alarmList;
            MainWindow main = (MainWindow)App.Current.MainWindow;
            main.effectReset();
        }

        private void btn_Acil_Stop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.PLCs[(int)StationName.AnaUnite].Write("Q_ACIL_STOP_EM.ROL_BOBIN", false, TypeCode.Boolean);



        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Main.updatePage((int)Sayfalar.AnaSayfa, this);
            sta_Alarm.State = ((S71200_Main)Main.PLCs[(int)StationName.AnaUnite]).AlarmState;
        }

        private void DegerGir_Text_Baski_Suresi_Click(object sender, EventArgs e)
        {
            Main.PLCs[(int)StationName.AnaUnite].Write("SAYKIL_BITIR", true, TypeCode.Boolean);
        }


        private void DegerGir_Digital_Baski_Suresi_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DegerGir_Text_Baski_Suresi.PerformClick();
        }

        private void btn_LOCK_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((MainWindow)App.Current.MainWindow).Lock_Popup.Visibility = Visibility.Visible;
            Main.LockStatus = true;
            lbl_LevelDescription.Content = "Kilitli";
        }
    }
}
