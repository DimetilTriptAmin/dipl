using dipl.Models;
using dipl.Models.Data;
using dipl.Stores;
using dipl.View.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                    if (DataHandler.DeletePlaylist(_playlists[(int)obj]))
                    {
                        _playlists.RemoveAt((int)obj);
                    }
                    else
                    {
                        //TODO: error
                    }
                });
            }
        }

        public ICommand CreatePlaylistCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    Playlist newPlaylist = new Playlist("") { PlaylistId = App.CurrentAccount.Playlists.Count };
                    newPlaylist.AccountId = App.CurrentAccount.AccountId;
                    if (DataHandler.AddPlaylist(newPlaylist))
                    {
                        _playlists.Add(newPlaylist);
                        _navigationStore.CurrentViewModel = new PlaylistViewModel(newPlaylist, _navigationStore);
                    }
                    else
                    {
                        //TODO: ошибка
                    }
                });
            }
        }

        public ICommand QueueAddCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    foreach (Audio audio in Playlists[(int)obj].Audios)
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

        public PlaylistsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _playlists = new ObservableCollection<Playlist>(App.CurrentAccount.Playlists.Skip(3));
        }
    }
}
