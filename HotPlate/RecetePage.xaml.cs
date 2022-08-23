using Hot_Plate_Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace HotPlate
{
    /// <summary>
    /// Interaction logic for RecetePage.xaml
    /// </summary>
    public partial class RecetePage : Page
    {


        ReceteJSON receteJSON;
        List<ReceteParam> receteList = new List<ReceteParam>();
        bool loaded = false;
        int rowIndex = -1;
        bool firstPush = false;
        string recete_adi = "";
        bool edit = false;

        public RecetePage()
        {
            InitializeComponent();
        }

        void update()
        {
            dt_recete.ItemsSource = null;


            int id = 0;

            receteList[id++].Value = receteJSON.Basma_Suresi;
            //receteList[id++].Value = receteJSON.Baski_Offset;
            receteList[id++].Value = receteJSON.Baski_Gucu;
            receteList[id++].Value = receteJSON.Kalip_Yukseklik;
            for (int i = 0; i < 32; i++)
            {
                receteList[id++].Value = receteJSON.AT_SV[i];
            }

            for (int i = 0; i < 32; i++)
            {
                receteList[id++].Value = receteJSON.UT_SV[i];
            }

            receteList[id++].Value = receteJSON.AT_PTol;
            receteList[id++].Value = receteJSON.AT_NTol;
            receteList[id++].Value = receteJSON.UT_PTol;
            receteList[id++].Value = receteJSON.UT_NTol;
            receteList[id++].Value = receteJSON.Basma_Suresi_PTol;
            receteList[id++].Value = receteJSON.Basma_Suresi_NTol;
            //receteList[id++].Value = receteJSON.Baski_Offset_PTol;
            //receteList[id++].Value = receteJSON.Baski_Offset_NTol;
            receteList[id++].Value = receteJSON.Baski_Gucu_PTol;
            receteList[id++].Value = receteJSON.Baski_Gucu_NTol;







            dt_recete.ItemsSource = receteList;
        }

        private void btn_Kaydet_Click(object sender, RoutedEventArgs e)
        {
            if (Main.LockStatus)
            {
                Login login = new Login() { SecurityLevel = SecurityLevel.Supervisor };
                login.ShowDialog();

                if (!login.Result)
                    return;
            }

            prg_Load.Value = 0;
            btn_Kaydet.IsEnabled = false;
            btn_Load.Visibility = Visibility.Hidden;
            lbl_Load.Content = "Kaydediliyor...";
            prg_Load_Grid.Visibility = Visibility.Visible;

            receteJSON = Main.Receteler[recete_adi];
            int id = 0;

            receteJSON.Basma_Suresi = Convert.ToUInt32(receteList[id++].Value);
            //receteJSON.Baski_Offset= Convert.ToSingle(receteList[id++].Value);
            receteJSON.Baski_Gucu = Convert.ToSingle(receteList[id++].Value);
            receteJSON.Kalip_Yukseklik = Convert.ToSingle(receteList[id++].Value);


            for (int i = 0; i < 32; i++)
            {
                receteJSON.AT_SV[i] = (Convert.ToUInt16(receteList[id++].Value));
            }

            for (int i = 0; i < 32; i++)
            {
                receteJSON.UT_SV[i] = (Convert.ToUInt16(receteList[id++].Value));
            }

            receteJSON.AT_PTol = Convert.ToUInt16(receteList[id++].Value);
            receteJSON.AT_NTol = Convert.ToUInt16(receteList[id++].Value);
            receteJSON.UT_PTol = Convert.ToUInt16(receteList[id++].Value);
            receteJSON.UT_NTol = Convert.ToUInt16(receteList[id++].Value);

            receteJSON.Basma_Suresi_PTol = Convert.ToUInt32(receteList[id++].Value);
            receteJSON.Basma_Suresi_NTol = Convert.ToUInt32(receteList[id++].Value);
            //receteJSON.Baski_Offset_PTol = Convert.ToSingle(receteList[id++].Value);
            //receteJSON.Baski_Offset_NTol = Convert.ToSingle(receteList[id++].Value);
            receteJSON.Baski_Gucu_PTol = Convert.ToSingle(receteList[id++].Value);
            receteJSON.Baski_Gucu_NTol = Convert.ToSingle(receteList[id++].Value);

            Main.Receteler[recete_adi] = receteJSON;
            string jsonValue = JsonConvert.SerializeObject(Main.Receteler);
            StreamWriter writer = new StreamWriter(Main.Config.receteFile);
            writer.Write(jsonValue);
            writer.Close();


            Main.CurrentRecete = Main.Receteler[recete_adi];


            prg_Load.Value = 100;
            btn_Kaydet.IsEnabled = true;
            btn_Load.Visibility = Visibility.Visible;
            lbl_Load.Content = "Kaydetme İşlemi Tamamlandı!";

        }


        private void btn_Yeni_Click(object sender, RoutedEventArgs e)
        {
            if (Main.LockStatus)
            {
                Login login = new Login() { SecurityLevel = SecurityLevel.Supervisor };
                login.ShowDialog();

                if (!login.Result)
                    return;
            }


            yeni_PopUp.Visibility = Visibility.Visible;
            txt_Yeni.Focus();

        }
        private void btn_Load_Click(object sender, RoutedEventArgs e)
        {
            prg_Load_Grid.Visibility = Visibility.Hidden;
        }


        private void btn_Edit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            edit = true;
            yeni_PopUp.Visibility = Visibility.Visible;
            txt_Yeni.Focus();
        }

        private void btn_Duzenle_Click(object sender, RoutedEventArgs e)
        {
            if (Main.LockStatus)
            {
                Login login = new Login() { SecurityLevel = SecurityLevel.Supervisor };
                login.ShowDialog();

                if (!login.Result)
                    return;
            }

            dgtc_Deger.IsReadOnly = false;
            btn_Edit.Visibility = Visibility.Visible;
        }

        private void btn_Iptal_Click(object sender, RoutedEventArgs e)
        {
            txt_Yeni.Text = "";
            yeni_PopUp.Visibility = Visibility.Hidden;
            edit = false;
        }




        private void btn_Tamam_Click(object sender, RoutedEventArgs e)
        {

            lbl_Uyarı.Content = "";

            if (txt_Yeni.Text.Length > 11)
            {
                lbl_Uyarı.Content = "Recete adı 11 karakterden fazla olamaz.";
                return;
            }

            if (Main.Receteler.ContainsKey(txt_Yeni.Text))
            {
                lbl_Uyarı.Content = "Aynı isim de recete mevcut. Farklı bir isim deneyiniz.";
                return;
            }


            if (edit)
            {
                ReceteJSON value = Main.Receteler[recete_adi];
                Main.Receteler.Remove(recete_adi);
                Main.Receteler[txt_Yeni.Text] = value;

                recete_adi = txt_Yeni.Text;

                ((ComboBoxItem)cmb_ReceteAdi.SelectedItem).Content = recete_adi;

                edit = false;
            }
            else
            {
                Main.Receteler.Add(txt_Yeni.Text, new ReceteJSON());
                cmb_ReceteAdi.Items.Add(new ComboBoxItem() { Content = txt_Yeni.Text });
                cmb_ReceteAdi.SelectedIndex = cmb_ReceteAdi.Items.Count - 1;

            }



            txt_Yeni.Text = "";
            yeni_PopUp.Visibility = Visibility.Hidden;
        }



        private void cmb_ReceteAdi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_ReceteAdi.SelectedItem != null)
                recete_adi = Main.Receteler.Keys.ElementAt(cmb_ReceteAdi.SelectedIndex);



            if (loaded)
            {
                receteJSON = Main.Receteler[recete_adi];
                update();
            }

        }


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            if (!loaded)
            {
                keyboard.ActiveContainer = yeni_PopUp;
            }


            if (!Main.LockStatus)
                btn_Duzenle_Click(this, new RoutedEventArgs());

            btn_Edit.Visibility = Visibility.Hidden;
            loaded = false;
            dt_recete.ItemsSource = null;

            string path = "Recete.json";
            if (File.Exists(Main.Config.receteFile))
                path = Main.Config.receteFile;

            StreamReader reader = new StreamReader(Main.Config.receteFile);
            string jsonValue = reader.ReadToEnd();
            reader.Close();
            Main.Receteler = JsonConvert.DeserializeObject<Dictionary<string, ReceteJSON>>(jsonValue);
            if (Main.Receteler == null)
            {
                Main.Receteler = new Dictionary<string, ReceteJSON>();
                Main.Receteler.Add("Recete1", new ReceteJSON());
                Main.ReceteAdi = "Recete1";
            }


            S71200_Main main_plc = (S71200_Main)Main.PLCs[(int)StationName.AnaUnite];
            recete_adi = Main.ReceteAdi;

            cmb_ReceteAdi.Items.Clear();
            int index = 0;
            foreach (string item in Main.Receteler.Keys)
            {

                cmb_ReceteAdi.Items.Add(new ComboBoxItem() { Content = item });

                if (item == recete_adi)
                    cmb_ReceteAdi.SelectedIndex = index;

                index++;
            }
            if (cmb_ReceteAdi.SelectedIndex == -1)
                cmb_ReceteAdi.SelectedIndex = 0;


            receteJSON = Main.Receteler[recete_adi];

            receteList.Clear();

            int id = 0;
            ReceteParam param;
            param = new ReceteParam();
            param.id = id++;
            param.Name = "Basma Süresi (s)";
            param.Value = receteJSON.Basma_Suresi;
            param.Alt_Limit = 0;
            param.Ust_Limit = 180;
            receteList.Add(param);

            //param = new ReceteParam();
            //param.id = id++;
            //param.Name = "Baskı Offset (mm)";
            //param.Value = receteJSON.Baski_Offset;
            //param.Alt_Limit = 0;
            //param.Ust_Limit = 4.5f;
            //receteList.Add(param);

            param = new ReceteParam();
            param.id = id++;
            param.Name = "Baskı Gücü (%)";
            param.Value = receteJSON.Baski_Gucu;
            param.Alt_Limit = 0;
            param.Ust_Limit = 100;
            receteList.Add(param);

            param = new ReceteParam();
            param.id = id++;
            param.Name = "Kalıp Yükseklik (mm)";
            param.Value = receteJSON.Kalip_Yukseklik;
            param.Alt_Limit = 25;
            param.Ust_Limit = 40;
            receteList.Add(param);


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    param = new ReceteParam();
                    param.id = id++;
                    param.Name = string.Format("Alt Tabla RES{0}.{1} (°C)", i + 1, j + 1);
                    param.Value = receteJSON.AT_SV[(i * 8) + j];
                    param.Alt_Limit = 0;
                    param.Ust_Limit = 240;
                    receteList.Add(param);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    param = new ReceteParam();
                    param.id = id++;
                    param.Name = string.Format("Üst Tabla RES{0}.{1} (°C)", i + 1, j + 1);
                    param.Value = receteJSON.UT_SV[(i * 8) + j];
                    param.Alt_Limit = 0;
                    param.Ust_Limit = 240;
                    receteList.Add(param);
                }
            }


            param = new ReceteParam();
            param.id = id++;
            param.Name = "Alt Tabla Pozitif Tolerans (°C)";
            param.Value = receteJSON.AT_PTol;
            param.Alt_Limit = 0;
            param.Ust_Limit = 125;
            receteList.Add(param);

            param = new ReceteParam();
            param.id = id++;
            param.Name = "Alt Tabla Negatif Tolerans (°C)";
            param.Value = receteJSON.AT_NTol;
            param.Alt_Limit = 0;
            param.Ust_Limit = 125;
            receteList.Add(param);

            param = new ReceteParam();
            param.id = id++;
            param.Name = "Üst Tabla Pozitif Tolerans (°C)";
            param.Value = receteJSON.UT_PTol;
            param.Alt_Limit = 0;
            param.Ust_Limit = 125;
            receteList.Add(param);

            param = new ReceteParam();
            param.id = id++;
            param.Name = "Üst Tabla Negatif Tolerans (°C)";
            param.Value = receteJSON.UT_NTol;
            param.Alt_Limit = 0;
            param.Ust_Limit = 125;
            receteList.Add(param);




            param = new ReceteParam();
            param.id = id++;
            param.Name = "Basma Süresi Pozitif Tolerans (s)";
            param.Value = receteJSON.Basma_Suresi_PTol;
            param.Alt_Limit = 0;
            param.Ust_Limit = 90;
            receteList.Add(param);

            param = new ReceteParam();
            param.id = id++;
            param.Name = "Basma Süresi Negatif Tolerans (s)";
            param.Value = receteJSON.Basma_Suresi_NTol;
            param.Alt_Limit = 0;
            param.Ust_Limit = 90;
            receteList.Add(param);

            //param = new ReceteParam();
            //param.id = id++;
            //param.Name = "Baskı Offset Pozitif Tolerans (mm)";
            //param.Value = receteJSON.Baski_Offset_PTol;
            //param.Alt_Limit = 0;
            //param.Ust_Limit = 0.2f;
            //receteList.Add(param);

            //param = new ReceteParam();
            //param.id = id++;
            //param.Name = "Baskı Offset Negatif Tolerans (mm)";
            //param.Value = receteJSON.Baski_Offset_NTol;
            //param.Alt_Limit = 0;
            //param.Ust_Limit = 0.2f;
            //receteList.Add(param);

            param = new ReceteParam();
            param.id = id++;
            param.Name = "Baskı Gücü Pozitif Tolerans (%)";
            param.Value = receteJSON.Baski_Gucu_PTol;
            param.Alt_Limit = 0;
            param.Ust_Limit = 50;
            receteList.Add(param);

            param = new ReceteParam();
            param.id = id++;
            param.Name = "Baskı Gücü Negatif Tolerans (%)";
            param.Value = receteJSON.Baski_Gucu_NTol;
            param.Alt_Limit = 0;
            param.Ust_Limit = 50;
            receteList.Add(param);

            dt_recete.ItemsSource = receteList;
            dgtc_Deger.IsReadOnly = true;

            loaded = true;
        }









        private void dt_recete_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            rowIndex = e.Row.GetIndex();
            firstPush = true;
        }

        private void dt_recete_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (((ReceteParam)dt_recete.Items[rowIndex]).Value.ToString() == "")
            {
                ((ReceteParam)dt_recete.Items[rowIndex]).CancelEdit();
                rowIndex = -1;
                firstPush = false;
                return;
            }


            float alt = ((ReceteParam)dt_recete.Items[rowIndex]).Alt_Limit;
            float ust = ((ReceteParam)dt_recete.Items[rowIndex]).Ust_Limit;
            float val = Convert.ToSingle(((ReceteParam)dt_recete.Items[rowIndex]).Value);


            if ((val < alt) || (val > ust))
            {
                ((ReceteParam)dt_recete.Items[rowIndex]).CancelEdit();
                MessageBox.Show(string.Format("Limit Değerleri Arasında Giriş Yapınız.\nAlt Limit:{0}\nÜst Limit:{1}", alt, ust));
            }

            rowIndex = -1;
            firstPush = false;
        }

        private void Numarator__Pressed(object sender, EventArgs e)
        {

            if (rowIndex != -1)
            {
                if (firstPush)
                {
                    ((ReceteParam)dt_recete.Items[rowIndex]).Value = numPad.Key;
                    firstPush = false;
                }
                else
                {
                    ((ReceteParam)dt_recete.Items[rowIndex]).Value += numPad.Key;
                }

            }

        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void numPad__UpKey(object sender, EventArgs e)
        {
            try
            {
                dt_recete.SelectedItem = dt_recete.Items[dt_recete.SelectedIndex - 1];
                dt_recete.CurrentItem = dt_recete.Items[dt_recete.SelectedIndex];

                dt_recete.ScrollIntoView(dt_recete.SelectedItem);
            }
            catch (Exception)
            {

            }

        }

        private void numPad__DownKey(object sender, EventArgs e)
        {
            try
            {
                dt_recete.SelectedItem = dt_recete.Items[dt_recete.SelectedIndex + 1];
                dt_recete.CurrentItem = dt_recete.Items[dt_recete.SelectedIndex];
                dt_recete.ScrollIntoView(dt_recete.SelectedItem);
            }
            catch (Exception)
            {

            }
        }

        private void numPad__BackSpace(object sender, EventArgs e)
        {
            if (rowIndex != -1)
            {
                if (firstPush)
                {
                    ((ReceteParam)dt_recete.Items[rowIndex]).Value = "";
                    firstPush = false;
                }
                else
                {
                    string val = ((ReceteParam)dt_recete.Items[rowIndex]).Value.ToString();
                    int len = val.Length - 1;
                    if (len > -1)
                        ((ReceteParam)dt_recete.Items[rowIndex]).Value = val.Substring(0, len);
                }

            }
        }

        private void numPad__OK(object sender, EventArgs e)
        {
            if (rowIndex == -1)
                dt_recete.BeginEdit();
            else
            {
                dt_recete.SelectedItem = dt_recete.Items[dt_recete.SelectedIndex + 1];
                dt_recete.CurrentItem = dt_recete.Items[dt_recete.SelectedIndex];
                dt_recete.BeginEdit();
                dt_recete.ScrollIntoView(dt_recete.SelectedItem);
            }

        }



        private async void btn_Yukle_Click(object sender, RoutedEventArgs e)
        {
            if (Main.LockStatus)
            {
                Login login = new Login() { SecurityLevel = SecurityLevel.Operator };
                login.ShowDialog();

                if (!login.Result)
                    return;
            }


            prg_Load.Value = 0;
            btn_Yukle.IsEnabled = false;
            btn_Load.Visibility = Visibility.Hidden;
            lbl_Load.Content = "Yükleniyor...";
            prg_Load_Grid.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {



                S71200_Main main_plc = (S71200_Main)Main.PLCs[(int)StationName.AnaUnite];

                Array.Clear(main_plc.RECETE_buff, 0, main_plc.RECETE_buff.Length);
                Array.Clear(main_plc.TOLERANS_buff, 0, main_plc.TOLERANS_buff.Length);
                Array.Clear(main_plc.TM2_Set_buff, 0, main_plc.TM2_Set_buff.Length);


                main_plc.Write("SAYKIL_BITIR", true, TypeCode.Boolean);

                for (int i = 0; i < 16; i++)
                {
                    int index_alt = (int)DataBlockStartIndex.Alt_Tabla_Set + ((int)TM2_Byte_Len.TM2_Set_Byte_Len) * i;

                    Tool.UintToByte(ref main_plc.TM2_Set_buff, (ushort)(receteJSON.AT_SV[i * 2] * 10), index_alt + ((int)TM2_Set_UINT.CH1_SV));

                    Tool.UintToByte(ref main_plc.TM2_Set_buff, (ushort)(receteJSON.AT_SV[(i * 2) + 1] * 10), index_alt + ((int)TM2_Set_UINT.CH2_SV));

                    //Tool.BitToByte(ref main_plc.TM2_Set_buff, receteJSON.AT_RUN_STOP[i * 2], index_alt + ((int)TM2_Set_UINT.TM2_SET_BIT), (int)TM2_Set_Bit.CH1_STOP);

                    //Tool.BitToByte(ref main_plc.TM2_Set_buff, receteJSON.AT_RUN_STOP[(i * 2) + 1], index_alt + ((int)TM2_Set_UINT.TM2_SET_BIT), (int)TM2_Set_Bit.CH2_STOP);


                    int index_ust = (int)DataBlockStartIndex.Ust_Tabla_Set + ((int)TM2_Byte_Len.TM2_Set_Byte_Len) * i;

                    Tool.UintToByte(ref main_plc.TM2_Set_buff, (ushort)(receteJSON.UT_SV[i * 2] * 10), index_ust + ((int)TM2_Set_UINT.CH1_SV));

                    Tool.UintToByte(ref main_plc.TM2_Set_buff, (ushort)(receteJSON.UT_SV[(i * 2) + 1] * 10), index_ust + ((int)TM2_Set_UINT.CH2_SV));

                    //Tool.BitToByte(ref main_plc.TM2_Set_buff, receteJSON.UT_RUN_STOP[i * 2], index_ust + ((int)TM2_Set_UINT.TM2_SET_BIT), (int)TM2_Set_Bit.CH1_STOP);

                    //Tool.BitToByte(ref main_plc.TM2_Set_buff, receteJSON.UT_RUN_STOP[(i * 2) + 1], index_ust + ((int)TM2_Set_UINT.TM2_SET_BIT), (int)TM2_Set_Bit.CH2_STOP);
                    Dispatcher.Invoke(() => { prg_Load.Value++; });
                }



                Tool.StringToByte(ref main_plc.RECETE_buff, (int)Recete.Recete_Adi, 11, recete_adi);
                Tool.UDintToByte(ref main_plc.RECETE_buff, receteJSON.Basma_Suresi, (int)Recete.Baski_Suresi);
                //Tool.RealToByte(ref main_plc.RECETE_buff, receteJSON.Baski_Offset, (int)Recete.Baski_Offset);
                Tool.RealToByte(ref main_plc.RECETE_buff, receteJSON.Baski_Gucu, (int)Recete.Baski_Gucu);
                Tool.RealToByte(ref main_plc.RECETE_buff, receteJSON.Kalip_Yukseklik, (int)Recete.Kalip_Yukseklik);

                Tool.UintToByte(ref main_plc.TOLERANS_buff, receteJSON.AT_PTol, (int)Tolerans.Alt_Tabla_P);
                Tool.UintToByte(ref main_plc.TOLERANS_buff, receteJSON.AT_NTol, (int)Tolerans.Alt_Tabla_N);
                Tool.UintToByte(ref main_plc.TOLERANS_buff, receteJSON.UT_PTol, (int)Tolerans.Ust_Tabla_P);
                Tool.UintToByte(ref main_plc.TOLERANS_buff, receteJSON.UT_NTol, (int)Tolerans.Ust_Tabla_N);
                Tool.UDintToByte(ref main_plc.TOLERANS_buff, receteJSON.Basma_Suresi_PTol, (int)Tolerans.Baski_Suresi_P);
                Tool.UDintToByte(ref main_plc.TOLERANS_buff, receteJSON.Basma_Suresi_NTol, (int)Tolerans.Baski_Suresi_N);
                //Tool.RealToByte(ref main_plc.TOLERANS_buff, receteJSON.Baski_Offset_PTol, (int)Tolerans.Baski_Offset_P);
                //Tool.RealToByte(ref main_plc.TOLERANS_buff, receteJSON.Baski_Offset_NTol, (int)Tolerans.Baski_Offset_N);
                Tool.RealToByte(ref main_plc.TOLERANS_buff, receteJSON.Baski_Gucu_PTol, (int)Tolerans.Baski_Gucu_P);
                Tool.RealToByte(ref main_plc.TOLERANS_buff, receteJSON.Baski_Gucu_NTol, (int)Tolerans.Baski_Gucu_N);
                Tool.RealToByte(ref main_plc.TOLERANS_buff, receteJSON.Kalip_Yukseklik_PTol, (int)Tolerans.Kalip_Yukseklik_P);
                Tool.RealToByte(ref main_plc.TOLERANS_buff, receteJSON.Kalip_Yukseklik_NTol, (int)Tolerans.Kalip_Yukseklik_N);


                for (int i = 0; i < 32; i++)
                {
                    Tool.UintToByte(ref main_plc.RECETE_buff, receteJSON.AT_SV[i], (int)Recete.Alt_Tabla_RV + i * 2);

                    Tool.UintToByte(ref main_plc.RECETE_buff, receteJSON.UT_SV[i], (int)Recete.Ust_Tabla_RV + i * 2);


                    Dispatcher.Invoke(() => { prg_Load.Value++; });
                }





                main_plc.DBWrite((int)DataBlockNum.TM2_Set, 0, (int)DataBlockSize.TM2_Set, main_plc.TM2_Set_buff);
                Dispatcher.Invoke(() => { prg_Load.Value++; });
                Thread.Sleep(200);


                main_plc.Write(main_plc.Tags["JD640_Set.Baskı_Suresi"], receteJSON.Basma_Suresi * 1000, TypeCode.UInt32);
                Dispatcher.Invoke(() => { prg_Load.Value++; });
                Thread.Sleep(100);

                //main_plc.Write(main_plc.Tags["JD640_Set.Baskı_Offset"], receteJSON.Baski_Offset, TypeCode.Single);
                //Dispatcher.Invoke(() => { prg_Load.Value++; });
                //Thread.Sleep(100);

                main_plc.Write(main_plc.Tags["JD640_Set.Current_Rate"], receteJSON.Baski_Gucu * 0.01, TypeCode.Single);
                Dispatcher.Invoke(() => { prg_Load.Value++; });
                Thread.Sleep(100);

                main_plc.Write(main_plc.Tags["JD640_Set.Kalıp_Kalınlık"], receteJSON.Kalip_Yukseklik, TypeCode.Single);
                Dispatcher.Invoke(() => { prg_Load.Value++; });
                Thread.Sleep(100);

                main_plc.DBWrite((int)DataBlockNum.RECETE, 0, (int)DataBlockSize.RECETE, main_plc.RECETE_buff);
                Dispatcher.Invoke(() => { prg_Load.Value++; });
                Thread.Sleep(200);

                main_plc.DBWrite((int)DataBlockNum.TOLERANS, 0, (int)DataBlockSize.TOLERANS, main_plc.TOLERANS_buff);
                Dispatcher.Invoke(() => { prg_Load.Value++; });
                Thread.Sleep(200);




            });

            Main.CurrentRecete = receteJSON;
            Main.ReceteAdi = recete_adi;
            prg_Load.Value = 100;
            btn_Yukle.IsEnabled = true;
            btn_Load.Visibility = Visibility.Visible;
            lbl_Load.Content = "Yükleme Tamamlandı!";

        }

        private async void btn_Upload_Click(object sender, RoutedEventArgs e)
        {
            if (Main.LockStatus)
            {
                Login login = new Login() { SecurityLevel = SecurityLevel.Supervisor };
                login.ShowDialog();

                if (!login.Result)
                    return;
            }




            prg_Load.Value = 0;
            btn_Upload.IsEnabled = false;
            btn_Load.Visibility = Visibility.Hidden;
            lbl_Load.Content = "Yükleniyor...";
            prg_Load_Grid.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {


                S71200_Main main_plc = (S71200_Main)Main.PLCs[(int)StationName.AnaUnite];
                main_plc.TM2_Read = true;

                main_plc.Connect_S7Client();

                Thread.Sleep(1000);

                for (int i = 0; i < 16; i++)
                {
                    int read_input_index = (int)DataBlockStartIndex.Read_Input_Alt_Tabla + ((int)TM2_Byte_Len.Read_Input_Byte_Len * i);
                    receteJSON.AT_SV[i * 2] = (ushort)(Tool.ByteToUInt(main_plc.TM2_buff, read_input_index + ((int)Read_Input_UINT.CH1_Set_Value)) / 10);
                    receteJSON.AT_SV[(i * 2) + 1] = (ushort)(Tool.ByteToUInt(main_plc.TM2_buff, read_input_index + ((int)Read_Input_UINT.CH2_Set_Value)) / 10);

                    read_input_index = (int)DataBlockStartIndex.Read_Input_Ust_Tabla + ((int)TM2_Byte_Len.Read_Input_Byte_Len * i);
                    receteJSON.UT_SV[i * 2] = (ushort)(Tool.ByteToUInt(main_plc.TM2_buff, read_input_index + ((int)Read_Input_UINT.CH1_Set_Value)) / 10);
                    receteJSON.UT_SV[(i * 2) + 1] = (ushort)(Tool.ByteToUInt(main_plc.TM2_buff, read_input_index + ((int)Read_Input_UINT.CH2_Set_Value)) / 10);
                    Dispatcher.Invoke(() => { prg_Load.Value += 2; });
                }


                receteJSON.Basma_Suresi = Convert.ToUInt32(main_plc.Read("JD640_Set.Baskı_Suresi", TypeCode.UInt32, main_plc.s7Client)) / 1000;
                Dispatcher.Invoke(() => { prg_Load.Value++; });
                Thread.Sleep(100);
                    //receteJSON.Baski_Offset = Convert.ToSingle(main_plc.Read("JD640_Set.Baskı_Offset", TypeCode.Single));
                    //Dispatcher.Invoke(() => { prg_Load.Value++; });
                    //Thread.Sleep(100);
                    receteJSON.Baski_Gucu = Convert.ToSingle(main_plc.Read("JD640_Set.Current_Rate", TypeCode.Single, main_plc.s7Client)) / 0.01f;
                Dispatcher.Invoke(() => { prg_Load.Value++; });
                Thread.Sleep(100);
                receteJSON.Kalip_Yukseklik = Convert.ToSingle(main_plc.Read("JD640_Set.Kalıp_Kalınlık", TypeCode.Single, main_plc.s7Client));
                Dispatcher.Invoke(() => { prg_Load.Value++; });
                Thread.Sleep(100);

                main_plc.Disconnect_S7Client();

            });

            update();

            Main.CurrentRecete = receteJSON;
            prg_Load.Value = 100;
            btn_Upload.IsEnabled = true;
            btn_Load.Visibility = Visibility.Visible;
            lbl_Load.Content = "Yükleme Tamamlandı!";




        }


    }
}



/*
        public DataGridCell GetCell(int row, int column,DataGrid dataGrid)
        {
            DataGridRow rowContainer = GetRow(row,dataGrid);

            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);

                // try to get the cell but it may possibly be virtualized
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                if (cell == null)
                {
                    // now try to bring into view and retreive the cell
                    dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                }
                return cell;
            }
            return null;
        }


        public DataGridRow GetRow(int index,DataGrid dataGrid)
        {
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // may be virtualized, bring into view and try again
                dataGrid.ScrollIntoView(dataGrid.Items[index]);
                row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }
        static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
        public static void Send(Key key)
        {
            if (Keyboard.PrimaryDevice != null)
            {
                if (Keyboard.PrimaryDevice.ActiveSource != null)
                {
                    var e = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    };
                    InputManager.Current.ProcessInput(e);

                    // Note: Based on your requirements you may also need to fire events for:
                    // RoutedEvent = Keyboard.PreviewKeyDownEvent
                    // RoutedEvent = Keyboard.KeyUpEvent
                    // RoutedEvent = Keyboard.PreviewKeyUpEvent
                }
            }
        }*/
