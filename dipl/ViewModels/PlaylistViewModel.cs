﻿using dipl.Models;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace dipl.ViewModels
{
    class PlaylistViewModel : ViewModelBase
    {
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

                });
            }
        }

        public PlaylistViewModel(Playlist playlist)
        {
            Playlist = playlist;
        }
    }
}
