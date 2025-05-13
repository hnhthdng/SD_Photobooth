using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoBooth_App.Handler
{
    public class UIHandler
    {
        public readonly Action<BitmapImage> SetImageAction;
        private readonly Image LiveViewImage;
        private readonly Button CaptureButton;
        private readonly Button FocusFarModeButton;
        private readonly Button FocusNearModeButton;
        private readonly Button NextButton;
        private readonly Grid BlurLayer;
        private readonly Grid CountdownCircle;
        private readonly TextBlock CountdownTextBlock;
        private readonly System.Windows.Shapes.Path ProgressArc;
        private int photoCount = 0;
        private int CountdownTime = 3;
        private const double CircleRadius = 75;


        public UIHandler(Image liveViewImage, Button captureButton, Button focusFarModeButton, Button focusNearModeButton, Button nextButton, Grid blurLayer, Grid countdownCircle, TextBlock countdownTextBlock, System.Windows.Shapes.Path progressArc)
        {
            LiveViewImage = liveViewImage;
            CaptureButton = captureButton;
            FocusFarModeButton = focusFarModeButton;
            FocusNearModeButton = focusNearModeButton;
            NextButton = nextButton;
            BlurLayer = blurLayer;
            CountdownCircle = countdownCircle;
            CountdownTextBlock = countdownTextBlock;
            ProgressArc = progressArc;
            SetImageAction = (BitmapImage img) => { LiveViewImage.Source = img; };
        }

        public async Task StartCountdown()
        {
            BlurLayer.Visibility = Visibility.Visible;
            CountdownCircle.Visibility = Visibility.Visible;

            for (int i = CountdownTime; i > 0; i--)
            {
                UpdateProgressArc(i);
                CountdownTextBlock.Text = i.ToString();
                await Task.Delay(1000);
            }

            BlurLayer.Visibility = Visibility.Collapsed;
            CountdownCircle.Visibility = Visibility.Collapsed;
        }

        private void UpdateProgressArc(int remainingTime)
        {
            double percentage = (double)remainingTime / CountdownTime;
            double angle = percentage == 1 ? 359.999 : 360 * percentage;

            double startAngle = -90;
            double endAngle = startAngle + angle;

            double startX = CircleRadius + CircleRadius * Math.Cos(startAngle * Math.PI / 180);
            double startY = CircleRadius + CircleRadius * Math.Sin(startAngle * Math.PI / 180);
            double endX = CircleRadius + CircleRadius * Math.Cos(endAngle * Math.PI / 180);
            double endY = CircleRadius + CircleRadius * Math.Sin(endAngle * Math.PI / 180);

            bool isLargeArc = angle > 180;

            PathFigure figure = new PathFigure
            {
                StartPoint = new Point(startX, startY),
                Segments = { new ArcSegment(new Point(endX, endY), new Size(CircleRadius, CircleRadius), 0, isLargeArc, SweepDirection.Clockwise, true) }
            };

            PathGeometry geometry = new PathGeometry { Figures = { figure } };
            ProgressArc.Data = geometry;
        }

        public void EnableUI(bool enable)
        {
            CaptureButton.IsEnabled = enable;
            FocusFarModeButton.IsEnabled = enable;
            FocusNearModeButton.IsEnabled = enable;
            NextButton.IsEnabled = enable;
        }
    }
}