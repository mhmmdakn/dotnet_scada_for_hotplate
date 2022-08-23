using Hot_Plate_Lib;
using System;
namespace HotPlate
{
    /// <summary>
    /// Interaction logic for _7segDisplay.xaml
    /// </summary>
    public partial class DegerOku : PLC_BaseUserControl
    {
        public DegerOku()
        {
            InitializeComponent();
        }




        public string Aciklama { get; set; }
        public string Birim { get; set; } = "";

        public string Adres { get; set; }

        public float Kazanc { get; set; } = 1;
        public float Offset { get; set; } = 0;
        public TypeCode TypeCode { get; set; } = TypeCode.Int16;




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





    }
}
