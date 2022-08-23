using Hot_Plate_Lib;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for BtnYuvarlak.xaml
    /// </summary>
    public partial class BtnKalici_M : PLC_BaseUserControl
    {
        public BtnKalici_M()
        {
            InitializeComponent();
        }



        private Color trueColor;

        public Color TrueColor
        {
            get { return trueColor; }
            set
            {
                trueColor = value;
                light_Color.Color = trueColor;
            }
        }

        private Color falseColor;

        public Color FalseColor
        {
            get { return falseColor; }
            set
            {
                falseColor = value;
                dark_Color.Color = falseColor;
            }
        }


        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                txt_Text.Text = text;
            }
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

        private bool reverse;

        public bool Reverse
        {
            get { return reverse; }
            set { reverse = value; }
        }



        private bool state;

        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                if (state != reverse)
                {
                    btn_State.Visibility = Visibility.Visible;

                }
                else
                {

                    btn_State.Visibility = Visibility.Hidden;


                }
            }
        }

        private void Viewbox_MouseUp(object sender, MouseButtonEventArgs e)
        {


            Main.PLCs[(int)PLCStationName].Write(PLCWriteAdress, !state, DataType);


            // Main.PLCs[(int)PLCStationName].Read_Async(PLCReadAdress,Dispatcher,this);
        }
    }
}
