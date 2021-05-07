using dipl.Models;
using dipl.Models.Data;
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
                    _navigationStore.CurrentViewModel = new PlaylistViewModel(Playlists[Convert.ToInt32(obj)], _navigationStore);
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
                        App.CurrentAccount.Playlists[1].Audios.Add(audio);
                    }
                    if (!DataHandler.UpdatePlaylist(App.CurrentAccount.Playlists[1], App.CurrentAccount.Playlists[1]))
                    {
                        //TODO ошибка
                    }
                });
            }
        }

        public HomeViewModel(NavigationStore navigationStore)
        {
            Playlists = new ObservableCollection<Playlist>(App.CurrentAccount.Playlists.Skip(3));
            Recent = App.CurrentAccount.Playlists[2].Audios;
            _navigationStore = navigationStore;
        }
    }
}
