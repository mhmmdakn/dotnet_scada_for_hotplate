using Hot_Plate_Lib;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
namespace HotPlate
{
    /// <summary>
    /// Interaction logic for _7segDisplay.xaml
    /// </summary>
    public partial class DegerGir_Text : PLC_BaseUserControl
    {
        public DegerGir_Text()
        {
            InitializeComponent();
        }

        public event EventHandler Click;
        private bool _keypad = false;

        public string Aciklama { get; set; }
        public string Birim { get; set; }

        public string Adres { get; set; }

        public float Kazanc { get; set; } = 1;
        public float Offset { get; set; } = 0;
        public TypeCode TypeCode { get; set; } = TypeCode.Int16;

        public bool MessageBoxState { get; set; }
        public string MessageBoxText { get; set; }
        public int MessgeBoxWorkMode { get; set; }
        public bool MessgeBoxRunStatus { get; set; }

        public string ParametreAdi { get; set; }
        public string PozitifToleransAdi { get; set; }
        public string NegatifToleransAdi { get; set; }

        public bool Recete { get; set; }

        public bool Keypad
        {
            get { return _keypad; }
            set
            {
                _keypad = value;

                txt_Value.MouseUp += new MouseButtonEventHandler(click);
            }
        }

        private object _value = 0;

        public override object Value
        {
            get { return _value; }
            set
            {

                _value = (Convert.ToSingle(value) - Offset) / Kazanc;
                if (TypeCode == TypeCode.Single)
                    txt_Value.Text = ((float)_value).ToString("0.###");
                else if (TypeCode == TypeCode.Int32)
                    txt_Value.Text = (Convert.ToInt32(_value)).ToString();
                else if (TypeCode == TypeCode.UInt32)
                    txt_Value.Text = (Convert.ToUInt32(_value)).ToString();
                else if (TypeCode == TypeCode.UInt16)
                    txt_Value.Text = (Convert.ToUInt16(_value)).ToString();
                else
                    txt_Value.Text = (Convert.ToInt16(_value)).ToString();

            }
        }



        private double textSize;

        public double TextSize
        {
            get { return textSize; }
            set
            {
                textSize = value;
                txt_Value.FontSize = textSize;
            }
        }
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                txt_Value.Text = text;
            }
        }

        public float Alt_Limit { get; set; }
        public float Ust_Limit { get; set; }

        public void PerformClick()
        {
            float alt_limit = Alt_Limit;
            float ust_limit = Ust_Limit;

            if (Recete && Main.LockStatus)
            {
                Type myType = typeof(ReceteJSON);
                PropertyInfo param = myType.GetProperty(ParametreAdi);
                PropertyInfo ptol = myType.GetProperty(PozitifToleransAdi);
                PropertyInfo ntol = myType.GetProperty(NegatifToleransAdi);


                float recete_degeri = Convert.ToSingle(param.GetValue(Main.CurrentRecete));
                float ptolerans = Convert.ToSingle(ptol.GetValue(Main.CurrentRecete));
                float ntolerans = Convert.ToSingle(ntol.GetValue(Main.CurrentRecete));

                ust_limit = recete_degeri + ptolerans;
                if (ust_limit > Ust_Limit)
                    ust_limit = Ust_Limit;

                alt_limit = recete_degeri - ntolerans;
                if (alt_limit < Alt_Limit)
                    alt_limit = Alt_Limit;
            }
            NumericKeypad numericKeypad = new NumericKeypad(float.Parse(Value.ToString()), alt_limit, ust_limit, Birim, Aciklama);
            //numericKeypad.WindowStartupLocation = WindowStartupLocation.Manual;
            numericKeypad.ShowDialog();

            if ((MessgeBoxWorkMode == Main.WorkMode) && (MessgeBoxRunStatus == Main.RunStatus))
                if (MessageBoxState && numericKeypad.Result)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show(MessageBoxText, "Uyarı", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.No)
                        return;
                }

            if (numericKeypad.Result)
            {

                Main.PLCs[(int)PLCStationName].Write(PLCWriteAdress, (numericKeypad.Set_Value * Kazanc) + Offset, DataType);
                if (Click != null)
                {
                    Click.Invoke(this, new EventArgs());
                }
                //todo: Devam edicek
            }
        }

        private void click(object sender, MouseButtonEventArgs e)
        {
            if (SecurityLevel != SecurityLevel.No_Security)
            {
                Login login = new Login() { SecurityLevel = SecurityLevel };
                login.ShowDialog();

                if (!login.Result)
                    return;

            }

            PerformClick();


        }

    }
}
