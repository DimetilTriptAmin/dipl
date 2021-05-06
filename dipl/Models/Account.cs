using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace dipl.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public string User { get; set; }
        public UserType UserType { get; set; }
        public ImageSource Image { get; set; }
        public ObservableCollection<Audio> Liked { get; set; }
        public ObservableCollection<Audio> Recent { get; set; }
        public ObservableCollection<Audio> Queue { get; set; }
        public ObservableCollection<Playlist> Playlists { get; set; }

    }
}
