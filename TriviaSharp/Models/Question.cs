using SQLite;

namespace TriviaSharp.Models
{
    public class Question
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int QuestionSetId { get; set; } // Foreign key

        [NotNull]
        public string Category { get; set; }

        [NotNull]
        public string Difficulty { get; set; } // "High", "Medium", "Low"

        [NotNull]
        public string Text { get; set; }
    }
}
