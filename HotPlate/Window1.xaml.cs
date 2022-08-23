using Hot_Plate_Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace HotPlate
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        //string connstrint = "Data Source=.;Initial Catalog=hot_plate_v1;User ID=sa;Password=realtekno;";


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, ReceteJSON> values = new Dictionary<string, ReceteJSON>();
            ReceteJSON recete = new ReceteJSON();
            recete.AT_SV = new UInt16[32];
            recete.UT_SV = new UInt16[32];
            recete.AT_RUN_STOP = new bool[32];
            recete.UT_RUN_STOP = new bool[32];
            recete.Basma_Suresi = 10000;
            values.Add("Recete1", recete);


            ReceteSave(values);
            //string json = JsonConvert.SerializeObject(values);

            //StreamWriter streamWriter = new StreamWriter("Recete.json");
            //streamWriter.Write(json);
            //streamWriter.Close();
        }
        Dictionary<string, ReceteJSON> values;
        private void btn_baslangic_Click(object sender, RoutedEventArgs e)
        {
            StreamReader reader = new StreamReader("Recete.json");
            string jsonValue = reader.ReadToEnd();
            reader.Close();
            values = JsonConvert.DeserializeObject<Dictionary<string, ReceteJSON>>(jsonValue);
            values["Recete1"].Basma_Suresi = 1000;



        }

        public void ReceteSave(object values)
        {
            string json = JsonConvert.SerializeObject(values);
            Stream receteStream = File.OpenWrite("Recete.json");
            StreamWriter streamWriter = new StreamWriter(receteStream, Encoding.UTF8);
            streamWriter.Write(json);
            streamWriter.Close();

        }
        Dictionary<string, string> tags;
        private void btn_Tag_Click(object sender, RoutedEventArgs e)
        {

            StreamReader reader = new StreamReader("TagName.csv");
            tags = new Dictionary<string, string>();
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string tagName = line.Split(';')[0];
                string address = line.Split(';')[1];
                tags.Add(tagName, address);
            }
        }


        S71200_Main main_plc = (S71200_Main)Main.PLCs[(int)StationName.AnaUnite];
        private void btn_yukle_Click(object sender, RoutedEventArgs e)
        {
            main_plc.Writed += Main_plc_Writed;

            for (int i = 0; i < 16; i++)
            {
                int index_alt = (int)DataBlockStartIndex.Alt_Tabla_Set + ((int)TM2_Byte_Len.TM2_Set_Byte_Len) * i;

                Tool.UintToByte(ref main_plc.TM2_Set_buff, values["Recete1"].AT_SV[i * 2], index_alt + ((int)TM2_Set_UINT.CH1_SV));

                Tool.UintToByte(ref main_plc.TM2_Set_buff, values["Recete1"].AT_SV[(i * 2) + 1], index_alt + ((int)TM2_Set_UINT.CH2_SV));

                Tool.BitToByte(ref main_plc.TM2_Set_buff, values["Recete1"].AT_RUN_STOP[i * 2], index_alt + ((int)TM2_Set_UINT.TM2_SET_BIT), (int)TM2_Set_Bit.CH1_STOP);

                Tool.BitToByte(ref main_plc.TM2_Set_buff, values["Recete1"].AT_RUN_STOP[(i * 2) + 1], index_alt + ((int)TM2_Set_UINT.TM2_SET_BIT), (int)TM2_Set_Bit.CH2_STOP);


                int index_ust = (int)DataBlockStartIndex.Ust_Tabla_Set + ((int)TM2_Byte_Len.TM2_Set_Byte_Len) * i;

                Tool.UintToByte(ref main_plc.TM2_Set_buff, values["Recete1"].UT_SV[i * 2], index_ust + ((int)TM2_Set_UINT.CH1_SV));

                Tool.UintToByte(ref main_plc.TM2_Set_buff, values["Recete1"].UT_SV[(i * 2) + 1], index_ust + ((int)TM2_Set_UINT.CH2_SV));

                Tool.BitToByte(ref main_plc.TM2_Set_buff, values["Recete1"].UT_RUN_STOP[i * 2], index_ust + ((int)TM2_Set_UINT.TM2_SET_BIT), (int)TM2_Set_Bit.CH1_STOP);

                Tool.BitToByte(ref main_plc.TM2_Set_buff, values["Recete1"].UT_RUN_STOP[(i * 2) + 1], index_ust + ((int)TM2_Set_UINT.TM2_SET_BIT), (int)TM2_Set_Bit.CH2_STOP);

            }

            main_plc.DBWrite((int)DataBlockNum.TM2_Set, 0, (int)DataBlockSize.TM2_Set, main_plc.TM2_Set_buff);

            main_plc.Write(tags["JD640_Set.Baskı_Suresi"], values["Recete1"].Basma_Suresi, TypeCode.Int16);




        }

        private void Main_plc_Writed(object sender, EventArgs e)
        {

            main_plc.Writed -= Main_plc_Writed;

        }

        private void btn_Split_Click(object sender, RoutedEventArgs e)
        {
            string address1 = "%DB6.DBW23";
            string address2 = "%DB6.DBD23";
            string address3 = "%DB6.DBX23.5";
            string address4 = "P#DB6.DBX23.5";
            string address = address4;

            int dbnum_start_index = 3;
            if (address.IndexOf("%DB") != -1)
            {
                dbnum_start_index = 3;
            }
            else if (address.IndexOf("P#DB") != -1)
            {
                dbnum_start_index = 4;
            }


            int dbnum_last_index = address.IndexOf('.');
            int DBNum = int.Parse(address.Substring(dbnum_start_index, dbnum_last_index - dbnum_start_index));

            int type_start_index = dbnum_last_index + 1;
            int type_len = 3;
            int type_last_index = type_start_index + type_len;

            string type = address.Substring(type_start_index, type_len);
            TypeCode typeCode = TypeCode.UInt16;
            if (type == "DBW")
                typeCode = TypeCode.UInt16;
            else if (type == "DBD")
                typeCode = TypeCode.UInt32;
            else if (type == "DBB")
                typeCode = TypeCode.Byte;
            else if (type == "DBX" && dbnum_start_index == 4)
                typeCode = TypeCode.Object;
            else if (type == "DBX")
                typeCode = TypeCode.Boolean;

            int startadress_start_index = type_last_index;
            int startadress_last_index = address.Length;


            string[] StartAddress = address.Substring(startadress_start_index, startadress_last_index - startadress_start_index).Split('.');

            int start_address_byte = int.Parse(StartAddress[0]);


            int start_address_bit = 0;
            if (StartAddress.Length > 1)
                start_address_bit = int.Parse(StartAddress[1]);




        }

        private void btn_Formul_Click(object sender, RoutedEventArgs e)
        {
            StreamReader reader = new StreamReader("TagName.csv");
            tags = new Dictionary<string, string>();
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string tagName = line.Split(';')[0];
                string address = line.Split(';')[1];
                tags.Add(tagName, address);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // res.PresentValue = UInt16.Parse(text.Text.ToString());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Main.LockStatus = !Main.LockStatus;

        }
    }
}
