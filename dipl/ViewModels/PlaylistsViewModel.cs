using dipl.Models;
using dipl.Models.Data;
using dipl.Stores;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class PlaylistsViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;


        public ICollectionView PlaylistsView { get; }
        private readonly ObservableCollection<Playlist> _playlists;

        public ObservableCollection<Playlist> Playlists
        {
            get => _playlists;
        }

        private string _filter = string.Empty;
        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                OnPropertyChanged(nameof(Filter));
                PlaylistsView.Refresh();
            }
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
                        App.CurrentAccount.Playlists.Add(newPlaylist);
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
                        App.CurrentAccount.Playlists[0].Audios.Add(audio);
                    }
                    if (!DataHandler.UpdatePlaylist(App.CurrentAccount.Playlists[0], App.CurrentAccount.Playlists[0]))
                    {
                        //TODO ошибка
                    }
                });
            }
        }

        public PlaylistsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _playlists = new ObservableCollection<Playlist>(App.CurrentAccount.Playlists.Skip(2));
            PlaylistsView = CollectionViewSource.GetDefaultView(_playlists);
            PlaylistsView.Filter = GetFilter;
        }

        private bool GetFilter(object obj)
        {
            if (obj is Playlist playlist)
            {
                return playlist.Name?.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }
    }
}
