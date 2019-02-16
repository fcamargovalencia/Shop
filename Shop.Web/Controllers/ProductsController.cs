namespace Shop.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Entities;
    using Data.Repository;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class ProductsController : Controller
    {
        private readonly IProductRepository repository;
        private readonly IUserHelper userHelper;

        public ProductsController(IProductRepository repository, IUserHelper userHelper)
        {
            this.repository = repository;
            this.userHelper = userHelper;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await this.repository.GetAll().OrderBy(p => p.Name).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await this.repository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (this.ModelState.IsValid)
            {
                //TODO: Change for the logged user
                product.User = await this.userHelper.GetUserByEmailAsync("fabiancv.90@gmail.com");
                await this.repository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.repository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    //TODO: Change for the logged user
                    product.User = await this.userHelper.GetUserByEmailAsync("fabiancv.90@gmail.com");
                    await this.repository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.repository.ExistAsync(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.repository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await this.repository.GetByIdAsync(id);
            await this.repository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
