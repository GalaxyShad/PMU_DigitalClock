using System;
using Xamarin.Forms;

namespace PMU_DigitalClock
{
    public partial class MainPage : ContentPage
    {
        private readonly DigitBox[] digitBoxes = new DigitBox[6];

        public MainPage()
        {
            InitializeComponent();

            AbsoluteLayout CreateDots(int x, int y)
            {
                var layout = new AbsoluteLayout();
                for (int i = 0; i < 2; i++)
                {
                    BoxView boxView = new BoxView();
                    boxView.BackgroundColor = Color.FromHex("#9FC131");
                    boxView.CornerRadius = 32;
                    layout.Children.Add(boxView, new Rectangle(x, y + i * 32, 8, 8));
                }
                return layout;
            }

            for (var i = 0; i < 6; i++)
            {
                var digitBox = new DigitBox();
                digitBoxes[i] = digitBox;
                clockLayout.Children.Add(digitBox);

                if (i % 2 == 1 && i < 5)
                {
                    var dots = CreateDots(0, 24);
                    dots.Margin = new Thickness(8, 0);
                    clockLayout.Children.Add(dots);
                }
            }

            Device.StartTimer(TimeSpan.FromSeconds(1), OnTimer);
            OnTimer();
        }

        bool OnTimer()
        {
            var dateTime = DateTime.Now.ToString("HHmmss");

            for (var i = 0; i < dateTime.Length; i++)
            {
                var digitBox = digitBoxes[i];
                var value = Convert.ToInt32(dateTime[i].ToString());
                digitBox.Set(value);
            }

            //digitBoxes[0].Set(dateTime.Hour / 10);
            //digitBoxes[1].Set(dateTime.Hour % 10);
            //digitBoxes[2].Set(dateTime.Minute / 10);
            //digitBoxes[3].Set(dateTime.Minute % 10);
            //digitBoxes[4].Set(dateTime.Second / 10);
            //digitBoxes[5].Set(dateTime.Second % 10);

            return true;
        }
    }


    public class DigitBox : AbsoluteLayout
    {
        readonly Color colOff = Color.FromHex("#005C53");
        readonly Color colOn = Color.FromHex("#DBF227");

        readonly int segThick = 8;
        readonly int segLen = 32;

        BoxView[] segments = new BoxView[7];

        readonly bool[,] digitPatterns = new bool[10, 7]
        {
            {true, true, true, true, true, true, false},        // 0
            {false, true, true, false, false, false, false},    // 1
            {true, true, false, true, true, false, true},       // 2
            {true, true, true, true, false, false, true},       // 3
            {false, true, true, false, false, true, true},      // 4
            {true, false, true, true, false, true, true},       // 5
            {true, false, true, true, true, true, true},        // 6
            {true, true, true, false, false, false, false},     // 7
            {true, true, true, true, true, true, true},         // 8
            {true, true, true, true, false, true, true},        // 9
        };

        public DigitBox()
        {
            for (int i = 0; i < 7; i++)
            {
                segments[i] = new BoxView
                {
                    BackgroundColor = colOff,
                    CornerRadius = 32
                };
            }

            Children.Add(segments[0],
                    new Rectangle(segThick, 0, segLen, segThick));
            Children.Add(segments[1],
                new Rectangle(segThick + segLen, segThick, segThick, segLen));
            Children.Add(segments[2],
                new Rectangle(segThick + segLen, segThick * 2 + segLen, segThick, segLen));
            Children.Add(segments[3],
                new Rectangle(segThick, segThick * 2 + segLen * 2, segLen, segThick));
            Children.Add(segments[4],
                new Rectangle(0, segThick * 2 + segLen, segThick, segLen));
            Children.Add(segments[5],
                new Rectangle(0, segThick, segThick, segLen));
            Children.Add(segments[6],
                new Rectangle(segThick, segThick + segLen, segLen, segThick));
        }

        public void Set(int digit)
        {
            if (digit < 0 || digit > 9)
                throw new ArgumentOutOfRangeException(nameof(digit));

            for (int i = 0; i < 7; i++)
            {
                segments[i].BackgroundColor = digitPatterns[digit, i] ? colOn : colOff;
            }
        }
    }
}
