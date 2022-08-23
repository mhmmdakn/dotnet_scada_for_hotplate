using System.Windows.Media;

namespace Hot_Plate_Lib
{
    public class Alarm
    {

        public string Name { get; set; }
        public Color Color { get; set; }
        public string Description { get; set; }
        public bool Go { get; set; }

        public Alarm(string name, Color color, string description, bool go)
        {
            Name = name;
            Color = color;

            Description = description;
            Go = go;
        }

    }
    public class AlarmHistory
    {

        public string Name { get; set; }
        public string Date { get; set; }
        public string State { get; set; }


        public AlarmHistory(string name, string state, string date)
        {
            Name = name;
            State = state;
            Date = date;

        }

    }
}
