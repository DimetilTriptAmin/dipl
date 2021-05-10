using dipl.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace dipl.Models.Data
{
    public class PlayerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Audio> Audios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Playlist>().HasKey(p => new { p.AccountId, p.PlaylistId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
