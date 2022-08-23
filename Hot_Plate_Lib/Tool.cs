using Sharp7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Hot_Plate_Lib
{
    public class Tool
    {

        public static string ByteToString(byte[] array_buff, int index)
        {
            int size = (int)array_buff[index + 1];
            return Encoding.UTF8.GetString(array_buff, index + 2, size);
        }
        public static void StringToByte(ref byte[] array_buff, int index, int MaxLen, string value)
        {

            int size = value.Length;
            array_buff[index] = (byte)MaxLen;
            array_buff[index + 1] = (byte)size;
            Encoding.UTF8.GetBytes(value, 0, size, array_buff, index + 2);
        }

        public static UInt32 ByteToUDInt(byte[] array_buff, int index)
        {
            UInt32 var = 0;
            byte[] temp = { array_buff[index + 3], array_buff[index + 2], array_buff[index + 1], array_buff[index] };

            var = BitConverter.ToUInt32(temp, 0);

            return var;
        }
        public static Single ByteToReal(byte[] array_buff, int index)
        {
            Single var = 0;
            byte[] temp = { array_buff[index + 3], array_buff[index + 2], array_buff[index + 1], array_buff[index] };

            var = BitConverter.ToSingle(temp, 0);

            return var;
        }
        public static Int32 ByteToDInt(byte[] array_buff, int index)
        {

            return (Int32)ByteToUDInt(array_buff, index);
        }
        public static UInt16 ByteToUInt(byte[] array_buff, int index)
        {
            UInt16 var = 0;
            byte[] temp = { array_buff[index + 1], array_buff[index] };

            var = BitConverter.ToUInt16(temp, 0);

            return var;
        }
        public static Int16 ByteToInt(byte[] array_buff, int index)
        {
            return (Int16)ByteToUInt(array_buff, index);
        }
        public static bool ByteToBit(byte[] array_buff, int index, int bit)
        {
            return S7.GetBitAt(array_buff, index, bit);
        }
        public static bool ByteToBit(byte[] array_buff)
        {
            return Convert.ToBoolean(array_buff[0]);
        }
        public static void UDintToByte(ref byte[] array_buff, UInt32 value, int index)
        {

            byte[] temp = BitConverter.GetBytes(value);
            array_buff[index] = temp[3];
            array_buff[index + 1] = temp[2];
            array_buff[index + 2] = temp[1];
            array_buff[index + 3] = temp[0];

        }
        public static void DintToByte(ref byte[] array_buff, Int32 value, int index)
        {

            byte[] temp = BitConverter.GetBytes(value);
            array_buff[index] = temp[3];
            array_buff[index + 1] = temp[2];
            array_buff[index + 2] = temp[1];
            array_buff[index + 3] = temp[0];

        }
        public static void RealToByte(ref byte[] array_buff, Single value, int index)
        {

            byte[] temp = BitConverter.GetBytes(value);
            array_buff[index] = temp[3];
            array_buff[index + 1] = temp[2];
            array_buff[index + 2] = temp[1];
            array_buff[index + 3] = temp[0];

        }

        public static void UintToByte(ref byte[] array_buff, UInt16 value, int index)
        {

            byte[] temp = BitConverter.GetBytes(value);
            array_buff[index] = temp[1];
            array_buff[index + 1] = temp[0];

        }
        public static void IntToByte(ref byte[] array_buff, Int16 value, int index)
        {

            byte[] temp = BitConverter.GetBytes(value);
            array_buff[index] = temp[1];
            array_buff[index + 1] = temp[0];

        }
        public static void BitToByte(ref byte[] array_buff, bool value, int index, int bit)
        {
            S7.SetBitAt(ref array_buff, index, bit, value);

        }

        public static string GenarateAddress(TypeCode typeCode, int DBNum, int start_address, int bit_address = 0)
        {
            string address = "%DB" + DBNum + ".";

            switch (typeCode)
            {
                case TypeCode.Empty:
                    break;
                case TypeCode.Object:
                    break;
                case TypeCode.DBNull:
                    break;
                case TypeCode.Boolean:
                    address += "DBX" + start_address + "." + bit_address;
                    break;
                case TypeCode.Char:
                    break;
                case TypeCode.SByte:
                    break;
                case TypeCode.Byte:
                    address += "DBB" + start_address;
                    break;
                case TypeCode.Int16:
                    address += "DBW" + start_address;
                    break;
                case TypeCode.UInt16:
                    address += "DBW" + start_address;
                    break;
                case TypeCode.Int32:
                    address += "DBD" + start_address;
                    break;
                case TypeCode.UInt32:
                    address += "DBD" + start_address;
                    break;
                case TypeCode.Int64:
                    break;
                case TypeCode.UInt64:
                    break;
                case TypeCode.Single:
                    address += "DBD" + start_address;
                    break;
                case TypeCode.Double:
                    break;
                case TypeCode.Decimal:
                    break;
                case TypeCode.DateTime:
                    break;
                case TypeCode.String:
                    break;
                default:
                    break;
            }

            return address;
        }

        public static void ObjectToByte(ref byte[] buffer, int wordLen, object value, int index, TypeCode typeCode, int bit = 0)
        {

            if (wordLen == S7Consts.S7WLBit)
                BitToByte(ref buffer, Convert.ToBoolean(value), index, bit);
            else if (wordLen == S7Consts.S7WLWord)
            {
                if (typeCode == TypeCode.Int16)
                    IntToByte(ref buffer, Convert.ToInt16(value), index);
                else
                    UintToByte(ref buffer, Convert.ToUInt16(value), index);


            }

            else if (wordLen == S7Consts.S7WLDWord)
            {

                if (typeCode == TypeCode.Int32)
                    DintToByte(ref buffer, Convert.ToInt32(value), index);
                else if (typeCode == TypeCode.Single)
                    RealToByte(ref buffer, Convert.ToSingle(value), index);
                else
                    UDintToByte(ref buffer, Convert.ToUInt32(value), index);

            }
            else if (wordLen == S7Consts.S7WLByte)
            {
                if (typeCode == TypeCode.String)
                    StringToByte(ref buffer, index, 11, value.ToString());
            }


        }
        public static void ByteToObject(byte[] buffer, int wordLen, ref object value, int index, TypeCode typeCode, int bit = 0)
        {

            if (wordLen == S7Consts.S7WLBit)
                value = ByteToBit(buffer);
            else if (wordLen == S7Consts.S7WLWord)
            {
                if (typeCode == TypeCode.Int16)
                    value = ByteToInt(buffer, index);
                else
                    value = ByteToUInt(buffer, index);
            }

            else if (wordLen == S7Consts.S7WLDWord)
            {
                if (typeCode == TypeCode.Int32)
                    value = ByteToDInt(buffer, index);
                else if (typeCode == TypeCode.Single)
                    value = ByteToReal(buffer, index);
                else
                    value = ByteToUDInt(buffer, index);

            }
            else if (wordLen == S7Consts.S7WLByte)
            {
                if (typeCode == TypeCode.String)
                    value = ByteToString(buffer, index);
            }


        }
        public static void SplitAddress(string address, ref int Area, ref int DBNum, ref int start_address, ref int bit_address, ref int wordLen)
        {

            int dbnum_start_index = 0;
            int startadress_start_index = 0;

            if (address.IndexOf("%DB") != -1)
            {
                dbnum_start_index = 3;
                Area = S7Consts.S7AreaDB;
            }
            if (address.IndexOf("P#DB") != -1)
            {
                dbnum_start_index = 4;
                Area = S7Consts.S7AreaDB;
            }
            else if (address.IndexOf("%") != -1)
            {
                DBNum = 0;
                wordLen = S7Consts.S7WLBit;//?????? S7WLByte
                startadress_start_index = 2;
                string register_type = address.Substring(1, 1);
                switch (register_type)
                {
                    case "M":
                        Area = S7Consts.S7AreaMK;
                        break;
                    case "I":
                        Area = S7Consts.S7AreaPE;
                        break;
                    case "Q":
                        Area = S7Consts.S7AreaPA;
                        break;

                }

                if (address.IndexOf("%IW") != -1)
                {
                    startadress_start_index = 3;
                    wordLen = S7Consts.S7WLWord;

                }

            }




            if (dbnum_start_index == 3)
            {
                int dbnum_last_index = address.IndexOf('.');
                DBNum = int.Parse(address.Substring(dbnum_start_index, dbnum_last_index - dbnum_start_index));

                int type_start_index = dbnum_last_index + 1;
                int type_len = 3;
                int type_last_index = type_start_index + type_len;

                string type = address.Substring(type_start_index, type_len);

                if (type == "DBW")
                    wordLen = S7Consts.S7WLWord;
                else if (type == "DBD")
                    wordLen = S7Consts.S7WLDWord;
                else if (type == "DBB")
                    wordLen = S7Consts.S7WLByte;
                else if (type == "DBX")
                    wordLen = S7Consts.S7WLBit;
                else
                    wordLen = S7Consts.S7WLByte;

                startadress_start_index = type_last_index;


            }
            else if (dbnum_start_index == 4)
            {
                int dbnum_last_index = address.IndexOf('.');
                DBNum = int.Parse(address.Substring(dbnum_start_index, dbnum_last_index - dbnum_start_index));

                int type_start_index = dbnum_last_index + 1;
                int type_len = 3;
                int type_last_index = type_start_index + type_len;

                string type = address.Substring(type_start_index, type_len);
                if (type == "DBX")
                    wordLen = S7Consts.S7WLByte;

                startadress_start_index = type_last_index;
            }

            int startadress_last_index = address.Length;


            string[] StartAddress = address.Substring(startadress_start_index, startadress_last_index - startadress_start_index).Split('.');

            start_address = int.Parse(StartAddress[0]);

            if (StartAddress.Length > 1)
                bit_address = int.Parse(StartAddress[1]);

            if (wordLen == S7Consts.S7WLBit)
            {
                start_address = (start_address * 8) + bit_address;
            }

        }


        public static void PageControlList(DependencyObject dependency, int index, ref List<PLC_BaseUserControl>[] myBaseUserControls)
        {
            myBaseUserControls[index] = new List<PLC_BaseUserControl>();
            FindChild<PLC_BaseUserControl>(dependency, myBaseUserControls[index]);

        }
        public static void FindChild<T>(DependencyObject parent, List<PLC_BaseUserControl> userControls)
         where T : PLC_BaseUserControl
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    FindChild<T>(child, userControls);
                    // If the child is found, break so we do not overwrite the found child. 

                }

                else
                {
                    // child element found.
                    userControls.Add((T)child);

                }
            }


        }

        public static string AddressCheck(ref string address, Dictionary<string, string> Tags)
        {


            if (address[0] != '%' && address[0] != '#')
            {
                try
                {
                    address = Tags[address];

                }
                catch (Exception)
                {


                }
            }
            return "";

        }

        public static bool TagGetir(ref Dictionary<string, string> tags, string FileName)
        {
            StreamReader reader = new StreamReader(FileName);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string tagName = line.Split(';')[0];
                string address = line.Split(';')[1];
                tags.Add(tagName, address);
            }
            return true;
        }



    }

}
