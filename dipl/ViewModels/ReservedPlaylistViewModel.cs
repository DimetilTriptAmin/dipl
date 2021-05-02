using dipl.Models;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class ReservedPlaylistViewModel : ViewModelBase
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

        bool _isQueue;
        public Visibility IsQueue
        {
            get
            {
                if (_isQueue)
                {
                    return Visibility.Collapsed;
                }
                else return Visibility.Visible;
            }
        }

        bool _isLiked;
        public Visibility IsLiked
        {
            get
            {
                if (_isLiked)
                {
                    return Visibility.Collapsed;
                }
                else return Visibility.Visible;
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _playlist.Audios.RemoveAt((int)obj);
                });
            }
        }

        public ObservableCollection<Audio> Audios
        {
            get
            {
                return _playlist.Audios;
            }
        }

        public ReservedPlaylistViewModel(Playlist playlist, bool isLiked)
        {
            Playlist = playlist;
            _isLiked = isLiked;
            _isQueue = !isLiked;
        }
    }
}
