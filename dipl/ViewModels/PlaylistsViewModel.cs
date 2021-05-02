using dipl.Models;
using dipl.Stores;
using dipl.View.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class PlaylistsViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private readonly ObservableCollection<Playlist> _playlists;

        public ObservableCollection<Playlist> Playlists
        {
            get => _playlists;
        }

        public ICommand ShowPlaylistCommand
        {
            get
            {
                return new RelayCommand((obj)=> 
                {
                    _navigationStore.CurrentViewModel = new PlaylistViewModel(Playlists[(int)obj], _navigationStore);
                });
            }
        }

        public ICommand DeletePlaylistCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _playlists.RemoveAt((int)obj);
                });
            }
        }

        public ICommand CreatePlaylistCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    Playlist newPlaylist = new Playlist("");
                    _playlists.Add(newPlaylist);
                    _navigationStore.CurrentViewModel = new PlaylistViewModel(newPlaylist, _navigationStore);
                });
            }
        }

        public PlaylistsViewModel(ObservableCollection<Playlist> playlists,NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _playlists = playlists;
        }
    }
}
