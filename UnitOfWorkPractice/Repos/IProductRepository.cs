using UnitOfWorkPractice.Models;

namespace UnitOfWorkPractice.Repos
{
    public interface IProductRepository: IRepository<Product>
    {
        // Add the base methods you need
        Task AddAsync(Product product);
        Task<Product> GetByIdAsync(object id);

        Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count);
    }
}
