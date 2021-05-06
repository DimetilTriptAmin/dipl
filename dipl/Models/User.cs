using System.ComponentModel.DataAnnotations;

namespace dipl.Models
{
    public enum UserType
    {
        Guest,
        Regular,
        Admin
    }

    public class User
    {
        [Key]
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
