using dipl.Models;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class PlaylistEditViewModel : ViewModelBase
    {
        private Playlist _bufferPlaylist;
        private Playlist _playlistToEdit;

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
                    _playlistToEdit = _bufferPlaylist;
                });
            }
        }

        public PlaylistEditViewModel(ref Playlist playlistToEdit)
        {
            _playlistToEdit = playlistToEdit;
            _bufferPlaylist = playlistToEdit;
        }
    }
}
