using dipl.Models.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace dipl.Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public ObservableCollection<Audio> Audios { get; set; }

        public Playlist() { }

        public Playlist(string name)
        {
            Name = name;
            Audios = new ObservableCollection<Audio>();
            // TODO: Добавить картинку по умолчанию
            Image = ((ImageSource)(new BitmapImage(new Uri("../../Assets/anime.jpg", UriKind.Relative)))).ToBytes();
        }

        public Playlist(string name, ObservableCollection<Audio> audios, byte[] imageSource)
        {
            Name = name;
            Audios = audios;
            Image = imageSource;
        }
    }
}
