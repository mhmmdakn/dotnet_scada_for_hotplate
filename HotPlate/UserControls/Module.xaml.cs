using Hot_Plate_Lib;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for Module.xaml
    /// </summary>
    public partial class Module : UserControl
    {
        public Module()
        {
            InitializeComponent();
            prog.Maximum = 0;

        }



        public Tabla Tabla { get; set; } = Tabla.Ust_Tabla;
        public int No { get; set; }

        public int Channel { get; set; } = 1;



        public string Set_Value_Address { get; set; }
        public string Run_Stop_Address { get; set; }
        private bool run_stop;

        public bool Run_Stop
        {
            get { return run_stop; }
            set
            {
                run_stop = value;
                Update();
            }
        }

        private bool autoTuning;

        public bool AutoTuning
        {
            get { return autoTuning; }
            set
            {
                autoTuning = value;
                Update();
            }
        }

        private bool resState;

        public bool ResState
        {
            get { return resState; }
            set
            {
                resState = value;
                Update();
            }
        }


        private int tolerans = 20;

        public int Tolerans
        {
            get { return tolerans; }
            set
            {
                if (tolerans != value)
                    tolerans = value;

                prog.Minimum = -(set_value + tolerans + 700);

                Update();
            }
        }

        private uint set_value;

        public uint Set_Value
        {
            get { return set_value; }
            set
            {

                set_value = value;
                txt_setValue.Text = (set_value / 10.0).ToString() + "°C";
                prog.Minimum = -(set_value + tolerans + 700);

                Update();

            }
        }

        private uint presentValue;

        public uint PresentValue
        {
            get { return presentValue; }
            set
            {

                presentValue = value;
                txt_presentValue.Text = (presentValue / 10.0).ToString().Replace(',', '.') + "°C";
                prog.Value = -(presentValue);

                Update();
            }

        }

        private uint currentValue;

        public uint CurrentValue
        {
            get { return currentValue; }
            set
            {

                currentValue = value;
                txt_currentValue.Text = (currentValue / 10.0).ToString().Replace(',', '.') + "A";

                Update();
            }
        }

        public string ParametreAdi { get; set; }
        public string PozitifToleransAdi { get; set; }
        public string NegatifToleransAdi { get; set; }

        public bool Recete { get; set; }

        public float Recete_Degeri { get; set; } = 0;
        public float Alt_Limit { get; set; } = 0;
        public float Ust_Limit { get; set; } = 240;

        public void Update()
        {
            if (run_stop)
            {
                rec_module.Fill = Brushes.LightSlateGray;
                txt_state.Visibility = Visibility.Visible;
                txt_state.Text = "Stop";
            }
            else if (autoTuning)
            {
                rec_module.Fill = Brushes.Yellow;
                txt_state.Visibility = Visibility.Visible;
                txt_state.Text = "Auto Tuning";
            }
            else if (resState)
            {
                rec_module.Fill = Brushes.HotPink;
                txt_state.Visibility = Visibility.Visible;
                txt_state.Text = "Rezistans Koptu";
            }
            else
            {
                if (presentValue > (set_value - tolerans) && presentValue < (set_value + tolerans))
                {
                    rec_module.Fill = Brushes.White;
                    txt_state.Visibility = Visibility.Hidden;
                }
                else if (presentValue > (set_value + tolerans))
                {
                    rec_module.Fill = Brushes.Red;
                    txt_state.Visibility = Visibility.Hidden;
                }
                else if (presentValue < (set_value - tolerans))
                {
                    rec_module.Fill = Brushes.Cyan;
                    txt_state.Visibility = Visibility.Hidden;
                }

            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            float alt_limit = Alt_Limit;
            float ust_limit = Ust_Limit;

            if (Main.LockStatus)
            {
                Type myType = typeof(ReceteJSON);

                if (Tabla == Tabla.Alt_Tabla)
                {
                    Recete_Degeri = Main.CurrentRecete.AT_SV[No];
                    PozitifToleransAdi = "AT_PTol";
                    NegatifToleransAdi = "AT_NTol";
                }

                else if (Tabla == Tabla.Ust_Tabla)
                {
                    Recete_Degeri = Main.CurrentRecete.UT_SV[No];
                    PozitifToleransAdi = "UT_PTol";
                    NegatifToleransAdi = "UT_NTol";
                }


                PropertyInfo ptol = myType.GetProperty(PozitifToleransAdi);
                PropertyInfo ntol = myType.GetProperty(NegatifToleransAdi);

                float ptolerans = Convert.ToSingle(ptol.GetValue(Main.CurrentRecete));
                float ntolerans = Convert.ToSingle(ntol.GetValue(Main.CurrentRecete));

                ust_limit = Recete_Degeri + ptolerans;
                if (ust_limit > Ust_Limit)
                    ust_limit = Ust_Limit;

                alt_limit = Recete_Degeri - ntolerans;
                if (alt_limit < Alt_Limit)
                    alt_limit = Alt_Limit;
            }


            NumericKeypad keypad = new NumericKeypad((float)(Set_Value / 10.0), alt_limit, ust_limit, "°C", Tabla + " " + txt_moduleNo.Text,true);
            keypad.ShowDialog();

            if (keypad.Result)
                Main.PLCs[(int)StationName.AnaUnite].Write(Set_Value_Address, keypad.Set_Value * 10, TypeCode.Int16);
            if(keypad.Modul_Stop)
                Main.PLCs[(int)StationName.AnaUnite].Write(Run_Stop_Address, true, TypeCode.Boolean);
        }
    }
}
