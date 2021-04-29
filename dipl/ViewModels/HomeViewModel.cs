using dipl.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dipl.ViewModels
{
    class HomeViewModel : ViewModelBase
    {
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

        public HomeViewModel()
        {
            ObservableCollection<Playlist> playlists = new ObservableCollection<Playlist>();
            playlists.Add(new Playlist("Linkin Park"));
            playlists.Add(new Playlist("Linkin Park"));
            playlists.Add(new Playlist("Linkin Park"));
            playlists.Add(new Playlist("Linkin Park"));
            playlists.Add(new Playlist("Linkin Park"));
            playlists.Add(new Playlist("Linkin Park"));
            playlists.Add(new Playlist("Linkin Park"));
            playlists.Add(new Playlist("Linkin Park"));
            playlists.Add(new Playlist("Linkin Park"));
            playlists.Add(new Playlist("Linkin Park"));
            playlists.Add(new Playlist("Linkin Park"));
            Playlists = playlists;

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

            Recent = pl.Audios;
        }
    }
}
