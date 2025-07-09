using SQLite;

namespace TriviaSharp.Models
{
    public class QuizSessionAnswer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int QuizSessionId { get; set; }

        [NotNull]
        public int QuestionId { get; set; }

        [NotNull]
        public int AnswerId { get; set; }
    }
}
