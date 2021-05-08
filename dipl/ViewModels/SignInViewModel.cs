using dipl.Models;
using dipl.Models.Data;
using dipl.Stores;
using dipl.View.ViewModel;
using dipl.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class SignInViewModel : ErrorViewModelBase
    {
        private readonly NavigationStore _navigationStore;

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

        private SecureString _password;
        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand NavigateSignUpCommand
        {
            get
            {
                return new RelayCommand((obj) => {
                    _navigationStore.CurrentViewModel = new SignUpViewModel(_navigationStore);
                });
            }
        }

        public ICommand SignInCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {

                    User user = DataHandler.CheckUser(Login, Password);
                    if (user != null)
                    {
                        App.CurrentAccount = DataHandler.GetAccount(user);
                    }
                    else
                    {
                        ClearErrors(nameof(Login));
                        ClearErrors(nameof(Password));
                        AddError(nameof(Login), mergedDict["l_Wrong"].ToString());
                        AddError(nameof(Password), mergedDict["l_Wrong"].ToString());
                    }
                });
            }
        }

        public SignInViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}
