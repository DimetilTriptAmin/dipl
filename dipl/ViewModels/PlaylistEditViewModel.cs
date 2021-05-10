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
using System.Windows.Media.Imaging;

namespace dipl.ViewModels
{
    class PlaylistEditViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Playlist _bufferPlaylist;
        private readonly Playlist _playlistToEdit;

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
                OnPropertyChanged(nameof(Audios));
            }
        }

        public ImageSource Image
        {
            get
            {
                return _bufferPlaylist.Image.ToImage();
            }
            set
            {
                _bufferPlaylist.Image = value.ToBytes();
                OnPropertyChanged(nameof(Image));
            }
        }

        public ICommand ConfirmCommand
        {
            get
            {
                return new RelayCommand((obj)=> {
                    if (DataHandler.UpdatePlaylist(_playlistToEdit, _bufferPlaylist))
                    {
                        _playlistToEdit.Name = _bufferPlaylist.Name;
                        _playlistToEdit.Image = _bufferPlaylist.Image;
                        _playlistToEdit.Audios = _bufferPlaylist.Audios;
                        _navigationStore.CurrentViewModel = new PlaylistViewModel(_playlistToEdit, _navigationStore);
                    }
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
                        Audio audio = new Audio(filename, false);
                        Audios.Add(audio);
                    }
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

        public ICommand ChoosePicCommand
        {
            get
            {
                return new RelayCommand((obj) => {
                    Image = new BitmapImage(new Uri((string)obj, UriKind.Absolute));
                });
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _bufferPlaylist.Audios.RemoveAt((int)obj);
                });
            }
        }

        public PlaylistEditViewModel(ref Playlist playlistToEdit, NavigationStore navigationStore)
        {
            _playlistToEdit = playlistToEdit;

            string name = string.Copy(playlistToEdit.Name);
            ObservableCollection<Audio> audios = new ObservableCollection<Audio>(playlistToEdit.Audios);
            byte[] imageSource = (byte[])playlistToEdit.Image.Clone();

            _bufferPlaylist = new Playlist(name,audios,imageSource);
            _navigationStore = navigationStore;
        }
    }
}
