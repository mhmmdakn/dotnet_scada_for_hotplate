using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace HotPlate.UserControls
{
    /// <summary>
    /// Interaction logic for Numarator.xaml
    /// </summary>
    public partial class Numarator_Flex : UserControl
    {
        public FrameworkElement FrameworkElement { get; set; }
        DispatcherTimer timer;
        public Numarator_Flex()
        {

            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            //timer.IsEnabled = true;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (upKey)
            {
                if (_UpKey != null)
                    _UpKey.Invoke(this, new EventArgs());
            }
            if (downKey)
            {
                if (_DownKey != null)
                    _DownKey.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler _Pressed;
        public event EventHandler _UpKey;
        public event EventHandler _DownKey;
        public event EventHandler _BackSpace;
        public event EventHandler _OK;
        public event EventHandler _Clear;

        bool downKey = false;
        bool upKey = false;


        public string Key { get; set; }

        private void btn_Num_Click(object sender, RoutedEventArgs e)
        {
            if (FrameworkElement != null)
            {
                Key = ((Button)sender).Content.ToString();

                if (FrameworkElement is TextBoxBase)
                {
                    var textBoxBase = (TextBox)FrameworkElement;
                    var start = textBoxBase.Text.Substring(0, textBoxBase.SelectionStart);
                    var end = textBoxBase.Text.Substring(textBoxBase.SelectionStart + textBoxBase.SelectionLength);
                    textBoxBase.Text = start + Key + end;
                    textBoxBase.SelectionStart = start.Length + 1;
                    textBoxBase.SelectionLength = 0;
                }
                else if (FrameworkElement is PasswordBox)
                {
                    var passwordBoxBase = (PasswordBox)FrameworkElement;
                    passwordBoxBase.Password = passwordBoxBase.Password + Key;
                }
            }



            if (_Pressed != null)
                _Pressed.Invoke(this, new EventArgs());


        }

        private void btn_Down_Click(object sender, RoutedEventArgs e)
        {
            if (_DownKey != null)
                _DownKey.Invoke(this, new EventArgs());
        }

        private void btn_Up_Click(object sender, RoutedEventArgs e)
        {
            if (_UpKey != null)
                _UpKey.Invoke(this, new EventArgs());
        }

        private void btn_backspace_Click(object sender, RoutedEventArgs e)
        {
            if (FrameworkElement != null)
            {
                if (FrameworkElement is TextBoxBase)
                {
                    var textBoxBase = (TextBox)FrameworkElement;
                    var start = textBoxBase.Text.Substring(0, textBoxBase.SelectionStart);
                    var end = textBoxBase.Text.Substring(textBoxBase.SelectionStart + textBoxBase.SelectionLength);
                    if (textBoxBase.SelectionLength > 0)
                    {
                        textBoxBase.Text = start + end;
                        textBoxBase.SelectionLength = 0;
                    }
                    else if (start.Length > 0)
                    {
                        textBoxBase.Text = start.Substring(0, start.Length - 1) + end;
                        textBoxBase.SelectionStart = start.Length - 1;
                        textBoxBase.SelectionLength = 0;
                    }
                }
                else if (FrameworkElement is PasswordBox)
                {
                    var passwordBoxBase = (PasswordBox)FrameworkElement;
                    if (passwordBoxBase.Password.Length > 0)
                        passwordBoxBase.Password = passwordBoxBase.Password.Substring(0, passwordBoxBase.Password.Length - 1);
                }
            }



            if (_BackSpace != null)
                _BackSpace.Invoke(this, new EventArgs());
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            if (_OK != null)
                _OK.Invoke(this, new EventArgs());
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            if (_Clear != null)
                _Clear.Invoke(this, new EventArgs());
        }


        private void btn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            upKey = true;
            downKey = false;
        }

        private void btn_MouseLeave(object sender, MouseEventArgs e)
        {
            timer.Stop();
            upKey = true;
            downKey = false;
        }

        private void btn_Down_MouseDown(object sender, MouseButtonEventArgs e)
        {
            timer.Start();
            downKey = true;
        }

        private void btn_Up_MouseDown(object sender, MouseButtonEventArgs e)
        {
            timer.Start();
            upKey = true;
        }
    }
}
