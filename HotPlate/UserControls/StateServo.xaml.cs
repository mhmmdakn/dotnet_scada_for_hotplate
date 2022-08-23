using Hot_Plate_Lib;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for StateServo.xaml
    /// </summary>
    public partial class StateServo : UserControl
    {
        DispatcherTimer timer;
        S71200_Main main_plc = (S71200_Main)Main.PLCs[(int)StationName.AnaUnite];
        public StateServo()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;
        }



        public int ServoNum { get; set; }


        private bool enable;

        public bool Enable
        {
            get { return enable; }
            set
            {
                enable = value;
                sta_Enable.State = enable;
                State = enable;
            }
        }

        private bool ready;

        public bool Ready
        {
            get { return ready; }
            set
            {
                ready = value;
                sta_Ready.State = ready;
                if (Ready)
                {
                    sta_Genel.Text = "HAZIR";
                    sta_Genel.Background = (Brush)(new BrushConverter().ConvertFrom("#FF00CF00"));

                }
            }
        }

        private bool error;

        public bool Error
        {
            get { return error; }
            set
            {
                error = value;
                sta_Error.State = error;
                if (!Ready)
                {
                    sta_Genel.Text = "HATA";
                    sta_Genel.Background = (Brush)(new BrushConverter().ConvertFrom("#FFCF0000"));

                }
                else
                {
                    sta_Genel.Text = "HAZIR";
                    sta_Genel.Background = (Brush)(new BrushConverter().ConvertFrom("#FF00CF00"));
                }
            }
        }

        private bool pos_reached;

        public bool Pos_Reached
        {
            get { return pos_reached; }
            set
            {
                pos_reached = value;
                sta_PosReached.State = pos_reached;
            }
        }




        private int i_q;

        public int I_q
        {
            get { return i_q; }
            set
            {
                i_q = value;
                txt_I_q.Text = (i_q / JD640_Const.I_q).ToString("0.##") + "A";
                //txt_I_q.Text = "abc";

            }
        }

        private int pos_actual;

        public int Pos_Actual
        {
            get { return pos_actual; }
            set
            {
                pos_actual = value;
                txt_Pos_Actual.Text = (pos_actual / JD640_Const.Pos_Actual).ToString("0.###") + "mm";
            }
        }

        private int pos_error;

        public int Pos_Error
        {
            get { return pos_error; }
            set
            {
                pos_error = value;
                txt_Pos_Error.Text = (pos_error / JD640_Const.Pos_Error).ToString("0.###") + "mm";

            }
        }


        private int speed;
        public int Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                txt_Speed.Text = (speed).ToString("0") + "rpm";
            }
        }

        private bool state;

        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                if (state)
                    timer.IsEnabled = true;
                else if (!state)
                    timer.IsEnabled = false;
            }
        }







        ScaleTransform scaleTransform = new ScaleTransform();
        private bool flipXAxis = false;

        public bool FlipXAxis
        {
            get { return flipXAxis; }
            set
            {
                flipXAxis = value;
                if (flipXAxis)
                {
                    scaleTransform.ScaleX = -1;
                    scaleTransform.ScaleY = 1;
                }
                if (!flipXAxis)
                {
                    scaleTransform.ScaleX = 1;
                    scaleTransform.ScaleY = 1;
                }
                if (flipXAxis && FlipYAxis)
                {
                    scaleTransform.ScaleX = -1;
                    scaleTransform.ScaleY = -1;
                }
                info_servo.RenderTransform = scaleTransform;
                servo.RenderTransform = scaleTransform;
                status_servo.RenderTransform = scaleTransform;

            }
        }

        private bool flipYAxis;

        public bool FlipYAxis
        {
            get { return flipYAxis; }
            set
            {
                flipYAxis = value;
                if (FlipYAxis)
                {
                    scaleTransform.ScaleX = 1;
                    scaleTransform.ScaleY = -1;
                }
                if (!FlipYAxis)
                {
                    scaleTransform.ScaleX = 1;
                    scaleTransform.ScaleY = 1;
                }
                if (flipXAxis && FlipYAxis)
                {
                    scaleTransform.ScaleX = -1;
                    scaleTransform.ScaleY = -1;
                }
                info_servo.RenderTransform = scaleTransform;
                servo.RenderTransform = scaleTransform;
                status_servo.RenderTransform = scaleTransform;

            }
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            Rotary();
        }

        double i;
        RotateTransform rotateTransform = new RotateTransform(0);
        private void Rotary()
        {
            rotateTransform.Angle = i;
            servoMil.RenderTransform = rotateTransform;
            servoMil2.RenderTransform = rotateTransform;
            if (i == 360)
            {
                i = 0;
            }
            i += speed / 12;
        }

        private void BaseGrid_Loaded(object sender, RoutedEventArgs e)
        {
            main_plc = (S71200_Main)Main.PLCs[(int)StationName.AnaUnite];
            if (main_plc != null)
                main_plc.Updated += Ana_Unite_PLC_Uptdated;
        }

        private void Ana_Unite_PLC_Uptdated(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                int servo_read_index = ((int)DataBlockStartIndex.Servo_Read) + ((int)JD640_Byte_Len.Servo_Read * (ServoNum - 1));
                int servo_set_read_index = ((int)DataBlockStartIndex.Servo_Set_Read) + ((int)JD640_Byte_Len.Servo_Set_Read * (ServoNum - 1));
                I_q = Tool.ByteToInt(main_plc.JD640_buff, servo_read_index + (int)Servo_Read.I_q_Int);
                Pos_Error = Tool.ByteToDInt(main_plc.JD640_buff, servo_read_index + (int)Servo_Read.Pos_Error_DInt);
                Pos_Actual = Tool.ByteToDInt(main_plc.JD640_buff, servo_read_index + (int)Servo_Read.Pos_Actual_DInt);
                Speed = Tool.ByteToInt(main_plc.JD640_buff, servo_read_index + (int)Servo_Read.Speed_RPM_Int);
                Enable = Tool.ByteToBit(main_plc.JD640_buff, servo_read_index + (int)Servo_Read.Dout_Status, (int)Dout_Status.Drive_Enabled);
                Ready = Tool.ByteToBit(main_plc.JD640_buff, servo_read_index + (int)Servo_Read.Dout_Status, (int)Dout_Status.Ready);
                Error = !(Tool.ByteToBit(main_plc.JD640_buff, servo_read_index + (int)Servo_Read.Dout_Status, (int)Dout_Status.Error));
                Pos_Reached = Tool.ByteToBit(main_plc.JD640_buff, servo_read_index + (int)Servo_Read.Dout_Status, (int)Dout_Status.Pos_Reached);
            });



        }

        private void BaseGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            main_plc = (S71200_Main)Main.PLCs[(int)StationName.AnaUnite];
            if (main_plc != null)
                main_plc.Updated -= Ana_Unite_PLC_Uptdated;
        }
    }
}
