using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Security;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace dipl.Models.Data
{
    public static class DataHandler
    {
        public static User CheckUser(string username, SecureString password)
        {
            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        string hash = HashGenerator.GetHash(password);
                        var user = pc.Users.Where(u => u.Username == username).Where(u => u.PasswordHash == hash).FirstOrDefault();
                        transaction.Commit();
                        return user;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
            }
        }

        public static bool DeleteAccount(Account accountToDelete)
        {
            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        var account = pc.Accounts
                            .Include(x => x.Playlists.Select(y => y.Audios))
                            .Include(x => x.User)
                            .Where(x => x.AccountId == accountToDelete.AccountId).FirstOrDefault();
                        foreach (Playlist playlist in account.Playlists)
                        {
                            pc.Audios.RemoveRange(playlist.Audios);
                        }
                        pc.Playlists.RemoveRange(account.Playlists);
                        pc.Users.Remove(account.User);
                        pc.Accounts.Remove(account);
                        pc.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        } 

        public static IQueryable<Account> GetAccounts()
        {
            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        var accounts = pc.Accounts
                            .Include(x => x.Playlists.Select(y => y.Audios))
                            .Include(x => x.User)
                            .Where(a => a.User.Username != "admin");
                        transaction.Commit();
                        return accounts;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return (IQueryable<Account>)(new ObservableCollection<Account>());
                    }
                }
            }
        }

        public static Account GetAccount(User user)
        {
            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        var account = pc.Accounts
                            .Include(x => x.Playlists.Select(y => y.Audios))
                            .Include(x => x.User)
                            .Where(a => a.User.Username == user.Username)
                            .FirstOrDefault();
                        transaction.Commit();
                        return account;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
            }
        }

        public static bool Register(User user)
        {
            bool isRegistered = false;

            Account account = new Account()
            {
                User = user,
                UserType = UserType.Regular,
                Image = ((ImageSource)(new BitmapImage(new Uri("../../Assets/account-default.jpg", UriKind.Relative)))).ToBytes(),
            Playlists = new ObservableCollection<Playlist>()
            };
            account.Playlists.Add(new Playlist("Queue") { PlaylistId = 0 });
            account.Playlists.Add(new Playlist("Recent") { PlaylistId = 1 });


            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        pc.Playlists.AddRange(account.Playlists);
                        pc.Users.Add(user);
                        pc.Accounts.Add(account);
                        pc.SaveChanges();
                        transaction.Commit();
                        isRegistered = true;

                    }
                    catch
                    {
                        isRegistered = false;
                    }
                }
            }
            return isRegistered;
        }

        public static bool AddPlaylist(Playlist playlist)
        {
            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        var result = pc.Playlists.Where(p => p.AccountId == App.CurrentAccount.AccountId);
                        int i = 2;
                        foreach (Playlist pl in result)
                        {
                            if (i == pl.PlaylistId) i++;
                        }
                        playlist.PlaylistId = i;
                        pc.Playlists.Add(playlist);
                        pc.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public static bool DeletePlaylist(Playlist playlistToRemove, bool IsDelete)
        {
            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        Playlist playlist = pc.Playlists
                            .Include(p=>p.Audios)
                            .Where(p=> p.AccountId == playlistToRemove.AccountId && p.PlaylistId==playlistToRemove.PlaylistId).FirstOrDefault();
                        pc.Audios.RemoveRange(playlist.Audios);
                        if (IsDelete)
                            pc.Playlists.Remove(playlist);
                        pc.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public static bool AddAudio(Playlist playlist, Audio audio)
        {
            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        Playlist playlistToUpdate = pc.Playlists
                            .Include(p => p.Audios)
                            .Where(p => p.AccountId == playlist.AccountId && p.PlaylistId == playlist.PlaylistId).FirstOrDefault();
                        playlistToUpdate.Audios.Add(audio);
                        pc.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public static bool UpdatePlaylist(Playlist oldplaylist, Playlist newplaylist)
        {
            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        Playlist playlistToUpdate = pc.Playlists
                            .Include(p=>p.Audios)
                            .Where(p => p.AccountId == oldplaylist.AccountId && p.PlaylistId == oldplaylist.PlaylistId).FirstOrDefault();
                        playlistToUpdate.Name = newplaylist.Name;
                        pc.Audios.RemoveRange(playlistToUpdate.Audios);
                        playlistToUpdate.Audios = newplaylist.Audios;
                        playlistToUpdate.Image = newplaylist.Image;
                        pc.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public static bool UpdateAudio(Audio audioToUpdate)
        {
            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        Audio audio = pc.Audios.Find(audioToUpdate.Id);
                        audio.IsLiked = audioToUpdate.IsLiked;
                        pc.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public static bool UpdateProfilePic(Account accountToUpdate, byte[] image)
        {
            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        Account account = pc.Accounts.Find(accountToUpdate.AccountId);
                        account.Image = image;
                        pc.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public static bool UpdateProfilePassword(User userToUpdate, SecureString newPassword)
        {
            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        User user = pc.Users.Find(userToUpdate.Username);
                        user.PasswordHash = HashGenerator.GetHash(newPassword);
                        pc.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public static bool DeleteAudio(Audio audioToRemove)
        {
            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        Audio audio = pc.Audios.Where(a => a.Id == audioToRemove.Id).FirstOrDefault();
                        pc.Audios.Remove(audio);
                        pc.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public static ObservableCollection<Audio> LoadLiked()
        {
            ObservableCollection<Audio> Liked = new ObservableCollection<Audio>();
            List<string> urls = new List<string>();
            foreach (Playlist playlist in App.CurrentAccount.Playlists)
            {
                foreach(Audio audio in playlist.Audios)
                {
                    if (audio.IsLiked && !urls.Contains(audio.SourceUrl))
                    {
                        urls.Add(audio.SourceUrl);
                        Liked.Add(audio);
                    }
                }
            }
            return Liked;
        }
    }
}
