namespace Shop.Web.Data.Repository
{
    using System.Linq;
    using Entities;

    public interface IProductRepository : IGenericRepository<Product>
    {
        IQueryable<Product> GetAllWithUsers();
    }
}
