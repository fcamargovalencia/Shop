namespace Shop.Web.Data.Repository
{
    using System.Linq;
    using Entities;
    using Microsoft.EntityFrameworkCore;

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext context;

        public ProductRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IQueryable<Product> GetAllWithUsers()
        {
            return this.context.Products.Include(p => p.User);
        }
    }
}
