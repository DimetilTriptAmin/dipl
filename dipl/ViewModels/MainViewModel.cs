using dipl.Models;
using dipl.Models.Data;
using dipl.Pages;
using dipl.Stores;
using dipl.View.ViewModel;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace dipl.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private bool _isAdmin = false;
        public bool IsAdmin => !_isAdmin;

        private readonly NavigationStore _navigationStore;

        private readonly PlaylistsViewModel playlistsViewModel;
        private readonly ProfileViewModel profileViewModel;

        private readonly Sleeper sleeper;

        public ViewModelBase CurrentViewModel
        {
            get => _navigationStore.CurrentViewModel;
        }

        private void notifyProgress()
        {
            OnPropertyChanged(nameof(Position));
            OnPropertyChanged(nameof(PositionTime));
        }

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            if (App.CurrentAccount.UserType == UserType.Admin)
            {
                _isAdmin = true;
                _navigationStore.CurrentViewModel = new AdminViewModel();
            }
            else _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
            App.AudioPlayer.AudioSelected += () =>
            {
                OnPropertyChanged(nameof(DurationTime));
                OnPropertyChanged(nameof(Duration));
                OnPropertyChanged(nameof(CurrentAudioName));
                OnPropertyChanged(nameof(Image));
            };

            App.AudioPlayer.PlayingStatusChanged += () => OnPropertyChanged(nameof(IsPlaying));

            App.AudioPlayer.ProgressChanged += notifyProgress;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            playlistsViewModel = new PlaylistsViewModel(_navigationStore);
            profileViewModel = new ProfileViewModel();

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

        public string IsPlaying
        {
            get
            {
                if (App.AudioPlayer.PlayingStatus == Status.Paused) return "Play";
                else return "Pause";
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
        public string PositionTime => App.AudioPlayer.PositionTime.ToString(@"hh\:mm");
        public double Duration => App.AudioPlayer.CurrentAudio.DurationTime == null? 0: TimeSpan.Parse(App.AudioPlayer.CurrentAudio.DurationTime).TotalMinutes;
        public string DurationTime => App.AudioPlayer.CurrentAudio.DurationTime;
        public ImageSource Image => App.AudioPlayer.CurrentAudio.Image.ToImage();
        public string CurrentAudioName => App.AudioPlayer.CurrentAudio.Name;

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
                }, (obj) => !_isAdmin);
            }
        }

        public ICommand StopSleepClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    sleeper.Stop();
                }, (obj) => !_isAdmin);
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
                        App.AudioPlayer.Play();
                    }
                    else
                    {
                        App.AudioPlayer.Pause();
                    }
                }, (obj) => !_isAdmin);
            }
        }

        public ICommand BackClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.AudioPlayer.Back();
                }, (obj) => !_isAdmin);
            }
        }

        public ICommand ForwardClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.AudioPlayer.Forward();
                }, (obj) => !_isAdmin);
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
                }, (obj) => !_isAdmin);
            }
        }

        public ICommand MixClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.AudioPlayer.MixPlaylist();
                }, (obj) => !_isAdmin);
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
                }, (obj) => !_isAdmin);
            }
        }

        public ICommand LikedClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = new ReservedPlaylistViewModel(true);
                }, (obj) => !_isAdmin);
            }
        }

        public ICommand ProfileClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = profileViewModel;
                }, (obj) => !_isAdmin);
            }
        }

        public ICommand HomeClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
                }, (obj) => !_isAdmin);
            }
        }

        public ICommand PlaylistsClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = playlistsViewModel;
                }, (obj) => !_isAdmin);
            }
        }

        public ICommand QueueClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = new ReservedPlaylistViewModel(false);
                }, (obj) => !_isAdmin);
            }
        }

        public ICommand ThumbDragStarted
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.AudioPlayer.ProgressChanged -= notifyProgress;
                    App.AudioPlayer.Pause();
                }, (obj) => !_isAdmin);
            }
        }

        public ICommand ThumbDragCompleted
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.AudioPlayer.Play();
                    App.AudioPlayer.ProgressChanged += notifyProgress;
                }, (obj) => !_isAdmin);
            }
        }

        public ICommand ThumbDragDelta
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    OnPropertyChanged(nameof(PositionTime));
                }, (obj) => !_isAdmin);
            }
        }
    }
}
