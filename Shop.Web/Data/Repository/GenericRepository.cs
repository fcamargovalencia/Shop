namespace Shop.Web.Data.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Entities;

    public class GenericRepository<T> : IGenericRepository<T>
        where T : class, IEntity
    {
        private readonly DataContext _dbContext;
        public GenericRepository(DataContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IQueryable<T> GetAll()
        {
            return this._dbContext.Set<T>().AsNoTracking();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await this._dbContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateAsync(T entity)
        {
            await this._dbContext.Set<T>().AddAsync(entity);
            await SaveAllAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            this._dbContext.Set<T>().Update(entity);
            await SaveAllAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            this._dbContext.Set<T>().Remove(entity);
            await SaveAllAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await this._dbContext.Set<T>().AnyAsync(e => e.Id == id);

        }

        public async Task<bool> SaveAllAsync()
        {
            return await this._dbContext.SaveChangesAsync() > 0;
        }
    }
}
