namespace TriviaSharp.Data.Repositories;
using TriviaSharp.Models;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
}
