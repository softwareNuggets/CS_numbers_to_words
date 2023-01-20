using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace NumberToWordsV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Media.Color Orange = Color.FromRgb(255, 127, 0);
        System.Windows.Media.Color LightOrange = Color.FromRgb(252, 180, 109);

        public MainWindow()
        {
            InitializeComponent();

            this.synthsizer = null;
            this.voiceList = null;

            SetVoiceRates();

            bool rv = LoadInstalledVoices();
        }



        private void ConvertToString_Click(object sender, RoutedEventArgs e)
        {
            ConvertNumberToString scn = new ConvertNumberToString();

            string justNumbers = scn.OnlyNumbers(this.InputNumber.Text.ToString());

            if ((justNumbers == "-" || justNumbers == "nothing to convert") == false)
            {
                this.InputNumber.Text = justNumbers;
                this.TbFormattedNumber.Text = scn.FormatNumbersWithCommas(justNumbers);

                Int128 number = 0;
                try
                {
                    number = Int128.Parse(justNumbers);
                }
                catch
                {
                    return;
                }
                string s = scn.Convert(number);
                NumberInWords.Text = s;
            }
            else
            {
                MessageBox.Show("Invalid Number to convert");
            }
        }


        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.Clear();
            NumberInWords.Clear();
            AnimateBackGroundColor(NumberInWords, Colors.LightGray, Colors.White);
        }

        private void SmallFontMouseDown(object sender, MouseButtonEventArgs e)
        {
            var fontsize = this.NumberInWords.FontSize;
            fontsize -= 2;

            if (fontsize < 7)
                fontsize = 7;
            this.NumberInWords.FontSize = fontsize;
            AnimateBackGroundColor(NumberInWords, LightOrange, Colors.White);
        }

        private void BigFontMouseDown(object sender, MouseButtonEventArgs e)
        {
            var fontsize = this.NumberInWords.FontSize;
            fontsize += 2;

            if (fontsize > 40)
                fontsize = 40;

            this.NumberInWords.FontSize = fontsize;
            AnimateBackGroundColor(NumberInWords, Orange, Colors.White);

        }

        private void BtnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Clipboard.SetText(NumberInWords.Text);

            AnimateBackGroundColor(NumberInWords, Colors.LightSkyBlue, Colors.White);
        }

        private void AnimateBackGroundColor(System.Windows.Controls.TextBox tb,
                    Color from, Color to)
        {
            ColorAnimation animation;
            animation = new ColorAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = new Duration(TimeSpan.FromSeconds(.9));
            tb.Background = new SolidColorBrush(from);
            tb.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

        }

        private void SayNumber_Click(object sender, RoutedEventArgs e)
        {
            if (synthsizer != null)
            {
                synthsizer.Dispose();
            }

            if (NumberInWords.Text.ToString().Trim().Length == 0)
            {
                MessageBox.Show("Please provide some text in the green input box to continue");
                return;
            }

            InitializeSynthsizer();

            if (synthsizer != null)
            {
                synthsizer.SpeakAsync(this.NumberInWords.Text);
            }

            this.cboVoices.IsEnabled = false;
            this.cboRate.IsEnabled = false;
        }

    }
}