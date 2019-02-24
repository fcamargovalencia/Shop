namespace Shop.Web.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Entities;
    using Data.Repository;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models;

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
        public async Task<IActionResult> Create(ProductViewModel view)
        {
            if (this.ModelState.IsValid)
            {
                var path = string.Empty;

                if (view.ImageFile != null && view.ImageFile.Length > 0)
                {
                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Products",
                        view.ImageFile.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(stream);
                    }
                    path = $"~/images/Products/{view.ImageFile.FileName}";
                }

                var product = ToProduct(view, path);
                //TODO: Change for the logged user
                product.User = await this.userHelper.GetUserByEmailAsync("fabiancv.90@gmail.com");
                await this.repository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(view);
        }

        private Product ToProduct(ProductViewModel view, string path)
        {
            return new Product
            {
                Id = view.Id,
                ImageUrl = path,
                IsAvailabe = view.IsAvailabe,
                LastPurchase = view.LastPurchase,
                LastSale = view.LastSale,
                Name = view.Name,
                Price = view.Price,
                Stock = view.Stock,
                User = view.User
            };
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
            var view = ToProductViewModel(product);
            return View(view);
        }

        private ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                ImageUrl = product.ImageUrl,
                IsAvailabe = product.IsAvailabe,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User
            };
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel view)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var path = view.ImageUrl;

                    if (view.ImageFile != null && view.ImageFile.Length > 0)
                    {
                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\Products",
                            view.ImageFile.FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await view.ImageFile.CopyToAsync(stream);
                        }
                        path = $"~/images/Products/{view.ImageFile.FileName}";
                    }

                    var product = ToProduct(view, path);
                    //TODO: Change for the logged user
                    product.User = await this.userHelper.GetUserByEmailAsync("fabiancv.90@gmail.com");
                    await this.repository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.repository.ExistAsync(view.Id))
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
            return View(view);
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
