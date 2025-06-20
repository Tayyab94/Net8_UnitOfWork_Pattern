using UnitOfWorkPractice.Models;

namespace UnitOfWorkPractice.Repos
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetFeatureUserBySameName(string name);
    }
}
