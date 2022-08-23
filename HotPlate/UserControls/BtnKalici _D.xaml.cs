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
    public partial class BtnKalici_D : PLC_BaseUserControl
    {
        public BtnKalici_D()
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

        public int Deger { get; set; }


        private object _value;

        public override object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                State = Convert.ToInt16(value);

            }
        }




        private int state;

        public int State
        {
            get { return state; }
            set
            {
                state = value;
                if (state == Deger)
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


            Main.PLCs[(int)PLCStationName].Write(PLCWriteAdress, Deger, DataType);


            // Main.PLCs[(int)PLCStationName].Read_Async(PLCReadAdress,Dispatcher,this);
        }
    }
}
