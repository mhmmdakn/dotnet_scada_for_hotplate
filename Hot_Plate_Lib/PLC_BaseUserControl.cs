using System;
using System.ComponentModel.DataAnnotations;

namespace Hot_Plate_Lib
{
    public class PLC_BaseUserControl : System.Windows.Controls.UserControl
    {

        public virtual string PLCReadAdress { get; set; }
        public virtual string PLCWriteAdress { get; set; }
        public virtual TypeCode DataType { get; set; } = TypeCode.UInt16;
        public virtual SecurityLevel SecurityLevel { get; set; } = SecurityLevel.No_Security;
        //public virtual string PLCOffsetAdress { get; set; }
        //public virtual bool OffsetRead { get; set; }
        //public virtual float Offset { get; set; }


        public virtual object Value { get; set; }

        public virtual StationName PLCStationName { get; set; } = StationName.AnaUnite;
    }
    public enum StationName
    {
        AnaUnite,
        Giris,
        Cikis

    }

    public enum SecurityLevel
    {
        [Display(Name = "No Security")]
        No_Security,

        [Display(Name = "Operator")]
        Operator,

        [Display(Name = "Supervisor")]
        Supervisor,

        [Display(Name = "Technical Service")]
        TechnicalService
    }

}
