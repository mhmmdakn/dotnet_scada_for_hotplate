using Hot_Plate_Lib;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for Servo.xaml
    /// </summary>
    public partial class Servo : Page
    {
        public Servo()
        {
            InitializeComponent();
        }

        private void BtnBeforePage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.frame.Content = MainWindow.lineerCetvel;
        }

        private void BtnAfterPage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.frame.Content = MainWindow.girisUnitesi;

        }

        private void StateServo_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((S71200_Main)Main.PLCs[(int)StationName.AnaUnite]).JD640_Read = true;
            Main.updatePage((int)Sayfalar.Servo, this);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ((S71200_Main)Main.PLCs[(int)StationName.AnaUnite]).JD640_Read = false;

        }

        private void StateServo_MouseUp(object sender, MouseButtonEventArgs e)
        {

            foreach (UserControls.Servo_Error_List item in grid_error.Children)
            {
                if (item.ServoNum == ((UserControls.StateServo)sender).ServoNum)
                {
                    if (item.Visibility == Visibility.Visible)
                        item.Visibility = Visibility.Hidden;
                    else
                        item.Visibility = Visibility.Visible;
                }

            }
        }
    }
}
