using System.Windows.Controls;

namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for Error_List_Item.xaml
    /// </summary>
    public partial class Error_List_Item : UserControl
    {
        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                lbl_Text.Text = text;
            }
        }
        private bool state;

        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                sta_Led.State = state;
            }
        }

        public Error_List_Item()
        {
            InitializeComponent();
        }
    }
}
