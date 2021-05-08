using dipl.Models;
using dipl.Models.Data;
using dipl.Stores;
using dipl.View.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class SignUpViewModel : ErrorViewModelBase
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
                ValidateUserName();
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
                ValidatePassword();
                ValidateRepeatedPassword();
            }
        }

        private SecureString _repeatedPassword;
        public SecureString RepeatedPassword
        {
            get => _repeatedPassword;
            set
            {
                _repeatedPassword = value;
                OnPropertyChanged(nameof(RepeatedPassword));
                ValidateRepeatedPassword();
            }
        }


        private string _notification ="d";
        public string Notification
        {
            get => _notification;
            set
            {
                _notification = value;
                OnPropertyChanged(nameof(Notification));
            }
        }

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
                return new RelayCommand((obj) => {
                    ValidateUserName();
                    ValidatePassword();
                    ValidateRepeatedPassword();
                    if (!CanValidate) return;

                    User user = new User()
                    {
                        Username = Login,
                        PasswordHash = HashGenerator.GetHash(Password)
                    };

                    if (DataHandler.Register(user))
                    {
                        //TODO оповещение об удачной регистрации
                    }
                    else
                    {
                        AddError(nameof(Login), mergedDict["l_TakenLogin"].ToString());
                    }
                });
            }
        }


        public SignUpViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        private void ValidateUserName()
        {
            ClearErrors(nameof(Login));
            if (string.IsNullOrWhiteSpace(Login))
            {
                AddError(nameof(Login), mergedDict["l_EmptyLogin"].ToString());
                return;
            }
            string pattern = @"^[a-z][0-9a-z]*";
            if (!Regex.IsMatch(Login, pattern, RegexOptions.IgnoreCase))
            {
                AddError(nameof(Login), mergedDict["l_IncorrectLogin"].ToString());
            }
            if (Login == null || Login?.Length <= 4)
                AddError(nameof(Login), mergedDict["l_ShortLogin"].ToString());
        }

        private void ValidatePassword()
        {
            ClearErrors(nameof(Password));
            if (Password == null || Password.Length <=4)
                AddError(nameof(Password), mergedDict["l_ShortPass"].ToString());
        }

        private void ValidateRepeatedPassword()
        {
            ClearErrors(nameof(RepeatedPassword));
            if (!RepeatedPassword.IsEqualTo(Password))
                AddError(nameof(RepeatedPassword), mergedDict["l_EqualPass"].ToString());
        }
    }
}
