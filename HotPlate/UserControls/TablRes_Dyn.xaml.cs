using Hot_Plate_Lib;
using System;
using System.Windows;
using System.Windows.Controls;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for TablRes_Dyn.xaml
    /// </summary>
    public partial class TablRes_Dyn : UserControl
    {

        UserControls.Module[][] moduls = new UserControls.Module[16][];
        S71200_Main main_plc = (S71200_Main)Main.PLCs[(int)StationName.AnaUnite];


        public TablRes_Dyn()
        {
            InitializeComponent();


        }

        bool _Loaded = false;

        private void Ana_Unite_PLC_Uptdated(object sender, EventArgs e)
        {

            for (int i = 0; i < 16; i++)
            {
                int read_input_index = RI_Start_Address + ((int)TM2_Byte_Len.Read_Input_Byte_Len * i);
                int coil_status_index = CS_Start_Address + ((int)TM2_Byte_Len.Coil_Status_Byte_Len * i);
                Dispatcher.Invoke(() =>
                {

                    moduls[i][0].PresentValue = Tool.ByteToUInt(main_plc.TM2_buff, read_input_index + ((int)Read_Input_UINT.CH1_Present_Value));
                    moduls[i][0].Set_Value = Tool.ByteToUInt(main_plc.TM2_buff, read_input_index + ((int)Read_Input_UINT.CH1_Set_Value));
                    moduls[i][0].CurrentValue = Tool.ByteToUInt(main_plc.TM2_buff, read_input_index + ((int)Read_Input_UINT.CT1_Heater_Current));
                    moduls[i][0].ResState = Tool.ByteToBit(main_plc.TM2_buff, read_input_index + ((int)Read_Input_Byte.IN_OUT), (int)IN_OUT.AL1_Lamp);
                    moduls[i][0].Run_Stop = Tool.ByteToBit(main_plc.TM2_buff, coil_status_index, (int)Coil_Status.CH1_RUN_STOP);
                    moduls[i][0].AutoTuning = Tool.ByteToBit(main_plc.TM2_buff, coil_status_index, (int)Coil_Status.CH1_Auto_Tuning);



                    moduls[i][1].PresentValue = Tool.ByteToUInt(main_plc.TM2_buff, read_input_index + ((int)Read_Input_UINT.CH2_Present_Value));
                    moduls[i][1].Set_Value = Tool.ByteToUInt(main_plc.TM2_buff, read_input_index + ((int)Read_Input_UINT.CH2_Set_Value));
                    moduls[i][1].CurrentValue = Tool.ByteToUInt(main_plc.TM2_buff, read_input_index + ((int)Read_Input_UINT.CT2_Heater_Current));
                    moduls[i][1].ResState = Tool.ByteToBit(main_plc.TM2_buff, read_input_index + ((int)Read_Input_Byte.IN_OUT), (int)IN_OUT.AL2_Lamp);
                    moduls[i][1].Run_Stop = Tool.ByteToBit(main_plc.TM2_buff, coil_status_index, (int)Coil_Status.CH2_RUN_STOP);
                    moduls[i][1].AutoTuning = Tool.ByteToBit(main_plc.TM2_buff, coil_status_index, (int)Coil_Status.CH2_Auto_Tuning);


                });
            }
        }


        int RI_Start_Address = 0;
        int CS_Start_Address = 0;
        int TM2_SET_Start_Address = 0;

        private Tabla tabla = Tabla.Alt_Tabla;
        public Tabla Tabla
        {
            get { return tabla; }
            set
            {
                tabla = value;
                if (tabla == Tabla.Alt_Tabla)
                {
                    RI_Start_Address = (int)DataBlockStartIndex.Read_Input_Alt_Tabla;
                    CS_Start_Address = (int)DataBlockStartIndex.Coil_Status_Alt_Tabla;
                    TM2_SET_Start_Address = (int)DataBlockStartIndex.Alt_Tabla_Set;

                }
                else if (tabla == Tabla.Ust_Tabla)
                {
                    RI_Start_Address = (int)DataBlockStartIndex.Read_Input_Ust_Tabla;
                    CS_Start_Address = (int)DataBlockStartIndex.Coil_Status_Ust_Tabla;
                    TM2_SET_Start_Address = (int)DataBlockStartIndex.Ust_Tabla_Set;
                }
            }

        }

        private void BaseGrid_Loaded(object sender, RoutedEventArgs e)
        {

            if (!_Loaded)
            {

                if (tabla == Tabla.Alt_Tabla)
                {
                    RI_Start_Address = (int)DataBlockStartIndex.Read_Input_Alt_Tabla;
                    CS_Start_Address = (int)DataBlockStartIndex.Coil_Status_Alt_Tabla;
                    TM2_SET_Start_Address = (int)DataBlockStartIndex.Alt_Tabla_Set;

                }
                else if (tabla == Tabla.Ust_Tabla)
                {
                    RI_Start_Address = (int)DataBlockStartIndex.Read_Input_Ust_Tabla;
                    CS_Start_Address = (int)DataBlockStartIndex.Coil_Status_Ust_Tabla;
                    TM2_SET_Start_Address = (int)DataBlockStartIndex.Ust_Tabla_Set;
                }

                //DropShadowEffect dropShadowEffect = new DropShadowEffect();

                //BaseGrid.Effect = dropShadowEffect;

                int tm2_address = 0;
                for (int i = 0; i < 4; i++)
                {

                    for (int j = 0; j < 8; j += 2)
                    {

                        int index = TM2_SET_Start_Address + ((int)TM2_Byte_Len.TM2_Set_Byte_Len) * tm2_address;
                        moduls[tm2_address] = new UserControls.Module[2];

                        moduls[tm2_address][0] = new UserControls.Module();
                        moduls[tm2_address][0].Channel = 1;
                        moduls[tm2_address][0].txt_moduleNo.Text = "RES" + (i + 1) + "." + (j + 1);
                        moduls[tm2_address][0].Set_Value_Address = Tool.GenarateAddress(TypeCode.UInt16, 11, (index + (int)TM2_Set_UINT.CH1_SV));
                        moduls[tm2_address][0].Run_Stop_Address = Tool.GenarateAddress(TypeCode.Boolean, 11, (index + (int)TM2_Set_UINT.TM2_SET_BIT), (int)TM2_Set_Bit.CH1_STOP);
                        moduls[tm2_address][0].Tabla = this.Tabla;
                        moduls[tm2_address][0].No = (i * 8) + j;
                        Grid.SetRow(moduls[tm2_address][0], i);
                        Grid.SetColumn(moduls[tm2_address][0], j);
                        BaseGrid.Children.Add(moduls[tm2_address][0]);

                        moduls[tm2_address][1] = new UserControls.Module();
                        moduls[tm2_address][1].Channel = 2;
                        moduls[tm2_address][1].txt_moduleNo.Text = "RES" + (i + 1) + "." + (j + 2);
                        moduls[tm2_address][1].Set_Value_Address = Tool.GenarateAddress(TypeCode.UInt16, 11, (index + (int)TM2_Set_UINT.CH2_SV));
                        moduls[tm2_address][1].Run_Stop_Address = Tool.GenarateAddress(TypeCode.Boolean, 11, (index + (int)TM2_Set_UINT.TM2_SET_BIT), (int)TM2_Set_Bit.CH2_STOP);
                        moduls[tm2_address][1].Tabla = this.Tabla;
                        moduls[tm2_address][1].No = (i * 8) + j + 1; ;
                        Grid.SetRow(moduls[tm2_address][1], i);
                        Grid.SetColumn(moduls[tm2_address][1], j + 1);
                        BaseGrid.Children.Add(moduls[tm2_address][1]);

                        tm2_address++;

                    }
                }
            }
            main_plc = (S71200_Main)Main.PLCs[(int)StationName.AnaUnite];
            if (main_plc != null)
                main_plc.Updated += Ana_Unite_PLC_Uptdated;

            _Loaded = true;
        }

        private void BaseGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            main_plc.Updated -= Ana_Unite_PLC_Uptdated;
        }
    }
}
