using Microsoft.Extensions.Logging;
using UnitOfWorkPractice.Models;

namespace UnitOfWorkPractice.Repos
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
   

        public ProductRepository(AppDbContext context):base(context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }
   
        public Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count)
        {
            throw new NotImplementedException();
        }
    }
}
