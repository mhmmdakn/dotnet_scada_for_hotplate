namespace Hot_Plate_Lib
{
    public class PLC_Register
    {
        public int StartAddress { get; set; }
        public int Lenght { get; set; } = 2;
        public RegisterType RegisterType { get; set; }

    }

    public enum RegisterType
    {
        Byte,
        Bit
    }
}
