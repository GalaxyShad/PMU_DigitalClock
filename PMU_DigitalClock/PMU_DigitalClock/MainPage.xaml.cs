using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PMU_DigitalClock
{
    public partial class MainPage : ContentPage
    {
        static DigitBox[] digitBoxes = new DigitBox[6];
        public MainPage()
        {
            InitializeComponent();

            for (int i = 0; i < 6; i++)
            {
                digitBoxes[i] = new DigitBox(clockLayout, i * 64 - (8 * (i % 2)), 0);
                digitBoxes[i].Set(0);
            }

            CreateDots(112, 24);
            CreateDots(240, 24);

            Device.StartTimer(TimeSpan.FromSeconds(1), OnTimer);
            OnTimer();

        }

        bool OnTimer()
        {
            DateTime dateTime = DateTime.Now;

            digitBoxes[0].Set(dateTime.Hour / 10);
            digitBoxes[1].Set(dateTime.Hour % 10);
            digitBoxes[2].Set(dateTime.Minute / 10);
            digitBoxes[3].Set(dateTime.Minute % 10);
            digitBoxes[4].Set(dateTime.Second / 10);
            digitBoxes[5].Set(dateTime.Second % 10);

            return true;
        }

        void CreateDots(int x, int y)
        {
            for (int i = 0; i < 2; i++)
            {
                BoxView boxView = new BoxView();
                boxView.BackgroundColor = Color.FromHex("#9FC131");
                boxView.CornerRadius = 32;
                clockLayout.Children.Add(boxView, new Rectangle(x, y + i * 32, 8, 8));
            }
        }
    }


    public class DigitBox
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

        public DigitBox(AbsoluteLayout layout, int x, int y)
        {
            for (int i = 0; i < 7; i++)
            {
                segments[i] = new BoxView();
                segments[i].BackgroundColor = colOff;
                segments[i].CornerRadius = 32;
            }

            layout.Children.Add(segments[0],
                    new Rectangle(x + segThick, y, segLen, segThick));
            layout.Children.Add(segments[1],
                new Rectangle(x + segThick + segLen, y + segThick, segThick, segLen));
            layout.Children.Add(segments[2],
                new Rectangle(x + segThick + segLen, y + segThick * 2 + segLen, segThick, segLen));
            layout.Children.Add(segments[3],
                new Rectangle(x + segThick, y + segThick * 2 + segLen * 2, segLen, segThick));
            layout.Children.Add(segments[4],
                new Rectangle(x, y + segThick * 2 + segLen, segThick, segLen));
            layout.Children.Add(segments[5],
                new Rectangle(x, y + segThick, segThick, segLen));
            layout.Children.Add(segments[6],
                new Rectangle(x + segThick, y + segThick + segLen, segLen, segThick));
        }

        public void Set(int digit)
        {
            if (digit < 0 || digit > 9)
                return;

            for (int i = 0; i < 7; i++)
            {
                segments[i].BackgroundColor = digitPatterns[digit, i] ? colOn : colOff;
            }
        }
    }
}
