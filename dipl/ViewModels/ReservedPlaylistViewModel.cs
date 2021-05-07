using dipl.Models;
using dipl.Models.Data;
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
                    App.CurrentAccount.Playlists[1].Audios.Add(Playlist[(int)obj]);
                    if (!DataHandler.UpdatePlaylist(App.CurrentAccount.Playlists[1], App.CurrentAccount.Playlists[1]))
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
                    App.CurrentAccount.Playlists[1].Audios.Clear();
                    if (!DataHandler.UpdatePlaylist(App.CurrentAccount.Playlists[1], App.CurrentAccount.Playlists[1]))
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
                    if (Playlist[(int)obj].IsLiked)
                    {
                        App.CurrentAccount.Playlists[0].Audios.Remove(Playlist[(int)obj]);
                    }
                    else
                    {
                        App.CurrentAccount.Playlists[0].Audios.Add(Playlist[(int)obj]);
                    }
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
                    if (DataHandler.UpdatePlaylist(App.CurrentAccount.Playlists[1], Queue))
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
                Playlist = App.CurrentAccount.Playlists[0].Audios;
                //TODO
            }
            else
            {
                Playlist = App.CurrentAccount.Playlists[1].Audios;
            }
            _isLiked = isLiked;
            _isQueue = !isLiked;
        }
    }
}
