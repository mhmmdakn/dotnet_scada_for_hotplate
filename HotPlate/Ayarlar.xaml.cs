using EasyModbus;
using Hot_Plate_Lib;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for Ayarlar.xaml
    /// </summary>
    public partial class Ayarlar : Page
    {
        public Ayarlar()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ModbusClient client = new ModbusClient("COM5");
            client.Baudrate = 38400;
            client.Parity = System.IO.Ports.Parity.None;
            try
            {
                client.Connect();
            }
            catch (Exception)
            {

                MessageBox.Show("Bağlantı Hatası");
                return;
            }

            int[,] temps = new int[17, 2];
            //int alt_tabla_ref = Convert.ToInt32(float.Parse(txt_alt_ref_sicaklik.Text)*10);
            for (int i = 1; i <= 16; i++)
            {
                client.UnitIdentifier = (byte)i;
                client.WriteSingleRegister(302, 0);
                client.WriteSingleRegister(1302, 0);
            }
            //for (int i = 1; i <= 16; i++)
            //{
            //    client.UnitIdentifier =(byte)i;
            //    temps[i,0]=client.ReadInputRegisters(1000, 1)[0];
            //    temps[i,1]=client.ReadInputRegisters(1006, 1)[0];
            //}
            //for (int i = 1; i <= 16; i++)
            //{
            //    client.UnitIdentifier = (byte)i;
            //    client.WriteSingleRegister(302,  alt_tabla_ref- temps[i, 0]);
            //    client.WriteSingleRegister(1302, alt_tabla_ref- temps[i, 1]);
            //}
            MessageBox.Show("İşlem Başarılı");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ModbusClient client = new ModbusClient("COM7");
            client.Baudrate = 38400;
            client.Parity = System.IO.Ports.Parity.None;
            try
            {
                client.Connect();
            }
            catch (Exception)
            {

                MessageBox.Show("Bağlantı Hatası");
                return;
            }

            int[,] temps = new int[17, 2];
            //int alt_tabla_ref = Convert.ToInt32(float.Parse(txt_ust_ref_sicaklik.Text)*10);
            for (int i = 1; i <= 16; i++)
            {
                client.UnitIdentifier = (byte)i;
                client.WriteSingleRegister(302, 0);
                client.WriteSingleRegister(1302, 0);
            }
            //for (int i = 1; i <= 16; i++)
            //{
            //    client.UnitIdentifier = (byte)i;
            //    temps[i, 0] = client.ReadInputRegisters(1000, 1)[0];
            //    temps[i, 1] = client.ReadInputRegisters(1006, 1)[0];
            //}
            //for (int i = 1; i <= 16; i++)
            //{
            //    client.UnitIdentifier = (byte)i;
            //    client.WriteSingleRegister(302, alt_tabla_ref - temps[i, 0]);
            //    client.WriteSingleRegister(1302, alt_tabla_ref - temps[i, 1]);
            //}
            MessageBox.Show("İşlem Başarılı");
        }

        private void btn_Kapat_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login() { SecurityLevel = SecurityLevel.TechnicalService };
            login.ShowDialog();

            if (login.Result)
            {
                App.Current.MainWindow.Close();
            }

        }

        private void btn_shutdown_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("shutdown", "/s /t 0");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Main.updatePage((int)Sayfalar.Ayarlar, this);
        }

        private void BtnAfterPage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Login login = new Login() { SecurityLevel = SecurityLevel.TechnicalService };
            login.ShowDialog();

            if (login.Result)
            {
                MainWindow.frame.Content = MainWindow.kullaniciAyarlari;
            }

        }

        private void degerGir_Baski_Offset_Click(object sender, EventArgs e)
        {
            Main.PLCs[(int)StationName.AnaUnite].Write("SAYKIL_BITIR", true, TypeCode.Boolean);
        }

        private void DegerGir_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
