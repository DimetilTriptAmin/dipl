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
    class Playlist
    {
        public string Name { get; private set; }
        public ImageSource Image { get; set; }
        public ObservableCollection<Audio> Audios;

        public Playlist(string name)
        {
            Name = name;
            Audios = new ObservableCollection<Audio>();
            Image = new BitmapImage(new Uri("../../Assets/lp.jpg", UriKind.Relative));
        }
    }
}
