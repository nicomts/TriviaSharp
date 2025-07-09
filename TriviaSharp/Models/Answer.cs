using SQLite;

namespace TriviaSharp.Models
{
    public class Answer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int QuestionId { get; set; } // Foreign key

        [NotNull]
        public string Text { get; set; }

        [NotNull]
        public bool IsCorrect { get; set; }
    }
}
