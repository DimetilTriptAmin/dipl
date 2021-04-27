using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private readonly Page LikedPage;
        private readonly Page HomePage;
        private readonly Page PlaylistsPage;

        public MainViewModel()
        {
            LikedPage = new Pages.LikedPage();
            HomePage = new Pages.HomePage();
            PlaylistsPage = new Pages.PlaylistsPage();
        }

        private Page _currentPage;
        public Page CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        public ICommand LikedClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    CurrentPage = LikedPage;
                });
            }
        }

        public ICommand HomeClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    CurrentPage = HomePage;
                });
            }
        }

        public ICommand PlaylistsClickCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    CurrentPage = PlaylistsPage;
                });
            }
        }
    }
}
