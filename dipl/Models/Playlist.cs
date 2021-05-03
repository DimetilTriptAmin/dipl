using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace dipl.Models
{
    public class Playlist
    {
        public string Name { get; set; }
        public ImageSource Image { get; set; }
        public ObservableCollection<Audio> Audios { get; set; }

        public Playlist(string name)
        {
            Name = name;
            Audios = new ObservableCollection<Audio>();
            // TODO: Добавить картинку по умолчанию
            Image = new BitmapImage(new Uri("../../Assets/lp.jpg", UriKind.Relative));
        }

        public Playlist(string name, ObservableCollection<Audio> audios, ImageSource imageSource)
        {
            Name = name;
            Audios = audios;
            Image = imageSource;
        }
    }
}
