using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for NumericKeypad.xaml
    /// </summary>
    public partial class NumericKeypad : Window
    {
        bool firstPush = false;
        int plus_minus = 1;

        public bool Result { get; set; }
        public float Set_Value { get; set; }
        public float Alt_Limit { get; set; }
        public float Ust_Limit { get; set; }
        public bool Modul_Stop { get; set; }

        public NumericKeypad(float set_value, float alt_limit, float ust_limit, string birim, string aciklama,bool modul_kapat=false)
        {
            InitializeComponent();
            if (modul_kapat)
                Height = 645;
            Set_Value = set_value;
            Alt_Limit = alt_limit;
            Ust_Limit = ust_limit;
            txt_set.Text = Set_Value.ToString();
            txt_alt_limit.Text = Alt_Limit + birim;
            txt_ust_limit.Text = Ust_Limit + birim;
            txt_Aciklama.Text = aciklama;
        }


        private void Key_MouseUp(object sender, MouseButtonEventArgs e)
        {
            txtSet_Color.Color = Color.FromRgb(166, 166, 166);
            if (!firstPush)
            {
                txt_set.Text = "";
                firstPush = true;
            }
            TextBlock textBlock = (TextBlock)sender;


            float temp = 0;
            float.TryParse(txt_set.Text + textBlock.Text, out temp);
            if (temp <= Ust_Limit)
            {
                txt_set.Text += textBlock.Text;
            }
        }

        private void Clear_MouseUp(object sender, MouseButtonEventArgs e)
        {
            txt_set.Text = "";
        }

        private void Deletion_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (txt_set.Text.Length > 0)
                txt_set.Text = txt_set.Text.Substring(0, txt_set.Text.Length - 1);
        }

        private void Enter_MouseUp(object sender, MouseButtonEventArgs e)
        {
            float temp = 0;
            if (txt_set.Text == "")
                return;
            temp = float.Parse(txt_set.Text);
            if (temp >= Alt_Limit)
            {
                Set_Value = float.Parse(txt_set.Text);
                Result = true;
                this.Close();
            }
            else
            {
                txtSet_Color.Color = Colors.LightPink;
            }

        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void btn_plus_MouseUp(object sender, MouseButtonEventArgs e)
        {
            float temp = float.Parse(txt_set.Text) + plus_minus;
            if (temp <= Ust_Limit && temp >= Alt_Limit)
            {
                Set_Value = temp;
                txt_set.Text = Set_Value.ToString();
            }

        }

        private void btn_minus_MouseUp(object sender, MouseButtonEventArgs e)
        {
            float temp = float.Parse(txt_set.Text) - plus_minus;
            if (temp <= Ust_Limit && temp >= Alt_Limit)
            {
                Set_Value = temp;
                txt_set.Text = Set_Value.ToString();
            }
        }

        private void btn_stop_modul_Click(object sender, RoutedEventArgs e)
        {
            Modul_Stop = true;
            this.Close();
        }
    }

}
