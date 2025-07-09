using SQLite;

namespace TriviaSharp.Models
{
    public class QuestionSet
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        public string Source { get; set; } // e.g., "OpenTDB"
    }
}
