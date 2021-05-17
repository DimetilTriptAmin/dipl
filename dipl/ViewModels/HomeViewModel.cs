using dipl.Models;
using dipl.Models.Data;
using dipl.Stores;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace dipl.ViewModels
{
    class HomeViewModel : ErrorViewModelBase
    {
        private readonly NavigationStore _navigationStore;

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

        public ICommand LikeCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    Recent[(int)obj].IsLiked = !Recent[(int)obj].IsLiked;
                    DataHandler.UpdateAudio(Recent[(int)obj]);
                });
            }
        }

        public ICommand ShowPlaylistCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _navigationStore.CurrentViewModel = new PlaylistViewModel(Playlists[Playlists.IndexOf((Playlist)obj)], _navigationStore);
                });
            }
        }

        public ICommand PlaylistPlayCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.AudioPlayer.Queue = Playlists[Playlists.IndexOf((Playlist)obj)].Audios;
                    App.AudioPlayer.SelectAudio(0);
                });
            }
        }

        public ICommand AudioPlayCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.AudioPlayer.Queue = Recent;
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
                    foreach(Audio audio in Playlists[Playlists.IndexOf((Playlist)obj)].Audios)
                    {
                        App.CurrentAccount.Playlists[0].Audios.Add(audio);
                        if (!DataHandler.AddAudio(App.CurrentAccount.Playlists[0], audio))
                        {
                            Notification = "";
                            Notification = mergedDict["g_DBerror"].ToString();
                        }
                    }
                });
            }
        }

        public ICommand QueueAudioAddCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.CurrentAccount.Playlists[0].Audios.Add(Recent[(int)obj]);
                    if (!DataHandler.AddAudio(App.CurrentAccount.Playlists[0], Recent[(int)obj]))
                    {
                        Notification = "";
                        Notification = mergedDict["g_DBerror"].ToString();
                    }
                });
            }
        }

        public ImageSource Image => App.AudioPlayer.CurrentAudio.Image.ToImage();

        public HomeViewModel(NavigationStore navigationStore)
        {
            App.AudioPlayer.AudioSelected += () => { OnPropertyChanged(nameof(Image)); };
            Playlists = new ObservableCollection<Playlist>(App.CurrentAccount.Playlists.Skip(2));
            Recent = App.CurrentAccount.Playlists[1].Audios;
            _navigationStore = navigationStore;
        }
    }
}
