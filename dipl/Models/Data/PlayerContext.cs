using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dipl.Models.Data
{
    class PlayerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Audio> Audios { get; set; }
    }
}
