using SQLite;

namespace TriviaSharp.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique, NotNull]
        public string Username { get; set; }

        [NotNull]
        public string PasswordHash { get; set; } // Store hashed password only
    }
}
