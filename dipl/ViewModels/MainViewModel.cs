using dipl.Models;
using dipl.Models.Data;
using dipl.Pages;
using dipl.Stores;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly NavigationStore _navigationStore;

        private readonly PlaylistsViewModel playlistsViewModel;
        private readonly ProfileViewModel profileViewModel;
        private readonly HomeViewModel homeViewModel;

        private readonly Sleeper sleeper;

        public ViewModelBase CurrentViewModel
        {
            get => _navigationStore.CurrentViewModel;
        }

        public MainViewModel(NavigationStore navigationStore)
        {

            App.AudioPlayer.AudioSelected += () =>
            {

            };

            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            homeViewModel = new HomeViewModel(_navigationStore);
            playlistsViewModel = new PlaylistsViewModel(_navigationStore);
            profileViewModel = new ProfileViewModel();
            _navigationStore.CurrentViewModel = homeViewModel;

            sleeper = new Sleeper();
            sleeper.PropertyChanged += (s, arg) => RemainingTime = sleeper.RemainingTime.ToString(@"mm\:ss");
        }

        private async void OnCurrentViewModelChanged()
        {
            await Task.Factory.StartNew(() =>
            {
                for (double i = 1; i > 0; i -= 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(10);
                }
                OnPropertyChanged("CurrentViewModel");
                for (double i = 0; i < 1.1; i += 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(10);
                }
            });
        }

        private bool _isPlaying = false;
        public string IsPlaying
        {
            get
            {
                return _isPlaying? "Pause":"Play";
            }
            set
            {
                if (value == "Pause")
                {
                    _isPlaying = true;
                }
                else
                {
                    _isPlaying = false;
                }
                OnPropertyChanged("IsPlaying");
            }

        }

        public double Position
        {
            get => App.AudioPlayer.Position;
            set
            {
                App.AudioPlayer.Position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
        public TimeSpan PositionTime => App.AudioPlayer.PositionTime;
        public double Duration => App.AudioPlayer.CurrentAudio.DurationTime == null?0: TimeSpan.Parse(App.AudioPlayer.CurrentAudio.DurationTime).TotalSeconds;
        public string DurationTime => App.AudioPlayer.CurrentAudio.DurationTime;

        private double _frameOpacity = 1;
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

        public int Volume
        {
            get { return App.AudioPlayer.Volume; }
            set { App.AudioPlayer.Volume = value; OnPropertyChanged(nameof(Volume)); }
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
                    if(App.AudioPlayer.PlayingStatus == Status.Paused)
                    {
                        App.AudioPlayer.SelectAudio(App.AudioPlayer.CurrentIndex);
                        IsPlaying = "Pause";
                        App.AudioPlayer.Play();
                    }
                    if (App.AudioPlayer.PlayingStatus == Status.Playing)
                    {
                        IsPlaying = "Play";
                        App.AudioPlayer.Pause();
                    }
                });
            }
        }

        public ICommand BackClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.AudioPlayer.Back();
                });
            }
        }

        public ICommand ForwardClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.AudioPlayer.Forward();
                });
            }
        }

        public ICommand RepeatClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (App.AudioPlayer.AutoRestart)
                    {
                        App.AudioPlayer.AutoRestart = false;
                        App.AudioPlayer.AutoNext = true;
                    }

                    else
                    {
                        App.AudioPlayer.AutoRestart = true;
                        App.AudioPlayer.AutoNext = false;
                    }
                });
            }
        }

        public ICommand MixClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.AudioPlayer.MixPlaylist();
                });
            }
        }

        public ICommand MuteCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (Volume != 0)
                    {
                        App.AudioPlayer.SavedVolume = Volume;
                        Volume = 0;
                    }
                    else
                    {
                        Volume = App.AudioPlayer.SavedVolume;
                    }
                });
            }
        }

        public ICommand LikedClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = new ReservedPlaylistViewModel(true);
                });
            }
        }

        public ICommand ProfileClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = profileViewModel;
                });
            }
        }

        public ICommand HomeClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = homeViewModel;
                });
            }
        }

        public ICommand PlaylistsClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = playlistsViewModel;
                });
            }
        }

        public ICommand QueueClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = new ReservedPlaylistViewModel(false);
                });
            }
        }
    }
}
