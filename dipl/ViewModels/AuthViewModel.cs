using dipl.Pages;
using dipl.Stores;
using dipl.View.ViewModel;
using dipl.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class AuthViewModel : ViewModelBase
    {

        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel
        {
            get => _navigationStore.CurrentViewModel;
        }

        public AuthViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModel = new SignInViewModel(navigationStore);
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
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
                _isSigningIn = !_isSigningIn;
                OnPropertyChanged(nameof(IsSigningIn));
                OnPropertyChanged(nameof(IsSigningUp));
                OnPropertyChanged(nameof(CurrentViewModel));
                for (double i = 0; i < 1.1; i += 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(20);
                }
            });
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

        private bool _isSigningIn = true;
        public Visibility IsSigningUp
        {
            get
            {
                return _isSigningIn ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Visibility IsSigningIn
        {
            get
            {
                return _isSigningIn ? Visibility.Visible : Visibility.Collapsed;
            }
        }


        public ICommand GuestCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    MainWindow mw = new MainWindow();
                    Application.Current.MainWindow.Close();
                    mw.ShowDialog();
                });
            }
        }

        public ICommand EnglishCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.Language = new CultureInfo("en-US");
                });
            }
        }

        public ICommand RussianCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.Language = new CultureInfo("ru-RU");
                });
            }
        }

        public ICommand DarkCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.Theme = "dark";
                });
            }
        }

        public ICommand LightCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.Theme = "light";
                });
            }
        }
    }

}
