using dipl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace dipl.Models
{
    class Sleeper : ViewModelBase
    {
        private DateTime starTime;
        private DateTime endTime { get; set; }

        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime
        {
            get
            {
                return _remainingTime;
            }
            set
            {
                _remainingTime = value;
                OnPropertyChanged("RemainingTime");
            }
        }

        private DispatcherTimer timer;
        public void Sleep(int value)
        {
            starTime = DateTime.UtcNow;
            endTime = starTime.AddMinutes(value);
            timer = new DispatcherTimer() { Interval = new TimeSpan(1000) };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void Stop()
        {
            timer?.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RemainingTime = endTime - DateTime.UtcNow;
            if (RemainingTime < TimeSpan.Zero)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
