using Hot_Plate_Lib;
using System;
using System.Windows;
using System.Windows.Controls;
namespace HotPlate
{
    /// <summary>
    /// Interaction logic for AlarmList.xaml
    /// </summary>
    public partial class AlarmList : Page
    {
        public AlarmList()
        {
            InitializeComponent();

        }

        S71200_Main main_plc;
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            main_plc = (S71200_Main)Main.PLCs[(int)StationName.AnaUnite];
            if (main_plc != null)
                main_plc.Updated += Ana_plc_Uptdated;
        }

        private void Ana_plc_Uptdated(object sender, EventArgs e)
        {
            int alarm_sayisi = main_plc.Alarms.Count;
            Dispatcher.Invoke(() =>
            {

                dt_anlik.Items.Clear();
                for (int i = 0; i < alarm_sayisi; i++)
                {
                    if (main_plc.currentAlarm_State[i])
                    {

                        dt_anlik.Items.Add(main_plc.Alarms[i]);

                    }
                }

                dt_gecmis.Items.Clear();
                for (int i = main_plc.historyAlarms.Count - 1; i >= 0; i--)
                {
                    dt_gecmis.Items.Add(main_plc.historyAlarms[i]);
                }

            });



        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            main_plc.Updated -= Ana_plc_Uptdated;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Main.updatePage((int)Sayfalar.Alarmlar, this);
        }
    }
}
