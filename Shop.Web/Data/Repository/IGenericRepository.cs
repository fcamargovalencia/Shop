namespace Shop.Web.Data.Repository
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IGenericRepository<T>
        where T : class
    {

        IQueryable<T> GetAll();

        Task<T> GetById(int id);

        Task Create(T entity);

        Task Update(T entity);

        Task Delete(T entity);

        Task<bool> Exist(int id);
    }
}