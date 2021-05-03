using dipl.Models;
using dipl.Stores;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class SignUpViewModel:ViewModelBase
    {

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _repeatedPassword;
        public string RepeatedPassword
        {
            get => _repeatedPassword;
            set
            {
                _repeatedPassword = value;
                OnPropertyChanged(nameof(RepeatedPassword));
            }
        }

        private readonly NavigationStore _navigationStore;

        public ICommand NavigateSignInCommand
        {
            get
            {
                return new RelayCommand((obj) => {
                    _navigationStore.CurrentViewModel = new SignInViewModel(_navigationStore);
                });
            }
        }

        public ICommand SignUpCommand
        {
            get
            {
                return null;
            }
        }

        public SignUpViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}
