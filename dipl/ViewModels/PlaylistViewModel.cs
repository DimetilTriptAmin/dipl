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
                    if (DataHandler.DeleteAudio(Playlist.Audios[Playlist.Audios.IndexOf((Audio)obj)]))
                    {
                        Playlist.Audios.RemoveAt(Playlist.Audios.IndexOf((Audio)obj));
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
                    Playlist.Audios[Playlist.Audios.IndexOf((Audio)obj)].IsLiked = !Playlist.Audios[Playlist.Audios.IndexOf((Audio)obj)].IsLiked;
                    DataHandler.UpdateAudio(Playlist.Audios[Playlist.Audios.IndexOf((Audio)obj)]);
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
                    App.AudioPlayer.SelectAudio(Playlist.Audios.IndexOf((Audio)obj));
                });
            }
        }

        public ICommand QueueAddCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    Audio audioToInsert = new Audio(Playlist.Audios[Playlist.Audios.IndexOf((Audio)obj)].SourceUrl,
                        Playlist.Audios[Playlist.Audios.IndexOf((Audio)obj)].IsLiked);
                    App.CurrentAccount.Playlists[0].Audios.Add(audioToInsert);
                    if (!DataHandler.AddAudio(App.CurrentAccount.Playlists[0], audioToInsert))
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
