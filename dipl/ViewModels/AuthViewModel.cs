using dipl.Pages;
using dipl.Stores;
using dipl.View.ViewModel;
using dipl.Windows;
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
    class AuthViewModel : ViewModelBase
    {

        public string Title
        {
            get
            {
                if (CurrentViewModel is SignInViewModel)
                {
                    return "Signing In";
                }
                else return "Signing Up";
            }
        }

        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel
        {
            get => _navigationStore.CurrentViewModel;
        }

        public AuthViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        public ICommand GuestCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    MainWindow mw = new MainWindow();
                    Application.Current.MainWindow.Close();
                    mw.Show();
                });
            }
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
                OnPropertyChanged(nameof(CurrentViewModel));
                OnPropertyChanged(nameof(Title));
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
    }
}
