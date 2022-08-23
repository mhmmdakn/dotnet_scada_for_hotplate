using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;


namespace Hot_Plate_Lib
{
    public class ReceteParam : NotificationObject, IEditableObject
    {
        public int id { get; set; }
        public string Name { get; set; }
        object _value;
        public object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                this.RaisePropertyChanged("Value");
            }
        }

        public float Alt_Limit { get; set; }
        public float Ust_Limit { get; set; }




        protected Dictionary<string, object> BackUp()
        {
            var dict = new Dictionary<string, object>();
            var itemProperties = this.GetType().GetTypeInfo().DeclaredProperties;

            foreach (var pDescriptor in itemProperties)
            {

                if (pDescriptor.CanWrite)
                    dict.Add(pDescriptor.Name, pDescriptor.GetValue(this));
            }
            return dict;
        }

        private Dictionary<string, object> storedValues;

        public void BeginEdit()
        {
            this.storedValues = this.BackUp();
        }

        public void CancelEdit()
        {

            if (this.storedValues == null)
                return;

            foreach (var item in this.storedValues)
            {
                var itemProperties = this.GetType().GetTypeInfo().DeclaredProperties;
                var pDesc = itemProperties.FirstOrDefault(p => p.Name == item.Key);

                if (pDesc != null)
                    pDesc.SetValue(this, item.Value);
            }
        }

        public void EndEdit()
        {

            if (this.storedValues != null)
            {
                this.storedValues.Clear();
                this.storedValues = null;
            }
            Debug.WriteLine("End Edit Called");
        }
    }

    public class NotificationObject : INotifyPropertyChanged
    {
        public void RaisePropertyChanged(string propName)
        {

            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
