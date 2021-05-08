using dipl.Models.Data;
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

        public string Username => App.CurrentAccount.User.Username;

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

        private SecureString _newPassword;
        public SecureString NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
                ValidateNewPassword();
            }
        }

        public ImageSource Image
        {
            get
            {
                return App.CurrentAccount.Image.ToImage();
            }
            set
            {
                App.CurrentAccount.Image = value.ToBytes();
                OnPropertyChanged(nameof(Image));
            }
        }

        public ICommand ChangePicCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    ImageSource imagesource = new BitmapImage(new Uri((string)obj, UriKind.Absolute));
                    if (DataHandler.UpdateProfilePic(App.CurrentAccount, imagesource.ToBytes()))
                    {
                        Image = imagesource;
                    }
                    else
                    {
                        //todo ошибка
                    }
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

        public ICommand LogoutCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.CurrentAccount = null;
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
                    ValidateNewPassword();
                    if (!CanValidate) return;
                    if (DataHandler.UpdateProfilePassword(App.CurrentAccount.User, NewPassword))
                    {
                        App.CurrentAccount.User.PasswordHash = HashGenerator.GetHash(NewPassword);
                    }
                });
            }
        }

        private void ValidatePassword()
        {
            ClearErrors(nameof(Password));
            if (Password == null || !HashGenerator.GetHash(Password).Equals(App.CurrentAccount.User.PasswordHash))
                AddError(nameof(Password), mergedDict["p_Wrong"].ToString());
        }

        private void ValidateNewPassword()
        {
            ClearErrors(nameof(NewPassword));
            if (NewPassword == null || NewPassword.Length <= 4)
                AddError(nameof(NewPassword), mergedDict["l_ShortPass"].ToString());
        }
    }
}
