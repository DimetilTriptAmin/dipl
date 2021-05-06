using dipl.Stores;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace dipl.ViewModels
{
    class ProfileViewModel : ErrorViewModelBase
    {

        public string Username => App.CurrentAccount.User;

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

        public ImageSource Image
        {
            get
            {
                return App.CurrentAccount.Image;
            }
            set
            {
                App.CurrentAccount.Image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public ICommand ChangePicCommand
        {
            get
            {
                return new RelayCommand((obj) => {
                    Image = new BitmapImage(new Uri((string)obj, UriKind.Absolute));
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

        public ICommand ChangePassword
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    ValidatePassword();
                    ValidateRepeatedPassword();
                });
            }
        }

        private void ValidatePassword()
        {
            ClearErrors(nameof(Password));
            if (Password == null || Password.Length <= 4)
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
