using Sharp7;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Hot_Plate_Lib
{
    public class S71200 : IPLC
    {
        public bool Connected { get; set; }
        public event EventHandler Connecting_Fail;
        public event EventHandler Updated;
        public event EventHandler Writed;
        public List<PLC_BaseUserControl> userControls = new List<PLC_BaseUserControl>();
        public byte[] writeBuff = new byte[16];
        public byte[] readBuff = new byte[16];

        public bool Trying { get; set; }

        public bool Busy { get; set; }

        public Dictionary<string, string> Tags { get; set; }

        public StationName PLCStationName { get; set; }

        public S7Client s7Client;
        public S7Client s7_Read;
        public S7Client s7_DBRead;
        public S7Client s7_DBWrite;
        public S7Client s7_Write;

        DispatcherTimer timer_connect;
        DispatcherTimer timer_Task;
        Task[] task = new Task[1];
        int connection_try_count = 0;
        string ip_address;
        public Dispatcher Dispatcher { get; set; }

        public S71200(string ip, Dispatcher dispatcher, StationName plcStationName, Dictionary<string, string> tags)
        {

            ip_address = ip;
            PLCStationName = plcStationName;
            Dispatcher = dispatcher;
            Tags = tags;
            init();

        }


        public void init()
        {

            s7_Read = new S7Client();
            s7_Write = new S7Client();
            s7_DBRead = new S7Client();
            s7_DBWrite = new S7Client();
            s7Client = new S7Client();
            timer_connect = new DispatcherTimer();
            timer_connect.Interval = TimeSpan.FromMilliseconds(5000);
            timer_connect.IsEnabled = true;
            timer_connect.Tick += Timer_Tick;

            timer_Task = new DispatcherTimer();
            timer_Task.Interval = TimeSpan.FromMilliseconds(200);
            timer_Task.IsEnabled = true;
            timer_Task.Tick += Timer_Task_Tick;

            Ping myPing = new Ping();
            try
            {
                PingReply reply = myPing.Send(ip_address, 1000);
                if (reply.Status == IPStatus.Success)
                {
                    s7_Read.ConnectTo(ip_address, 0, 0);
                    s7_Write.ConnectTo(ip_address, 0, 0);
                    s7_DBRead.ConnectTo(ip_address, 0, 0);
                    s7_DBWrite.ConnectTo(ip_address, 0, 0);

                }
            }
            catch (Exception)
            {

                
            }
            


        }

        public void Connect_S7Client()
        {
            s7Client.ConnectTo(ip_address, 0, 0);
        }

        public void Disconnect_S7Client()
        {
            s7Client.Disconnect();
        }


        private async void Timer_Task_Tick(object sender, EventArgs e)
        {

            timer_Task.Stop();
            if (task[0] != null)
                await Task.WhenAll(task);
            task[0] = Task.Run(() =>
            {

                if (Connected)
                {
                    if (Check())
                    {
                        if (Updated != null)
                            Updated.Invoke(this, new EventArgs());
                    }

                }
            });

            timer_Task.Start();
        }
        public virtual bool Check()
        {
            bool result = true;

            if (userControls != null)
            {


                foreach (PLC_BaseUserControl item in userControls)
                {
                    object value = Read(item.PLCReadAdress, item.DataType);

                    Dispatcher.Invoke(() =>
                    {
                        item.Value = value;
                    });


                }
            }

            return result;
        }


        public async void DBWrite_Async(int DBNum, int start_index, int Size, byte[] Buffer)
        {

            if (task[0] != null)
                await Task.WhenAll(task);
            task[0] = Task.Run(() =>
            {
                if (Connected)
                {

                    int result = s7_DBWrite.DBWrite(DBNum, start_index, Size, Buffer);

                    if (result == 0)
                        if (Writed != null)
                            Writed.Invoke(this, new EventArgs());


                }
            });


        }
        public void DBWrite(int DBNum, int start_index, int Size, byte[] Buffer)
        {
            if (Connected)
            {

                int result = s7_DBWrite.DBWrite(DBNum, start_index, Size, Buffer);

                if (result == 0)
                    if (Writed != null)
                        Writed.Invoke(this, new EventArgs());

            }
        }

        public async void Write_Async(string address, object value, TypeCode typeCode)
        {


            int area = 0, dbnum = 0, start_address = 0, bit_address = 0, word_len = 0;

            Array.Clear(writeBuff, 0, writeBuff.Length);

            Tool.AddressCheck(ref address, Tags);

            Tool.SplitAddress(address, ref area, ref dbnum, ref start_address, ref bit_address, ref word_len);

            Tool.ObjectToByte(ref writeBuff, word_len, value, 0, typeCode, bit_address);


            if (task[0] != null)
                await Task.WhenAll(task);
            task[0] = Task.Run(() =>
            {
                if (Connected)
                {

                    int result = s7_Read.WriteArea(area, dbnum, start_address, 1, word_len, writeBuff);

                    if (result == 0)
                        if (Writed != null)
                            Writed.Invoke(this, new EventArgs());

                }
            });

        }

        public async void Read_Async(string address, Dispatcher dispatcher, PLC_BaseUserControl userControl)
        {
            object value = 0;
            int area = 0, dbnum = 0, start_address = 0, bit_address = 0, word_len = 0;

            Array.Clear(readBuff, 0, readBuff.Length);

            Tool.AddressCheck(ref address, Tags);

            Tool.SplitAddress(address, ref area, ref dbnum, ref start_address, ref bit_address, ref word_len);

            if (task[0] != null)
                await Task.WhenAll(task);
            task[0] = Task.Run(() =>
            {
                if (Connected)
                {
                    int result = s7_Read.ReadArea(area, dbnum, start_address, 1, word_len, readBuff);

                    Tool.ByteToObject(readBuff, word_len, ref value, 0, userControl.DataType, bit_address);
                    dispatcher.Invoke(() => { userControl.Value = value; });


                }
            });


        }


        public void Write(string address, object value, TypeCode typeCode)
        {


            int area = 0, dbnum = 0, start_address = 0, bit_address = 0, word_len = 0;

            Array.Clear(writeBuff, 0, writeBuff.Length);

            Tool.AddressCheck(ref address, Tags);

            Tool.SplitAddress(address, ref area, ref dbnum, ref start_address, ref bit_address, ref word_len);

            Tool.ObjectToByte(ref writeBuff, word_len, value, 0, typeCode, bit_address);
            if (Connected)
            {
                int result = s7_Write.WriteArea(area, dbnum, start_address, 1, word_len, writeBuff);
            }



        }
        public object Read(string _address, TypeCode typeCode)
        {
            object value = 0;
            int area = 0, dbnum = 0, start_address = 0, bit_address = 0, word_len = 0;
            string address = _address;
            Array.Clear(readBuff, 0, readBuff.Length);

            Tool.AddressCheck(ref address, Tags);

            Tool.SplitAddress(address, ref area, ref dbnum, ref start_address, ref bit_address, ref word_len);

            if (Connected)
            {
                int result = s7_Read.ReadArea(area, dbnum, start_address, 1, word_len, readBuff);
                Tool.ByteToObject(readBuff, word_len, ref value, 0, typeCode, bit_address);
            }
            return value;
        }

        public object Read(string _address, TypeCode typeCode, S7Client s7Client)
        {
            object value = 0;
            int area = 0, dbnum = 0, start_address = 0, bit_address = 0, word_len = 0, size = 0;
            string address = _address;
            Array.Clear(readBuff, 0, readBuff.Length);

            Tool.AddressCheck(ref address, Tags);

            Tool.SplitAddress(address, ref area, ref dbnum, ref start_address, ref bit_address, ref word_len);

            if (Connected)
            {
                int result = s7Client.ReadArea(area, dbnum, start_address, 1, word_len, readBuff);
                Tool.ByteToObject(readBuff, word_len, ref value, 0, typeCode, bit_address);
            }
            return value;
        }

        public bool DBRead(int DBNumber, int start, int size, ref byte[] buffer, int WordLen)
        {
            int result = -1;
            if (Connected)
            {
                result = s7_Read.ReadArea(S7Consts.S7AreaDB, DBNumber, start, size, WordLen, buffer);

            }

            if (result != 0)
                return false;

            return true;
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            if (s7_Read.Connected == false)
            {
                if (connection_try_count < 3)
                {
                    Trying = false;

                    try
                    {
                        Ping myPing = new Ping();
                        PingReply reply = myPing.Send(ip_address, 1000);
                        if (reply.Status == IPStatus.Success)
                        {

                            s7_Read.Disconnect();
                            s7_Write.Disconnect();
                            s7_DBRead.Disconnect();
                            s7_DBWrite.Disconnect();

                            s7_Read.ConnectTo(ip_address, 0, 0);
                            s7_Write.ConnectTo(ip_address, 0, 0);
                            s7_DBRead.ConnectTo(ip_address, 0, 0);
                            s7_DBWrite.ConnectTo(ip_address, 0, 0);
                            connection_try_count = 0;
                        }
                        else
                            connection_try_count++;
                    }
                    catch (Exception)
                    {
                        connection_try_count++;

                    }

                    
                   

                    
                }
                else if (Trying == true)
                {
                    connection_try_count = 0;
                }
                else
                {
                    if (Connecting_Fail != null)
                        Connecting_Fail.Invoke(this, new EventArgs());
                }

                Connected = false;
            }
            else
                Connected = true;
        }


        public async void UpdateUserControlList(List<PLC_BaseUserControl> myBaseUserControls)
        {
            if (task[0] != null)
                await Task.WhenAll(task);
            userControls.Clear();

            foreach (PLC_BaseUserControl item in myBaseUserControls)
            {
                if (item.PLCStationName == PLCStationName)
                {
                    userControls.Add(item);
                }
            }

        }

        void DenemeWrite()
        {
            //byte[] a = new byte[5];
            //s7.ABWrite(0)
        }


    }
}

