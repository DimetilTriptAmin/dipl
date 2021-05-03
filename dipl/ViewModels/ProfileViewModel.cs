using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class ProfileViewModel : ViewModelBase
    {

        private int _language;
        public int Language
        {
            get
            {
                if (App.Language.ToString() == "en-US") return 0;
                if (App.Language.ToString() == "ru-RU") return 1;
                else return 0;
            }
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));
            }
        }

        private int _theme;
        public int Theme
        {
            get
            {
                return App.themeid;
            }
            set
            {
                _theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }
        

        public ICommand ApplyCommand
        {
            get
            {
                return new RelayCommand((obj)=> {
                    CultureInfo lang;
                    switch (_language)
                    {
                        case 1: lang = new CultureInfo("ru-RU"); break;
                        default: lang = new CultureInfo("en-US"); break;
                    }
                    App.Language = lang;

                    switch (_theme)
                    {
                        case 1: App.Theme="light"; break;
                        default: App.Theme="dark"; break;
                    }
                });
            }
        }
    }
}
