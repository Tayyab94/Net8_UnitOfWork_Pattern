using Microsoft.EntityFrameworkCore;
using UnitOfWorkPractice.Models;

namespace UnitOfWorkPractice.Repos
{
    public class UserRepository :BaseRepository<User>, IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context):base(context) {
            this._context= context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<IEnumerable<User>> GetFeatureUserBySameName(string name)
        {
           return await _context.Users
                .Where(u => u.Name == name)
                .ToListAsync();
        }
    }
}
