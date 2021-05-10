using dipl.Models;
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
    class PlaylistViewModel : ErrorViewModelBase
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
                    Playlist.Audios[(int)obj].IsLiked = !Playlist.Audios[(int)obj].IsLiked;
                    DataHandler.UpdateAudio(Playlist.Audios[(int)obj]);
                });
            }
        }

        public ICommand AudioPlayCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.AudioPlayer.Queue = Playlist.Audios;
                    App.AudioPlayer.SelectAudio((int)obj);
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
                    if (!DataHandler.AddAudio(App.CurrentAccount.Playlists[0], Playlist.Audios[(int)obj]))
                    {
                        Notification = "";
                        Notification = mergedDict["g_DBerror"].ToString();
                    }
                });
            }
        }

        public PlaylistViewModel(Playlist playlist, NavigationStore navigationStore)
        {
            Playlist = playlist;
            _navigationStore = navigationStore;
        }
    }
}
