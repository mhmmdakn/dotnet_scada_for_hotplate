using Hot_Plate_Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace HotPlate
{


    public class Main
    {

        public static event EventHandler ReceteAdiSet;

        public static IPLC[] PLCs = new IPLC[3];
        public static Config Config { get; set; }

        public static Dictionary<string, ReceteJSON> Receteler = new Dictionary<string, ReceteJSON>();
        public static Dictionary<string, string> AnaUniteTags = new Dictionary<string, string>();
        public static Dictionary<string, string> GirisTags = new Dictionary<string, string>();
        public static Dictionary<string, string> CikisTags = new Dictionary<string, string>();

        public static ReceteJSON CurrentRecete = new ReceteJSON();
        public static List<PLC_BaseUserControl>[] myBaseUserControls = new List<PLC_BaseUserControl>[15];


        public static int WorkMode { get; set; }
        public static bool RunStatus { get; set; }

        public static bool LockStatus { get; set; }

        private static string recete_adi;
        static Dispatcher _dispatcher;

        public static string ReceteAdi
        {
            get { return recete_adi; }
            set
            {
                recete_adi = value;
                if (ReceteAdiSet != null)
                    ReceteAdiSet.Invoke(new object(), new EventArgs());
                if (recete_adi != "")
                {
                    try
                    {
                        CurrentRecete = Receteler[recete_adi];
                    }
                    catch (Exception)
                    {


                    }
                }


            }

        }


        public static bool ReceteGetir()
        {
            string path = "Recete.json";
            if (File.Exists(Main.Config.receteFile))
                path = Main.Config.receteFile;

            StreamReader reader = new StreamReader(path);
            string jsonValue = reader.ReadToEnd();
            reader.Close();
            Main.Receteler = JsonConvert.DeserializeObject<Dictionary<string, ReceteJSON>>(jsonValue);
            return true;
        }



        public static bool init(TextBlock textBlock, Dispatcher dispatcher)
        {

            StreamReader reader = new StreamReader("config.json");
            string jsonValue = reader.ReadToEnd();
            Config = JsonConvert.DeserializeObject<Config>(jsonValue);
            reader.Close();


            _dispatcher = dispatcher;

            string hata = "";

            if (!ReceteGetir())
                hata = "Reçeteler Getirilemedi";

            if (!Tool.TagGetir(ref AnaUniteTags, "AnaUnite.csv"))
                hata = "Ana Ünite Tagları Getirilemedi...";

            if (!Tool.TagGetir(ref GirisTags, "Giris.csv"))
                hata = "Giriş Ünitesi Tagları Getirilemedi...";

            if (!Tool.TagGetir(ref CikisTags, "Cikis.csv"))
                hata = "Çıkış Ünitesi Tagları Getirilemedi...";


            PLCs[(int)StationName.AnaUnite] = new S71200_Main("192.168.0.1", dispatcher, StationName.AnaUnite, AnaUniteTags);

            PLCs[(int)StationName.Giris] = new S71200("192.168.0.2", dispatcher, StationName.Giris, GirisTags);

            PLCs[(int)StationName.Cikis] = new S71200("192.168.0.3", dispatcher, StationName.Cikis, CikisTags);


            ((S71200_Main)PLCs[(int)StationName.AnaUnite]).Alarm_Read = true;


            alarmUpdate();
            dispatcher.Invoke(() => { textBlock.Text = hata; });

            S71200_Main main_plc = (S71200_Main)Main.PLCs[(int)StationName.AnaUnite];
            try
            {
                Main.ReceteAdi = Receteler.Keys.ElementAt(0);
            }
            catch (Exception)
            {


            }




            return true;
        }



        private static void alarmUpdate()
        {
            List<Alarm> alarms = new List<Alarm>();
            alarms.Add(new Alarm("Servo İşlem Hatası", Colors.Red, "", true));
            alarms.Add(new Alarm("Ana Ünite Işık Bariyeri", Colors.Black, "", true));
            alarms.Add(new Alarm("Acil Stop", Colors.Black, "", true));
            alarms.Add(new Alarm("Akım Limiti Aşıldı", Colors.Red, "", true));
            alarms.Add(new Alarm("Servo Güvenlik İşlemleri Eksik", Colors.Blue, "Referans yapınız.", false));
            alarms.Add(new Alarm("Maksimum Sıcaklık Değeri Aşıldı", Colors.Red, "SSR'lerini ve PT100'leri kontrol ediniz. ", true));
            alarms.Add(new Alarm("İşlem Dışı Hareket Algılandı.", Colors.Red, "", true));
            alarms.Add(new Alarm("Servo 1 Hata", Colors.Red, "Hata listesini kontrol ediniz.", true));
            alarms.Add(new Alarm("Servo 2 Hata", Colors.Red, "Hata listesini kontrol ediniz.", true));
            alarms.Add(new Alarm("Servo 3 Hata", Colors.Red, "Hata listesini kontrol ediniz.", true));
            alarms.Add(new Alarm("Servo 4 Hata", Colors.Red, "Hata listesini kontrol ediniz.", true));
            alarms.Add(new Alarm("Çıkış Ünitesi Genel Alarm", Colors.Black, "", true));
            alarms.Add(new Alarm("Çıkış Kumaş Çekme Servo Sürücü Hatası", Colors.Red, "", true));
            alarms.Add(new Alarm("Çıkış Menzer Motor Sürücü Hatası", Colors.Red, "", true));
            alarms.Add(new Alarm("Çıkış Işık Bariyeri", Colors.Black, "", true));
            alarms.Add(new Alarm("Giriş Ünitesi Genel Alarm", Colors.Black, "", true));
            alarms.Add(new Alarm("Giriş Kumaş Besleme Servo Sürücü Hatası", Colors.Red, "", true));
            alarms.Add(new Alarm("Giriş Dok Çözme Motor Sürücü Hatası", Colors.Red, "", true));
            alarms.Add(new Alarm("Pres Referansı Yapılmadı.", Colors.Black, "Referans yapınız.", false));
            alarms.Add(new Alarm("Çekici Referansı Yapılmadı.", Colors.Black, "Referans yapınız.", true));
            alarms.Add(new Alarm("Pres Dengesiz", Colors.Red, "Referans yapınız.", true));
            alarms.Add(new Alarm("Motor Freni1 Hata", Colors.Red, "", true));
            alarms.Add(new Alarm("Motor Freni2 Hata", Colors.Red, "", true));
            alarms.Add(new Alarm("Fren Açma Hatası", Colors.Red, "", true));
            alarms.Add(new Alarm("Pozisyona Ulaştı Hatası", Colors.Red, "", true));



            ((S71200_Main)PLCs[(int)StationName.AnaUnite]).AlarmDbNum = 40;
            ((S71200_Main)PLCs[(int)StationName.AnaUnite]).Alarms = alarms;

        }

        public static void updatePage(int SayfaAdi, DependencyObject dependency)
        {

            if (myBaseUserControls[SayfaAdi] == null)
                Tool.PageControlList(dependency, SayfaAdi, ref myBaseUserControls);

            ((S71200_Main)PLCs[(int)StationName.AnaUnite]).UpdateUserControlList(myBaseUserControls[SayfaAdi]);
            ((S71200)PLCs[(int)StationName.Giris]).UpdateUserControlList(myBaseUserControls[SayfaAdi]);
            ((S71200)PLCs[(int)StationName.Cikis]).UpdateUserControlList(myBaseUserControls[SayfaAdi]);

        }
    }





    public enum Tabla
    {
        Alt_Tabla,
        Ust_Tabla
    }

    public enum Sayfalar
    {
        AnaSayfa,
        Otomatik,
        Manuel,
        Referans,
        Recete,
        Ayarlar,
        Alarmlar,
        Giris,
        Cikis,
        Servo,
        Plate
    }

    public enum DataBlockNum
    {
        TM2 = 6,
        TM2_Set = 11,
        JD640 = 23,
        JD640_Set = 24,
        RECETE = 48,
        TOLERANS = 49
    }


    public enum DataBlockStartIndex
    {
        Read_Input_Alt_Tabla = 0,
        Coil_Status_Alt_Tabla = 1792,
        Alt_Tabla_Set = 0,

        Read_Input_Ust_Tabla = 896,
        Coil_Status_Ust_Tabla = 1824,
        Ust_Tabla_Set = 96,

        Alt_Tabla_RV = 14,
        Ust_Tabla_RV = 78,

        Servo_Read = 0,
        Servo_Set_Read = 104
    }


    #region RECETE
    public enum Recete
    {
        Recete_Adi = 0,
        Alt_Tabla_RV = 14,
        Ust_Tabla_RV = 78,
        Baski_Suresi = 142,
        Baski_Offset = 146,
        Baski_Gucu = 150,
        Kalip_Yukseklik = 154

    }
    public enum Tolerans
    {
        Alt_Tabla_P = 0,
        Alt_Tabla_N = 2,
        Ust_Tabla_P = 4,
        Ust_Tabla_N = 6,
        Baski_Suresi_P = 8,
        Baski_Suresi_N = 12,
        Baski_Offset_P = 16,
        Baski_Offset_N = 20,
        Baski_Gucu_P = 24,
        Baski_Gucu_N = 28,
        Kalip_Yukseklik_P = 32,
        Kalip_Yukseklik_N = 36


    }
    #endregion

    #region JD640
    public enum JD640_Byte_Len
    {
        Servo_Read = 26,
        Servo_Set_Read = 52,
        Status_Word = 2,
        Error_State = 2,
        Din_Status = 2,
        Dout_Status = 2,
    }
    public enum Servo_Read
    {
        I_q_Int = 0,
        Pos_Error_DInt = 2,
        Pos_Actual_DInt = 6,
        Speed_RPM_Int = 10,
        Status_Word = 12,
        Error_State_1 = 14,
        Error_State_2 = 16,
        Din_Status = 18,
        Dout_Status = 20,
        Error_State_1_UInt = 22,
        Error_State_2_UInt = 24,
    }

    public enum Servo_Set_Read
    {
        Din_Pos0_DInt = 0,
        Din_Pos1_DInt = 4,
        Din_Pos2_DInt = 8,
        Din_Pos3_DInt = 12,
        Din_Speed0_DInt = 16,
        Din_Speed1_DInt = 20,
        Din_Speed2_DInt = 24,
        Din_Speed3_DInt = 28,
        CMD_q_Max_UInt = 32,
        Home_Offset_DInt = 34,
        Profile_Acc_UDInt = 38,
        Profile_Dec_UDInt = 42,
        Home_Speed_UDInt = 46,
        Home_Method_Byte = 50,
        Home_OFF_M_Byte = 51,
    }

    public enum Din_Status
    {
        Enable,
        Reset_Errors,
        Operation_Mode,
        Din_Pos_Index0,
        Din_Pos_Index1,
        Active_Command,
        Start_Homing,
        Homing_Signal
    }

    public enum Dout_Status
    {
        Ready,
        reserve1,
        Pos_Reached,
        Drive_Enabled,
        reserve2,
        Motor_Brake,
        Error,
        NC
    }


    public class JD640_Const
    {
        public const float I_q = 45.4f;
        public const float Pos_Actual = 13107;
        public const float Pos_Error = 13107;
        public const float Speed_RPM = 17895.424f;


    }
    #endregion


    #region TM2
    public enum TM2_Byte_Len
    {
        Read_Input_Byte_Len = 56,
        Coil_Status_Byte_Len = 2,
        IN_OUT_Byte_Len = 2,
        TM2_Set_Byte_Len = 6

    }
    public enum TM2_Set_UINT
    {
        CH1_SV = 0,
        CH2_SV = 2,
        TM2_SET_BIT = 4
    }
    public enum TM2_Set_Bit
    {
        CH1_STOP,
        CH2_STOP,
        CH1_AUTO_TUN,
        CH2_AUTO_TUN
    }

    public enum Read_Input_UINT
    {
        CH1_Present_Value = 0,
        CH1_Set_Value = 6,
        CH2_Present_Value = 12,
        CH2_Set_Value = 18,
        Unit_Adress = 50,
        CT1_Heater_Current = 52,
        CT2_Heater_Current = 54
    }
    public enum Read_Input_Byte
    {
        IN_OUT = 49

    }


    public enum Coil_Status
    {
        CH1_RUN_STOP,
        CH1_Auto_Tuning,
        CH2_RUN_STOP,
        CH2_Auto_Tuning,
        Coil_Status_Size,


    }
    public enum IN_OUT
    {
        CH1_Lamp,
        CH2_Lamp,
        nc,
        nc_1,
        AL1_Lamp,
        AL2_Lamp,
        AL3_Lamp,
        AL4_Lamp,
        DI_1_IN,
        DI_2_IN,
        IN_OUT_Size,
    }
    #endregion
}
