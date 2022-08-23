using Hot_Plate_Lib;
using System;
using System.Windows;
namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for State_Gif_Media.xaml
    /// </summary>
    public partial class State_Gif_Media : PLC_BaseUserControl
    {
        public State_Gif_Media()
        {
            InitializeComponent();
        }

        private void gif_loading_MediaEnded(object sender, RoutedEventArgs e)
        {
            gif_loading.Position = TimeSpan.FromMilliseconds(1);
            if (sta_Gif_Media.Visibility == Visibility.Visible)
                gif_loading.Play();
            else if (sta_Gif_Media.Visibility == Visibility.Hidden)
                gif_loading.Pause();
        }

        private object _value;

        public override object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                State = (bool)_value;
            }
        }

        private bool state;

        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                if (state)
                {

                    sta_Gif_Media.Visibility = Visibility.Visible;
                }
                else
                {

                    sta_Gif_Media.Visibility = Visibility.Hidden;

                }
            }
        }
    }
}
