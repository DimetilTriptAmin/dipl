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
        private ObservableCollection<Audio> _playlist;
        public ObservableCollection<Audio> Playlist
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

        readonly bool _isQueue;
        public Visibility IsQueue
        {
            get
            {
                if (_isQueue)
                {
                    return Visibility.Visible;
                }
                else return Visibility.Collapsed;
            }
        }

        readonly bool _isLiked;
        public Visibility IsLiked
        {
            get
            {
                if (_isLiked)
                {
                    return Visibility.Visible;
                }
                else return Visibility.Collapsed;
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    Playlist.RemoveAt((int)obj);
                });
            }
        }

        public ICommand QueueAddCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.CurrentAccount.Queue.Add(Playlist[(int)obj]);
                });
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return new RelayCommand((obj) => {
                    foreach (string filename in (string[])obj)
                    {
                        Playlist.Add(new Audio(filename, false));
                    }
                });
            }
        }

        public ReservedPlaylistViewModel(bool isLiked)
        {
            if (isLiked)
            {
                Playlist = App.CurrentAccount.Liked;
            }
            else
            {
                Playlist = App.CurrentAccount.Queue;
            }
            _isLiked = isLiked;
            _isQueue = !isLiked;
        }
    }
}
