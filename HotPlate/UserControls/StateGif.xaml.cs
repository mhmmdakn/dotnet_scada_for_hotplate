using Hot_Plate_Lib;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Threading;
namespace HotPlate
{
    /// <summary>
    /// Interaction logic for StateImage.xaml
    /// </summary>
    public partial class StateGif : PLC_BaseUserControl
    {
        DispatcherTimer timer = new DispatcherTimer();
        int i = 0;
        public StateGif()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (state)
            {
                if (image1.Count > i)
                    img_state.Source = image1[i].Source;

                else
                    i = -1;
            }
            else
            {

                if (image2.Count > i)
                    img_state.Source = image2[i].Source;
                else
                    i = -1;
            }

            i++;
        }

        private int interval = 1000;

        public int Interval
        {
            get { return interval; }
            set
            {
                interval = value;
                timer.Interval = TimeSpan.FromMilliseconds(interval);
            }
        }


        private List<Image> image1 = new List<Image>();

        public List<Image> Image1
        {
            get { return image1; }
            set
            {
                image1 = value;

            }
        }

        private List<Image> image2 = new List<Image>();

        public List<Image> Image2
        {
            get { return image2; }
            set
            {
                image2 = value;

            }
        }

        private string textTrue;

        public string TextTrue
        {
            get { return textTrue; }
            set { textTrue = value; }
        }

        private string textFalse;

        public string TextFalse
        {
            get { return textFalse; }
            set { textFalse = value; }
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

                    txt_state.Text = textTrue;
                }
                else
                {

                    txt_state.Text = textFalse;
                }
            }
        }

    }
}
