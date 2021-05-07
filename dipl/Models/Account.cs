using dipl.Models.Data;
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
        public User User { get; set; }
        public UserType UserType { get; set; }
        public byte[] Image { get; set; }
        public ObservableCollection<Playlist> Playlists { get; set; }

        public Account()
        {
        }
    }
}
