using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Security;

namespace dipl.Models.Data
{
    class DataHandler
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
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return null;
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
                    catch (Exception ex)
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
                Image = System.Text.Encoding.UTF8.GetBytes("../../Assets/anime.jpg"),
                Playlists = new ObservableCollection<Playlist>()
            };
            account.Playlists.Add(new Playlist("Liked") { PlaylistId = 0});
            account.Playlists.Add(new Playlist("Queue") { PlaylistId = 1 });
            account.Playlists.Add(new Playlist("Recent") { PlaylistId = 2 });


            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        pc.Playlists.AddRange(account.Playlists);
                        pc.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Playlists ON");
                        pc.Users.Add(user);
                        pc.Accounts.Add(account);
                        pc.SaveChanges();
                        transaction.Commit();
                        isRegistered = true;

                    }
                    catch (Exception ex)
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
                        pc.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Playlists ON");
                        pc.Playlists.Add(playlist);
                        pc.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public static bool DeletePlaylist(Playlist playlistToRemove)
        {
            using (PlayerContext pc = new PlayerContext())
            {
                using (var transaction = pc.Database.BeginTransaction())
                {
                    try
                    {
                        Playlist playlist = pc.Playlists.Find(playlistToRemove.AccountId, playlistToRemove.PlaylistId);
                        pc.Playlists.Remove(playlist);
                        pc.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
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
                        Playlist playlistToUpdate = pc.Playlists.Find(oldplaylist.AccountId, oldplaylist.PlaylistId);
                        playlistToUpdate.Name = newplaylist.Name;
                        playlistToUpdate.Audios = newplaylist.Audios;
                        playlistToUpdate.Image = newplaylist.Image;
                        pc.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
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
                    catch (Exception ex)
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
                    catch (Exception ex)
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
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        //public static ObservableCollection<Audio> GetLiked(Account account)
        //{
        //    using (PlayerContext pc = new PlayerContext())
        //    {
        //        using (var transaction = pc.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                ObservableCollection<Audio> Liked = new ObservableCollection<Audio>();
        //                var result = pc.Accounts
        //                     .Include(x => x.Playlists.Select(y => y.Audios))
        //                     .Where(x => x.AccountId == account.AccountId);
        //                transaction.Commit();
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                return new ObservableCollection<Audio>();
        //            }
        //        }
        //    }
        //}
    }
}
