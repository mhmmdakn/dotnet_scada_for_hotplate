using Hot_Plate_Lib;
using System;
using System.Windows;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for BtnHome.xaml
    /// </summary>
    public partial class BtnHome : PLC_BaseUserControl
    {
        public BtnHome()
        {
            InitializeComponent();
        }

        private string mainContent;

        public string MainContent
        {
            get { return mainContent; }
            set
            {
                mainContent = value;
                btn_Home.Content = mainContent;

            }
        }


        private object _value;
        public override object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                State = (bool)value;
            }
        }

        private bool state;

        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                if (state)
                    btn_Home.IsEnabled = false;
                else
                    btn_Home.IsEnabled = true;

            }
        }






        //private bool state;

        //public bool State
        //{
        //    get { return state; }
        //    set
        //    {

        //        state = value;
        //        if (state)
        //        {
        //            if (state)
        //            {
        //                btn_Home.Background = new SolidColorBrush(Colors.Lime);
        //            }

        //            else
        //            {
        //                btn_Home.Background = new SolidColorBrush(Colors.Red);
        //            }
        //        }

        //    }
        //}

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Main.PLCs[(int)PLCStationName].Write(PLCWriteAdress, true, TypeCode.Boolean);
        }

    }
}
