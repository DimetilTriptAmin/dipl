using dipl.Models;
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
    class PlaylistEditViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private Playlist _bufferPlaylist;
        private Playlist _playlistToEdit;

        public Playlist Playlist => _bufferPlaylist;

        public string Name
        {
            get
            {
                return _bufferPlaylist.Name;
            }
            set
            {
                _bufferPlaylist.Name = value;
                OnPropertyChanged(Name);
            }
        }

        public ObservableCollection<Audio> Audios
        {
            get
            {
                return _bufferPlaylist.Audios;
            }
            set
            {
                _bufferPlaylist.Audios = value;
                OnPropertyChanged("Audios");
            }
        }

        public ICommand ConfirmCommand
        {
            get
            {
                return new RelayCommand((obj)=> {
                    _playlistToEdit.Name = _bufferPlaylist.Name;
                    _navigationStore.CurrentViewModel = new PlaylistViewModel(_playlistToEdit, _navigationStore);
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand((obj) => {
                    _navigationStore.CurrentViewModel = new PlaylistViewModel(_playlistToEdit, _navigationStore);
                });
            }
        }

        public PlaylistEditViewModel(ref Playlist playlistToEdit, NavigationStore navigationStore)
        {
            _playlistToEdit = playlistToEdit;

            string name = string.Copy(playlistToEdit.Name);
            ObservableCollection<Audio> audios = new ObservableCollection<Audio>(playlistToEdit.Audios);
            ImageSource imageSource = playlistToEdit.Image.Clone();

            _bufferPlaylist = new Playlist(name,audios,imageSource);
            _navigationStore = navigationStore;
        }
    }
}
