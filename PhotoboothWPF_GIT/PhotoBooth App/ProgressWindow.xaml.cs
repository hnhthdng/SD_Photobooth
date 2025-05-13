using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhotoBooth_App
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public double ProgressValue
        {
            get { return (double)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }

        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.Register("ProgressValue", typeof(double), typeof(ProgressWindow),
                new PropertyMetadata(0.0, OnProgressValueChanged));

        private static void OnProgressValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as ProgressWindow;
            window?.UpdateArc((double)e.NewValue);
        }

        public ProgressWindow()
        {
            InitializeComponent();
        }

        private void UpdateArc(double progress)
        {
            progressText.Text = $"{progress:0}%";

            double angle = progress * 360.0 / 100.0;
            double radians = angle * Math.PI / 180.0;

            double centerX = 75;
            double centerY = 75;
            double radius = 70;

            double endX = centerX + radius * Math.Sin(radians);
            double endY = centerY - radius * Math.Cos(radians);

            arcSegment.Point = new Point(endX, endY);
            arcSegment.Size = new Size(radius, radius);
            arcSegment.IsLargeArc = angle > 180.0;
        }

        public void AnimateProgress(double newProgress)
        {
            newProgress = Math.Max(0, Math.Min(100, newProgress));

            DoubleAnimation animation = new DoubleAnimation
            {
                From = ProgressValue,
                To = newProgress,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            this.BeginAnimation(ProgressValueProperty, animation);
        }
    }
}
