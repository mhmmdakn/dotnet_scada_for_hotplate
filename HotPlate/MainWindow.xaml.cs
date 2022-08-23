using Hot_Plate_Lib;
using Sharp7;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public bool Load { get; set; }

        public static AlarmList alarmList = new AlarmList();
        public static MainPage mainPage = new MainPage();
        public static RezistansPage auto = new RezistansPage();
        public static ManualPage manual = new ManualPage();
        public static Plate lineerCetvel = new Plate();
        public static Home home = new Home();
        public static GirisUnitesi girisUnitesi = new GirisUnitesi();
        public static CikisUnitesi cikisUnitesi = new CikisUnitesi();
        public static Servo servo = new Servo();
        public static RecetePage recetePage = new RecetePage();
        public static Ayarlar ayarlar = new Ayarlar();
        public static KullaniciAyarlari kullaniciAyarlari = new KullaniciAyarlari();
        public static Frame frame;
        S71200_Main main_plc;
        S71200 giris_plc;
        S71200 cikis_plc;

        DispatcherTimer timer = new DispatcherTimer();
        INTRO intro;
        public MainWindow()
        {
            intro = new INTRO();
            intro.Show();


            Main.ReceteAdiSet += Main_ReceteAdiSet;
            InitializeComponent();
            timer.Interval = TimeSpan.FromMinutes(2);
            timer.Tick += Timer_Tick1;
        }

        private void Timer_Tick1(object sender, EventArgs e)
        {
            timer.Stop();
            Main.LockStatus = true;

            Lock_Popup.Visibility = Visibility.Visible;
            mainPage.lbl_LevelDescription.Content = "Kilitli";



        }

        private void Main_ReceteAdiSet(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() => { txt_Durum.Text = Main.ReceteAdi; });

        }

        CultureInfo ci = new CultureInfo("tr-TR");
        private void Timer_Tick(object sender, EventArgs e)
        {
            txt_tarih_saat.Text = DateTime.Now.ToString();

            if (main_plc.Connected)
                txt_ana_plc.Background = Brushes.LimeGreen;
            else
                txt_ana_plc.Background = Brushes.Red;

            if (giris_plc.Connected)
                txt_giris_plc.Background = Brushes.LimeGreen;
            else
                txt_giris_plc.Background = Brushes.Red;

            if (cikis_plc.Connected)
                txt_cikis_plc.Background = Brushes.LimeGreen;
            else
                txt_cikis_plc.Background = Brushes.Red;

        }




        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            this.Hide();


            Main.LockStatus = true;


            Main.init(txt_Durum, Dispatcher);

            main_plc = (S71200_Main)Main.PLCs[(int)StationName.AnaUnite];
            giris_plc = (S71200)Main.PLCs[(int)StationName.Giris];
            cikis_plc = (S71200)Main.PLCs[(int)StationName.Cikis];

            main_plc.Connecting_Fail += Main_plc_Connecting_Fail;
            giris_plc.Connecting_Fail += Main_plc_Connecting_Fail;
            cikis_plc.Connecting_Fail += Main_plc_Connecting_Fail;
            frame = MainFrame;
            MainFrame.Content = auto;
            btn_auto.Background.Opacity = 0.5;

            main_plc.Updated += Main_plc_Uptdated;
            main_plc.CurrentAlarmAdded += Main_plc_CurrentAlarmAdded;


            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.IsEnabled = true;


            this.Show();
            intro.Close();
            Load = true;
            //Window1 window = new Window1();
            //window.Show();
        }

        private void Main_plc_CurrentAlarmAdded(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                MainFrame.Content = alarmList;
                effectReset();
            });

        }
        short auto_mod;
        byte[] recete_adi_buff = new byte[14];
        private void Main_plc_Uptdated(object sender, EventArgs e)
        {
            auto_mod = Convert.ToInt16(main_plc.Read("STATUS.AUTO_MODE_STATE", TypeCode.UInt16));
            short manuel_mod = Convert.ToInt16(main_plc.Read("STATUS.MANUEL_MODE_STATE", TypeCode.UInt16));
            short referans_mod = Convert.ToInt16(main_plc.Read("STATUS.HOME_MODE_STATE", TypeCode.UInt16));
            Main.WorkMode = Convert.ToInt16(main_plc.Read("Giden_Veri.Work_Mode", TypeCode.UInt16));
            Main.RunStatus = Convert.ToBoolean(main_plc.Read("JD640_START", TypeCode.Boolean));

            string text = "";
            if (auto_mod != 0)
            {
                if (auto_mod == 1)
                    text = "OTOMATİK MOD AKTİF";
                else if (auto_mod == 2)
                    text = "OTOMATİK MOD PASİF";

            }
            else if (manuel_mod != 0)
            {
                if (manuel_mod == 1)
                    text = "MANUEL MOD AKTİF";
                else if (manuel_mod == 2)
                    text = "MANUEL MOD PASİF";
            }
            else if (referans_mod != 0)
            {
                if (referans_mod == 1)
                    text = "REFERANS MODU AKTİF";
                else if (referans_mod == 2)
                    text = "REFERANS MODU PASİF";
            }
            else
            {
                text = "...MOD SEÇİNİZ...";
            }

            object baski_suresi = Convert.ToUInt32(main_plc.Read("STATUS.Baski_Suresi_Kalan", TypeCode.UInt32));


            Dispatcher.Invoke(() =>
            {
                txt_Mod.Text = text;
                degerOku_Baski_Suresi.Value = baski_suresi;
            });


            main_plc.DBRead(48, 0, 14, ref recete_adi_buff, S7Consts.S7WLByte);
            Main.ReceteAdi = Tool.ByteToString(recete_adi_buff, 0);


        }

        private void Main_plc_Connecting_Fail(object sender, EventArgs e)
        {
            Try_Connect_Popup.Visibility = Visibility.Visible;

        }
        private void btn_TryConnect_Click(object sender, RoutedEventArgs e)
        {
            if (!main_plc.Connected)
                main_plc.Trying = true;
            if (!giris_plc.Connected)
                giris_plc.Trying = true;
            if (!cikis_plc.Connected)
                cikis_plc.Trying = true;


            Try_Connect_Popup.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            int index = int.Parse(((Button)e.Source).Uid);

            switch (index)
            {
                case 0:
                    MainFrame.Content = mainPage;
                    effectReset();
                    btn_main.Background.Opacity = 0.5;


                    break;
                case 1:
                    MainFrame.Content = auto;
                    effectReset();
                    btn_auto.Background.Opacity = 0.5;


                    break;
                case 2:
                    MainFrame.Content = manual;
                    effectReset();
                    btn_manual.Background.Opacity = 0.5;


                    break;
                case 3:
                    MainFrame.Content = home;
                    effectReset();
                    btn_home.Background.Opacity = 0.5;


                    break;

                case 4:
                    MainFrame.Content = recetePage;
                    effectReset();
                    btn_recete.Background.Opacity = 0.5;


                    break;
                case 5:

                    if (Main.LockStatus)
                    {
                        Login login = new Login() { SecurityLevel = SecurityLevel.Operator };
                        login.ShowDialog();

                        if (!login.Result)
                        {
                            break;

                        }
                    }

                    MainFrame.Content = ayarlar;
                    effectReset();
                    btn_ayarlar.Background.Opacity = 0.5;
                    break;
            }


        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        public void effectReset()
        {

            btn_main.Background.Opacity = 1;
            btn_auto.Background.Opacity = 1;
            btn_manual.Background.Opacity = 1;
            btn_home.Background.Opacity = 1;
            btn_recete.Background.Opacity = 1;
            btn_ayarlar.Background.Opacity = 1;
        }

        private void MainWindow_V_MouseDown(object sender, MouseButtonEventArgs e)
        {

            timer.Stop();
            if (auto_mod != 1) 
            {
                timer.Start();
            }
            

        }

        private void btn_LOCK_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Login login = new Login() { SecurityLevel = SecurityLevel.Operator };
            btn_LOCK.Visibility = Visibility.Hidden;
            login.ShowDialog();



            btn_LOCK.Visibility = Visibility.Visible;

            if (login.Result)
            {
                mainPage.lbl_LevelDescription.Content = login.LevelDescription;
                Lock_Popup.Visibility = Visibility.Hidden;

                if (login.SecurityLevelResult >= (int)SecurityLevel.Supervisor)
                    Main.LockStatus = false;
            }


        }



        private void MainWindow_V_MouseMove(object sender, MouseEventArgs e)
        {
            timer.Stop();
            if (auto_mod != 1)
                timer.Start();
        }

        private void MainWindow_V_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            if (auto_mod != 1)
                timer.Start();
        }
    }
}
