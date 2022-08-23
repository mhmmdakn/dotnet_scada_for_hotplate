using Hot_Plate_Lib;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for Servo_Error_List.xaml
    /// </summary>
    public partial class Servo_Error_List : UserControl
    {



        public int ServoNum { get; set; }
        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                lbl_Title.Content = title;
            }
        }

        private ushort error_state_1;

        public ushort Error_State_1
        {
            get { return error_state_1; }
            set
            {
                error_state_1 = value;
                string binary = Convert.ToString(error_state_1, 2);
                binary = binary.PadLeft(16, '0');

                int i = 0;
                foreach (Error_List_Item item in lst_Error_State_1.Items)
                {
                    item.State = Convert.ToBoolean(int.Parse(binary[15 - i++].ToString()));
                }
            }
        }


        private ushort error_state_2;

        public ushort Error_State_2
        {
            get { return error_state_2; }
            set
            {
                error_state_2 = value;
                string binary = Convert.ToString(error_state_2, 2);
                binary = binary.PadLeft(16, '0');

                int i = 0;
                foreach (Error_List_Item item in lst_Error_State_2.Items)
                {
                    item.State = Convert.ToBoolean(int.Parse(binary[15 - i++].ToString()));
                }
            }
        }


        S71200_Main main_plc;

        public Servo_Error_List()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                main_plc = (S71200_Main)Main.PLCs[(int)StationName.AnaUnite];
                if (main_plc != null)
                    main_plc.Updated += Ana_Unite_PLC_Uptdated;
            }
        }

        private void Ana_Unite_PLC_Uptdated(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                int servo_read_index = ((int)DataBlockStartIndex.Servo_Read) + ((int)JD640_Byte_Len.Servo_Read * (ServoNum - 1));
                int servo_set_read_index = ((int)DataBlockStartIndex.Servo_Set_Read) + ((int)JD640_Byte_Len.Servo_Set_Read * (ServoNum - 1));
                Error_State_1 = Tool.ByteToUInt(main_plc.JD640_buff, servo_read_index + (int)Servo_Read.Error_State_1_UInt);
                Error_State_2 = Tool.ByteToUInt(main_plc.JD640_buff, servo_read_index + (int)Servo_Read.Error_State_2_UInt);

            });
        }
    }
}
