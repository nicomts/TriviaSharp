namespace TriviaSharp.Data.Repositories;
using TriviaSharp.Models;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
}