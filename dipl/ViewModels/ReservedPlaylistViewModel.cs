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
using System.Windows.Media;

namespace dipl.ViewModels
{
    class ReservedPlaylistViewModel : ErrorViewModelBase
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
                    if (DataHandler.DeleteAudio(Playlist[Playlist.IndexOf((Audio)obj)]))
                    {
                        Playlist.RemoveAt(Playlist.IndexOf((Audio)obj));
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
                    App.CurrentAccount.Playlists[0].Audios.Add(Playlist[Playlist.IndexOf((Audio)obj)]);
                    if (!DataHandler.AddAudio(App.CurrentAccount.Playlists[0], (Playlist[Playlist.IndexOf((Audio)obj)])))
                    {
                        Notification = "";
                        Notification = mergedDict["g_DBerror"].ToString();
                    }
                });
            }
        }

        public ICommand AudioPlayCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.AudioPlayer.Queue = Playlist;
                    App.AudioPlayer.SelectAudio(Playlist.IndexOf((Audio)obj));
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
                    if (!DataHandler.DeletePlaylist(App.CurrentAccount.Playlists[0], false))
                    {
                        Notification = "";
                        Notification = mergedDict["g_DBerror"].ToString();
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
                    Playlist[Playlist.IndexOf((Audio)obj)].IsLiked = !Playlist[Playlist.IndexOf((Audio)obj)].IsLiked;
                    DataHandler.UpdateAudio(Playlist[Playlist.IndexOf((Audio)obj)]);
                });
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    Playlist Queue = App.CurrentAccount.Playlists[0];
                    foreach (string filename in (string[])obj)
                    {
                        Queue.Audios.Add(new Audio(filename, false));
                        if (DataHandler.AddAudio(Queue, new Audio(filename, false)))
                        {

                            Playlist = Queue.Audios;
                        }
                        else
                        {
                            Notification = mergedDict["g_DBerror"].ToString();
                        }
                    }

                });
            }
        }

        public ImageSource Image => App.AudioPlayer.CurrentAudio.Image.ToImage();

        public ReservedPlaylistViewModel(bool isLiked)
        {
            App.AudioPlayer.AudioSelected += () => { OnPropertyChanged(nameof(Image)); };
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
