# TriviaSharp
Trivia game that fetches questions from OpenTDB.

# Diagrams
## Class Diagram
```mermaid
classDiagram
    class Answer {
        +int Id
        +string Text
        +bool IsCorrect
    }
    class Category {
        +int Id
        +string Name
    }
    class LoginResult {
        +LoginStatus Status
        +User User
    }
    class Question {
        +int Id
        +string Text
        +Category Category
        +List<Answer> Answers
    }
    class QuestionSet {
        +int Id
        +string Name
        +List<Question> Questions
    }
    class QuizSession {
        +int Id
        +User User
        +QuestionSet QuestionSet
        +int Score
    }
    class User {
        +int Id
        +string Username
        +string PasswordHash
        +UserRole Role
    }
    class ApiCategory {
        +int Id
        +string Name
    }
    class ApiCategoryResponse {
        +List<ApiCategory> Categories
    }
    class ApiQuestion {
        +string Category
        +string Type
        +string Difficulty
        +string Question
        +string CorrectAnswer
        +List<string> IncorrectAnswers
    }
    class ApiResponse {
        +int ResponseCode
        +List<ApiQuestion> Results
    }
    class OpenTdbFetcher {
        +Task<ApiResponse> FetchQuestions()
        +Task<ApiCategoryResponse> FetchCategories()
    }
    class OpenTdbService {
        +Task<List<Question>> GetQuestions()
        +Task<List<Category>> GetCategories()
    }
    class QuizService {
        +Task<Question> GetQuiz()
        +Task<int> CalculateScore()
    }
    class UserService {
        +Task<bool> Register(string, string)
        +Task<LoginResult> Login(string, string)
        +Task<ChangePasswordStatus> ChangePassword(User, string)
    }
    class TriviaDbContext {
        +DbSet<User> Users
        +DbSet<Category> Categories
        +DbSet<Question> Questions
        +DbSet<Answer> Answers
        +DbSet<QuestionSet> QuestionSets
        +DbSet<QuizSession> QuizSessions
    }

    Question "0..*" -- "1" Category
    Question *-- Answer
    QuestionSet *-- Question
    QuizSession *-- User
    QuizSession *-- QuestionSet
    LoginResult "1" -- "0..1" User
    ApiCategoryResponse "1" -- "0..*" ApiCategory
    ApiResponse "1" -- "0..*" ApiQuestion
    OpenTdbFetcher "1" -- "0..1" ApiResponse
    OpenTdbFetcher "1" -- "0..1" ApiCategoryResponse
    OpenTdbService "1" -- "0..*" Question
    OpenTdbService "1" -- "0..*" Category
    QuizService "1" -- "0..*" QuizSession
    QuizService "1" -- "0..*" Question
    UserService "1" -- "1" LoginResult
    TriviaDbContext "1" -- "0..*" User
    TriviaDbContext "1" -- "0..*" Category
    TriviaDbContext "1" -- "0..*" Question
    TriviaDbContext "1" -- "0..*" Answer
    TriviaDbContext "1" -- "0..*" QuestionSet
    TriviaDbContext "1" -- "0..*" QuizSession
``` 
