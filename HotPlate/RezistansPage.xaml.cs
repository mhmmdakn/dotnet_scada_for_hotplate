using Hot_Plate_Lib;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for AutoPage.xaml
    /// </summary>
    public partial class RezistansPage : Page
    {
        public RezistansPage()
        {
            InitializeComponent();
        }

        private void BtnAfterPage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.frame.Content = MainWindow.lineerCetvel;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Main.updatePage((int)Sayfalar.Otomatik, this);
            ((S71200_Main)Main.PLCs[(int)StationName.AnaUnite]).TM2_Read = true;

        }

        private void btn_usSet_Click(object sender, RoutedEventArgs e)
        {
            if (Main.LockStatus)
            {
                Login login = new Login() { SecurityLevel = SecurityLevel.Supervisor };
                login.ShowDialog();

                if (!login.Result)
                    return;
            }
            Main.PLCs[(int)Hot_Plate_Lib.StationName.AnaUnite].Write("UST_ONE_SET", true, TypeCode.Boolean);

        }

        private void btn_AltSet_Click(object sender, RoutedEventArgs e)
        {
            if (Main.LockStatus)
            {
                Login login = new Login() { SecurityLevel = SecurityLevel.Supervisor };
                login.ShowDialog();

                if (!login.Result)
                    return;
            }
            Main.PLCs[(int)Hot_Plate_Lib.StationName.AnaUnite].Write("ALT_ONE_SET", true, TypeCode.Boolean);

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ((S71200_Main)Main.PLCs[(int)StationName.AnaUnite]).TM2_Read = false;
        }
    }
}
