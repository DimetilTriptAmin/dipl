using dipl.Models;
using dipl.Models.Data;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class ReservedPlaylistViewModel : ViewModelBase
    {
        public ICollectionView PlaylistView { get; }

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
                OnPropertyChanged(nameof(Playlist));
            }
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
                PlaylistView.Refresh();
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
                    if (DataHandler.DeleteAudio(Playlist[(int)obj]))
                    {
                        Playlist.RemoveAt((int)obj);
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
                    App.CurrentAccount.Playlists[0].Audios.Add(Playlist[(int)obj]);
                    if (!DataHandler.UpdatePlaylist(App.CurrentAccount.Playlists[0], App.CurrentAccount.Playlists[0]))
                    {
                        //TODO ошибка
                    }
                });
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.CurrentAccount.Playlists[0].Audios.Clear();
                    if (!DataHandler.UpdatePlaylist(App.CurrentAccount.Playlists[0], App.CurrentAccount.Playlists[0]))
                    {
                        //TODO ошибка
                    }
                });
            }
        }

        public ICommand LikeCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    Playlist[(int)obj].IsLiked = !Playlist[(int)obj].IsLiked;
                    DataHandler.UpdateAudio(Playlist[(int)obj]);
                });
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return new RelayCommand((obj) => {
                    Playlist Queue = new Playlist("Queue");
                    Queue.Audios = Playlist;
                    foreach (string filename in (string[])obj)
                    {
                        Queue.Audios.Add(new Audio(filename, false));
                    }
                    if (DataHandler.UpdatePlaylist(App.CurrentAccount.Playlists[0], Queue))
                    {
                        Playlist = Queue.Audios;
                    }
                    else
                    {
                        //TODO ошибка
                    }
                });
            }
        }

        public ReservedPlaylistViewModel(bool isLiked)
        {

            if (isLiked)
            {
                Playlist = DataHandler.LoadLiked();
            }
            else
            {
                Playlist = App.CurrentAccount.Playlists[0].Audios;
            }

            PlaylistView = CollectionViewSource.GetDefaultView(Playlist);
            PlaylistView.Filter = GetFilter;

            _isLiked = isLiked;
            _isQueue = !isLiked;
        }

        private bool GetFilter(object obj)
        {
            if(obj is Audio audio)
            {
                return audio.Name?.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }
    }
}
