using dipl.Models;
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
        private readonly ViewModelBase _homeVM;
        private readonly ViewModelBase _playlistsVM;
        private readonly ViewModelBase _queueVM;
        private readonly ViewModelBase _profileVM;
        private readonly ViewModelBase _likedVM;

        private readonly Sleeper sleeper;

        public ViewModelBase CurrentViewModel
        {
            get => _navigationStore.CurrentViewModel;
        }

        public MainViewModel(NavigationStore navigationStore)
        {

            _navigationStore = navigationStore;
            _homeVM = new HomeViewModel(App.CurrentAccount.Playlists, App.CurrentAccount.Recent, _navigationStore);
            _navigationStore.CurrentViewModel = _homeVM;

            _playlistsVM = new PlaylistsViewModel(App.CurrentAccount.Playlists, _navigationStore);
            _queueVM = new ReservedPlaylistViewModel(App.CurrentAccount.Queue, false);
            _profileVM = new ProfileViewModel();
            _likedVM = new ReservedPlaylistViewModel(App.CurrentAccount.Liked,true);


            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

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
                    _navigationStore.CurrentViewModel = _likedVM;
                });
            }
        }

        public ICommand ProfileClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = _profileVM;
                });
            }
        }

        public ICommand HomeClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = _homeVM;
                });
            }
        }

        public ICommand PlaylistsClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = _playlistsVM;
                });
            }
        }

        public ICommand QueueClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = _queueVM;
                });
            }
        }
    }
}
