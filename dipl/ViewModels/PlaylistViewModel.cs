﻿using dipl.Models;
using dipl.Models.Data;
using dipl.Stores;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace dipl.ViewModels
{
    class PlaylistViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private Playlist _playlist;
        public Playlist Playlist
        {
            get
            {
                return _playlist;
            }
            set
            {
                _playlist = value;
                OnPropertyChanged("Playlist");
            }
        }

        public ObservableCollection<Audio> Audios
        {
            get
            {
                return _playlist.Audios;
            }
        }

        public ICommand EditCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = new PlaylistEditViewModel(ref _playlist, _navigationStore);
                });
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    if (DataHandler.DeleteAudio(Audios[(int)obj]))
                    {
                        _playlist.Audios.RemoveAt((int)obj);
                    }
                    else
                    {
                        //TODO: error
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
                        App.CurrentAccount.Playlists[0].Audios.Add(Playlist.Audios[(int)obj]);
                });
                if (!DataHandler.UpdatePlaylist(App.CurrentAccount.Playlists[0], App.CurrentAccount.Playlists[0]))
                {
                    //TODO ошибка
                }
            }
        }

        public PlaylistViewModel(Playlist playlist, NavigationStore navigationStore)
        {
            Playlist = playlist;
            _navigationStore = navigationStore;
        }
    }
}
