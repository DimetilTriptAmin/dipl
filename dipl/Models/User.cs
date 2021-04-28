using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace dipl.Models
{
    class User : AbstractUser
    {
        ObservableCollection<Audio> PlayingQueue;
        List<Playlist> Playlists;

        private void LoadPlaylists()
        {

        }

        User()
        {
            LoadPlaylists();
        }
    }
}
