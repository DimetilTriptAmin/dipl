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
using System.Windows.Media;

namespace dipl.ViewModels
{
    class PlaylistsViewModel : ErrorViewModelBase
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
                    _navigationStore.CurrentViewModel = new PlaylistViewModel(Playlists[Playlists.IndexOf((Playlist)obj)], _navigationStore);
                });
            }
        }

        public ICommand DeletePlaylistCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (DataHandler.DeletePlaylist(Playlists[Playlists.IndexOf((Playlist)obj)], true))
                    {
                        Playlists.RemoveAt((int)obj+2);
                        PlaylistsView.Refresh();
                    }
                    else
                    {
                        Notification = mergedDict["g_DBerror"].ToString();
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
                    Playlist newPlaylist = new Playlist("New Playlist");
                    newPlaylist.AccountId = App.CurrentAccount.AccountId;
                    if (DataHandler.AddPlaylist(newPlaylist))
                    {
                        Playlists.Add(newPlaylist);
                        _navigationStore.CurrentViewModel = new PlaylistViewModel(newPlaylist, _navigationStore);
                    }
                    else
                    {
                        Notification = "";
                        Notification = mergedDict["g_DBerror"].ToString();
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
                    foreach (Audio audio in Playlists[Playlists.IndexOf((Playlist)obj)].Audios)
                    {
                        Playlists[0].Audios.Add(audio);
                        if (!DataHandler.AddAudio(Playlists[0], audio))
                        {
                            Notification = "";
                            Notification = mergedDict["g_DBerror"].ToString();
                        }
                    }
                });
            }
        }

        public ICommand PlaylistPlayCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.AudioPlayer.Queue = Playlists[Playlists.IndexOf((Playlist)obj)].Audios;
                    App.AudioPlayer.SelectAudio(0);
                });
            }
        }

        public ImageSource Image => App.AudioPlayer.CurrentAudio.Image.ToImage();

        public PlaylistsViewModel(NavigationStore navigationStore)
        {
            App.AudioPlayer.AudioSelected += () => { OnPropertyChanged(nameof(Image)); };
            _navigationStore = navigationStore;
            _playlists = App.CurrentAccount.Playlists;
            PlaylistsView = CollectionViewSource.GetDefaultView(Playlists.Skip(2));
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
