using Sharp7;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Threading;
namespace Hot_Plate_Lib
{


    public class S71200_Main : S71200
    {


        public event EventHandler AlarmStateChanged;
        public event EventHandler CurrentAlarmAdded;

        CultureInfo ci = new CultureInfo("tr-TR");

        public byte[] RECETE_buff = new byte[256];
        public byte[] TOLERANS_buff = new byte[128];



        public byte[] TM2_buff = new byte[2048];
        public byte[] TM2_Set_buff = new byte[256];

        public byte[] JD640_buff = new byte[512];
        public byte[] JD640_Set_buff = new byte[128];

        public byte[] Alarm_buff = new byte[256];

        public List<Alarm> Alarms { get; set; }
        public int AlarmDbNum { get; set; }

        public Dictionary<int, bool> currentAlarm_State = new Dictionary<int, bool>();

        public List<AlarmHistory> historyAlarms = new List<AlarmHistory>();

        private bool alarm_state = false;
        private bool alarm_state_temp = false;
        public bool AlarmState
        {
            get { return alarm_state; }
            set
            {
                alarm_state_temp = alarm_state;
                alarm_state = value;
                if (alarm_state != alarm_state_temp)
                {

                    if (AlarmStateChanged != null)
                        AlarmStateChanged.Invoke(this, new EventArgs());
                }




            }
        }

        public bool JD640_Read { get; set; }
        public bool TM2_Read { get; set; }
        public bool Alarm_Read { get; set; }

        public S71200_Main(string ip_address, Dispatcher dispatcher, StationName plcStationName, Dictionary<string, string> tags) : base(ip_address, dispatcher, plcStationName, tags)
        {


        }




        public override bool Check()
        {




            //Busy = true;

            if (Alarm_Read)
            {
                bool alarm_Durum = false;
                if (DBRead(AlarmDbNum, 0, 4, ref Alarm_buff, Sharp7.S7Consts.S7WLByte))
                    for (int i = 0; i < Alarms.Count; i++)
                    {

                        bool alarm_state = S7.GetBitAt(Alarm_buff, i / 8, i % 8);
                        if (currentAlarm_State.Count != Alarms.Count)
                            currentAlarm_State.Add(i, alarm_state);
                        else
                        {
                            if (currentAlarm_State[i] != alarm_state)
                            {
                                if (currentAlarm_State[i])
                                {
                                    historyAlarms.Add(new AlarmHistory(Alarms[i].Name, "Pasif", DateTime.Now.ToString("F", ci)));
                                }
                                else
                                {
                                    historyAlarms.Add(new AlarmHistory(Alarms[i].Name, "Aktif", DateTime.Now.ToString("F", ci)));
                                    if (Alarms[i].Go)
                                        if (CurrentAlarmAdded != null)
                                            CurrentAlarmAdded.Invoke(this, new EventArgs());
                                }

                                if (historyAlarms.Count > 50)
                                {
                                    historyAlarms.RemoveRange(1, 10);

                                }
                            }

                            currentAlarm_State[i] = alarm_state;
                        }


                        alarm_Durum |= alarm_state;


                    }
                AlarmState = alarm_Durum;

            }



            if (TM2_Read)
                DBRead(6, 0, (int)DataBlockSize.TM2, ref TM2_buff, Sharp7.S7Consts.S7WLByte);

            if (JD640_Read)
                DBRead(23, 0, (int)DataBlockSize.JD640, ref JD640_buff, Sharp7.S7Consts.S7WLByte);



            if (userControls != null)
            {
                //byte[][] multi_buff = new byte[256][];
                //for (int j = 0; j < userControls.Count; j++)
                //{
                //    multi_buff[j] = new byte[4];
                //}



                //// Multi Reader Instance
                //S7MultiVar Reader = new S7MultiVar(s7);

                //int i = 0;

                //foreach (PLC_BaseUserControl item in userControls)
                //{
                //    int area = 0, dbnum = 0, start_address = 0, bit_address = 0, word_len = 0;
                //    string address = item.PLCReadAdress;
                //    Tool.AddressCheck(ref address, Tags);

                //    Tool.SplitAddress(address, ref area, ref dbnum, ref start_address, ref bit_address, ref word_len);

                //    Reader.Add(area, word_len, dbnum, start_address, 8, ref multi_buff[i]);

                //    i++;
                //}
                //int Result = Reader.Read();




                foreach (PLC_BaseUserControl item in userControls)
                {

                    object value = Read(item.PLCReadAdress, item.DataType);
                    Dispatcher.Invoke(() =>
                    {
                        item.Value = value;

                    });
                }
            }


            // Busy = false;




            //else
            //    Busy = false;







            return true;
        }
    }
    public enum DataBlockSize
    {
        TM2 = 1856,
        TM2_Set = 192,
        JD640 = 312,
        JD640_Set = 160,
        RECETE = 158,
        TOLERANS = 40
    }

}
