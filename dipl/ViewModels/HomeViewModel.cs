using dipl.Models;
using dipl.Stores;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class HomeViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private ObservableCollection<Playlist> _playlists;

        public ObservableCollection<Playlist> Playlists
        {
            get
            {
                return _playlists;
            }
            set
            {
                _playlists = value;
                OnPropertyChanged("Playlists");
            }
        }

        private ObservableCollection<Audio> _recent;

        public ObservableCollection<Audio> Recent
        {
            get
            {
                return _recent;
            }
            set
            {
                _recent = value;
                OnPropertyChanged("Recent");
            }
        }

        public ICommand ShowPlaylistCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = new PlaylistViewModel(Playlists[(int)obj], _navigationStore);
                });
            }
        }

        public ICommand QueueAddCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    foreach(Audio audio in Playlists[(int)obj].Audios)
                    {
                        App.CurrentAccount.Queue.Add(audio);
                    }
                });
            }
        }

        public HomeViewModel(NavigationStore navigationStore)
        {
            Playlists = App.CurrentAccount.Playlists;
            Recent = App.CurrentAccount.Recent;
            _navigationStore = navigationStore;
        }
    }
}
