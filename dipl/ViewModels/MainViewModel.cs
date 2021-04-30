using dipl.Models;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private readonly Page LikedPage;
        private readonly Page ProfilePage;
        private readonly Page HomePage;
        private readonly Page PlaylistsPage;
        private readonly Sleeper sleeper;

        public MainViewModel()
        {
            ProfilePage = new Pages.ProfilePage();
            LikedPage = new Pages.PlaylistPage();
            HomePage = new Pages.HomePage();
            PlaylistsPage = new Pages.PlaylistsPage();
            sleeper = new Sleeper();
            sleeper.PropertyChanged += (s, arg) => RemainingTime = sleeper.RemainingTime.ToString(@"mm\:ss");
            FrameOpacity = 1;
            CurrentPage = HomePage;
        }

        private bool _isPlaying = true;
        public string IsPlaying
        {
            get
            {
                return _isPlaying? "Pause":"Play";
            }
            set
            {
                if (value == "Pause") _isPlaying = true;
                else _isPlaying = false;
                OnPropertyChanged("IsPlaying");
            }

        }

        private Page _currentPage;
        public Page CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        private double _frameOpacity;
        public double FrameOpacity
        {
            get
            {
                return _frameOpacity;
            }
            set
            {
                _frameOpacity = value;
                OnPropertyChanged("FrameOpacity");
            }
        }

        private int _timeToSleep = 1;
        public int TimeToSleep
        {
            get
            {
                return _timeToSleep;
            }
            set
            {
                _timeToSleep = value;
                OnPropertyChanged("TimeToSleep");
            }
        }

        public string _remainingTime;
        public string RemainingTime
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

        public ICommand SleepClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    sleeper.Sleep(TimeToSleep);
                });
            }
        }

        public ICommand StopSleepClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    sleeper.Stop();
                });
            }
        }

        public ICommand PlayClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (_isPlaying)
                    {
                        IsPlaying = "Play";
                    }
                    else IsPlaying = "Pause";
                });
            }
        }

        public ICommand LikedClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    SlowOpacity(LikedPage);
                });
            }
        }

        public ICommand ProfileClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    SlowOpacity(ProfilePage);
                });
            }
        }

        public ICommand HomeClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    SlowOpacity(HomePage);
                });
            }
        }

        public ICommand PlaylistsClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    SlowOpacity(PlaylistsPage);
                });
            }
        }

        private async void SlowOpacity(Page page)
        {
            await Task.Factory.StartNew(() =>
            {
                for (double i = 1; i > 0; i -= 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(10);
                }
                CurrentPage = page;
                for (double i = 0; i < 1.1; i += 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(20);
                }
            });
        }
    }
}
