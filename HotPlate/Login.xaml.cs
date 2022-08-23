using Hot_Plate_Lib;
using SQL_Lib;
using System;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for NumericKeypad.xaml
    /// </summary>
    public partial class Login : Window
    {
        bool firstPush = false;


        public bool Result { get; set; }
        public SecurityLevel SecurityLevel { get; set; }

        public int SecurityLevelResult { get; set; }
        public string LevelDescription { get; set; }
        public Login()
        {
            InitializeComponent();
        }

        string connString;

        private void Key_MouseUp(object sender, MouseButtonEventArgs e)
        {
            txt_password.Background = Brushes.White;
            if (!firstPush)
            {
                txt_password.Password = "";
                firstPush = true;
            }
            TextBlock textBlock = (TextBlock)sender;



            if (txt_password.Password.Length < 16)
            {
                txt_password.Password += textBlock.Text;
            }
        }



        private void Deletion_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (txt_password.Password.Length > 0)
                txt_password.Password = txt_password.Password.Substring(0, txt_password.Password.Length - 1);
        }

        private void Enter_MouseUp(object sender, MouseButtonEventArgs e)
        {

            SqlCeCommand cmd = new SqlCeCommand("Select * from Users where password=@1 order by security_level ");
            cmd.Parameters.AddWithValue("@1", txt_password.Password);

            DataTable dt = model.ExecuteCeCommandDT(cmd, connString);

            int security_level = 0;
            if (dt.Rows.Count > 0)
            {
                security_level = int.Parse(dt.Rows[0]["security_level"].ToString());
                LevelDescription = dt.Rows[0]["description"].ToString();

            }

            if (security_level >= (int)SecurityLevel)
            {
                Result = true;
                this.Close();
            }
            else
            {
                txt_password.Background = Brushes.LightPink;
            }

            SecurityLevelResult = security_level;
            firstPush = false;

        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //StreamReader reader = new StreamReader("config.json");
            //string jsonValue = reader.ReadToEnd();

            //Config config = JsonConvert.DeserializeObject<Config>(jsonValue);
            //connString = @"DataSource=" + config.dataFile + ";Password=RealTekno8684";
            string path = "Data.sdf";
            if (File.Exists(Main.Config.dataFile))
                path = Main.Config.dataFile;

            connString = @"DataSource=" + Main.Config.dataFile + ";Password=RealTekno8684";

            txt_LevelDesc.Text = Enum.GetName(SecurityLevel.GetType(), SecurityLevel);
        }
    }

}
