using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace PhotoBooth_App.Handler
{
    class CountdownTimer
    {
        private DispatcherTimer timer;
        private TimeSpan countdownTime;

        public event Action<string, SolidColorBrush> OnTimeChanged;
        public event Action OnTimerFinished;

        public CountdownTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        public void Start(TimeSpan duration)
        {
            countdownTime = duration;
            timer.Start();
            UpdateUI();
        }

        public void Stop()
        {
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (countdownTime.TotalSeconds > 0)
            {
                countdownTime = countdownTime.Subtract(TimeSpan.FromSeconds(1));
                UpdateUI();
            }
            else
            {
                timer.Stop();
                OnTimerFinished?.Invoke();
            }
        }

        private void UpdateUI()
        {
            SolidColorBrush color = GetColorBasedOnTime();
            OnTimeChanged?.Invoke(countdownTime.ToString(@"hh\:mm\:ss"), color);
        }

        private SolidColorBrush GetColorBasedOnTime()
        {
            if (countdownTime.TotalMinutes <= 1)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D32F2F")); 
            }
            else if (countdownTime.TotalMinutes <= 3)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB300")); 
            }
            else
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#388E3C")); 
            }
        }
    }
}
