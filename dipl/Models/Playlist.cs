using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace dipl.Models
{
    class Playlist
    {
        public string Name { get; private set; }
        public ImageSource Image { get; set; }
        public List<Audio> Audios;

        public Playlist(string name)
        {
            Name = name;
            Audios = new List<Audio>();
        }
    }
}
