using SQL_Lib;
using System;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for KullaniciAyarlari.xaml
    /// </summary>
    public partial class KullaniciAyarlari : Page
    {
        //=  @"DataSource=" + Main.Config.dataFile + ";Password=RealTekno8684";

        public KullaniciAyarlari()
        {
            InitializeComponent();
        }
        string connString;
        private void BtnBeforePage_MouseUp(object sender, MouseButtonEventArgs e)
        {

            MainWindow.frame.Content = MainWindow.ayarlar;

        }

        private void PasswordBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            numarator1.FrameworkElement = (FrameworkElement)sender;
        }

        private void PasswordBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            numarator1.FrameworkElement = null;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string path = "Data.sdf";
            if (File.Exists(Main.Config.dataFile))
                path = Main.Config.dataFile;
            connString = @"DataSource=" + Main.Config.dataFile + ";Password=RealTekno8684";
            DataTable dt = model.SelectCeCommand("Select * from Users where security_level not in(3) ", connString);

            cmb_KullanciAdi.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmb_KullanciAdi.Items.Add(new ComboBoxItem() { Content = dt.Rows[i]["description"].ToString(), Uid = dt.Rows[i]["id"].ToString() });

            }


        }

        private void numarator1__OK(object sender, EventArgs e)
        {
            if (cmb_KullanciAdi.SelectedItem == null)
            {
                MessageBox.Show("Kullanıcı Seçiniz", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (password1 && password2)
            {
                string id = ((ComboBoxItem)(cmb_KullanciAdi.SelectedItem)).Uid;
                SqlCeCommand command = new SqlCeCommand("UPDATE Users SET password=@1 where id=" + id);
                command.Parameters.AddWithValue("@1", password_Box1.Password);
                try
                {
                    model.ExecuteCeCommand(command, connString);
                }
                catch (Exception)
                {
                    MessageBox.Show("İşlem Hatalı", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("İşlem Başarılı", "", MessageBoxButton.OK, MessageBoxImage.Information);
                password_Box1.Password = "";
                password_Box2.Password = "";
                password_Box1.Background = Brushes.White;
                password_Box2.Background = Brushes.White;

            }

        }

        private void cmb_KullaniciAdi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        bool password1 = false;
        bool password2 = false;

        private void password_Box1_PasswordChanged(object sender, RoutedEventArgs e)
        {

            if (password_Box1.Password.Length >= 4 && password_Box1.Password.Length <= 16)
            {
                password_Box1.Background = Brushes.LightGreen;
                password1 = true;
            }
            else
            {
                password_Box1.Background = Brushes.LightPink;
                password1 = false;

            }


        }

        private void password_Box2_PasswordChanged(object sender, RoutedEventArgs e)
        {

            if (password_Box1.Password == password_Box2.Password && password_Box2.Password.Length >= 4 && password_Box1.Password.Length <= 16)
            {
                password_Box2.Background = Brushes.LightGreen;
                password2 = true;
            }

            else
            {
                password_Box2.Background = Brushes.LightPink;
                password2 = false;
            }

        }


    }
}
