using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace dipl.Models
{
    public class Account
    {
        public string User { get; set; }
        public UserType UserType { get; set; }
        public ImageSource Image { get; set; }
        public Playlist Liked { get; set; }
        public Playlist Queue { get; set; }
        public ObservableCollection<Playlist> Playlists { get; set; }
    }
}
