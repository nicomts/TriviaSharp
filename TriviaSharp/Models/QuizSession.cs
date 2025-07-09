using SQLite;
using System;

namespace TriviaSharp.Models
{
    public class QuizSession
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int UserId { get; set; }

        [NotNull]
        public int QuestionSetId { get; set; }

        [NotNull]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int Score { get; set; }
    }
}
