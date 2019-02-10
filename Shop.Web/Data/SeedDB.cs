namespace Shop.Web.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;

    public class SeedDb
    {
        private readonly DataContext context;
        private Random random;

        public SeedDb(DataContext context)
        {
            this.context = context;
            random = new Random();
        }

        public async Task SeedAsync()
        {
            await context.Database.EnsureCreatedAsync();

            if (!context.Products.Any())
            {
                AddProduct("iPhone X");
                AddProduct("Magic Mouse");
                AddProduct("iWatch Series 4");
                await context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name)
        {
            context.Products.Add(new Product
            {
                Name = name,
                Price = random.Next(1000),
                IsAvailabe = true,
                Stock = random.Next(100)
            });
        }
    }
}
