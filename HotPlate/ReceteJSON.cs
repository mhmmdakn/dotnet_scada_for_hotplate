using System;

namespace HotPlate
{
    public class ReceteJSON
    {
        public UInt16[] AT_SV { get; set; } = new UInt16[32];
        public UInt16[] UT_SV { get; set; } = new UInt16[32];
        public bool[] AT_RUN_STOP { get; set; }
        public bool[] UT_RUN_STOP { get; set; }

        public uint Basma_Suresi { get; set; } = 0;
        public float Baski_Offset { get; set; } = 0;
        public float Baski_Gucu { get; set; } = 0;
        public float Kalip_Yukseklik { get; set; } = 0;

        public ushort AT_PTol { get; set; }
        public ushort AT_NTol { get; set; }

        public ushort UT_PTol { get; set; }
        public ushort UT_NTol { get; set; }

        public uint Basma_Suresi_PTol { get; set; }
        public uint Basma_Suresi_NTol { get; set; }

        public float Baski_Offset_PTol { get; set; }
        public float Baski_Offset_NTol { get; set; }

        public float Baski_Gucu_PTol { get; set; }
        public float Baski_Gucu_NTol { get; set; }

        public float Kalip_Yukseklik_PTol { get; set; }
        public float Kalip_Yukseklik_NTol { get; set; }



    }
}
