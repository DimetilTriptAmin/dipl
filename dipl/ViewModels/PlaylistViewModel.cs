using dipl.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public PlaylistViewModel()
        {
            Playlist pl = new Playlist("Liked");
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            pl.Audios.Add(new Audio("Linkin Park - Numb"));
            Playlist = pl;
        }
    }
}
