namespace UnitOfWorkPractice.Repos
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T> GetByIdAsync(object id);
    }
}
