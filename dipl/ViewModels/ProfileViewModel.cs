﻿using dipl.Stores;
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
