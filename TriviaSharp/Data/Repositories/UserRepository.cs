namespace TriviaSharp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using TriviaSharp.Models;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(TriviaDbContext context) : base(context) { }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
    public async Task Update(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
