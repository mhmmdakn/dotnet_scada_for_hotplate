using Hot_Plate_Lib;
using System;
using System.Windows;

namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for BtnStart.xaml
    /// </summary>
    public partial class BtnStart : PLC_BaseUserControl
    {
        public BtnStart()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Main.PLCs[(int)PLCStationName].Write(PLCWriteAdress, true, DataType);
            // Main.PLCs[(int)PLCStationName].Read_Async(PLCWriteAdress, true);

        }

        private object _value;
        public override object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                State = Convert.ToBoolean(value);
            }
        }

        private double fontSize;

        public double FontSizeText
        {
            get { return fontSize; }
            set
            {
                fontSize = value;
                btn_Button.FontSize = fontSize;
            }
        }


        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                btn_Button.Content = text;
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
                    btn_Button.IsEnabled = false;
                else
                    btn_Button.IsEnabled = true;

            }
        }


    }
}
