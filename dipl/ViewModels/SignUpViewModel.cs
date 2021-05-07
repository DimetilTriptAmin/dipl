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
                return new RelayCommand(async (obj) => {
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
                        AddError(nameof(Login), "The username is taken.");
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
            string pattern = @"^[a-z][0-9a-z]*";

            if (!Regex.IsMatch(Login, pattern, RegexOptions.IgnoreCase))
            {
                AddError(nameof(Login), "The username must begin with a letter.");
            }
            if (string.IsNullOrWhiteSpace(Login))
                AddError(nameof(Login), "The username cannot be empty.");
            if (string.Equals(Login, "Admin", StringComparison.OrdinalIgnoreCase))
                AddError(nameof(Login), "Admin is not valid username.");
            if (Login == null || Login?.Length <= 4)
                AddError(nameof(Login), "The username must be at least 5 characters long.");
        }

        private void ValidatePassword()
        {
            ClearErrors(nameof(Password));
            if (Password == null || Password.Length <=4)
                AddError(nameof(Password), "Password must be at least 5 characters long.");
        }

        private void ValidateRepeatedPassword()
        {
            ClearErrors(nameof(RepeatedPassword));
            if (!RepeatedPassword.IsEqualTo(Password))
                AddError(nameof(RepeatedPassword), "Passwords must be equal.");
        }
    }
}
