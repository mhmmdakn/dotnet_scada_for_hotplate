using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace Hot_Plate_Lib
{
    public interface IPLC
    {
        Dictionary<string, string> Tags { get; set; }

        void Read_Async(string address, Dispatcher dispatcher, PLC_BaseUserControl userControl);
        void Write_Async(string address, object value, TypeCode typeCode);

        object Read(string address, TypeCode typeCode);
        void Write(string address, object value, TypeCode typeCode);
    }
}
