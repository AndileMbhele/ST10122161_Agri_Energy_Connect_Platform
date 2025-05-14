using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10122162_Agri_Energy_Connect_Platform.Data;
using ST10122162_Agri_Energy_Connect_Platform.Models;
using ST10122162_Agri_Energy_Connect_Platform.ViewModels;
using System.Security.Claims;

namespace ST10122162_Agri_Energy_Connect_Platform.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index(string farmerId = null, string category = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var user = await _userManager.GetUserAsync(User);
            var isEmployee = User.IsInRole("Employee");
            var isFarmer = User.IsInRole("Farmer");

            // Create a base query
            var productsQuery = _context.Products
                .Include(p => p.Farmer)
                .AsQueryable();

            // For farmers, only show their own products
            if (isFarmer && !isEmployee)
            {
                if (user?.FarmerId == null)
                {
                    return View(new List<Product>());
                }
                productsQuery = productsQuery.Where(p => p.FarmerId == user.FarmerId);
            }
            // For employees with a specific farmer selected
            else if (isEmployee && !string.IsNullOrEmpty(farmerId))
            {
                productsQuery = productsQuery.Where(p => p.FarmerId == farmerId);
            }

            // Apply category filter if provided
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                productsQuery = productsQuery.Where(p => p.Category == category);
            }

            // Apply date range filter if provided
            if (startDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate <= endDate.Value);
            }

            // Get the list of farmers for the dropdown (for employees only)
            if (isEmployee)
            {
                ViewBag.Farmers = new SelectList(await _context.Farmers.ToListAsync(), "Id", "FarmName");
            }

            // Get the list of categories for the dropdown
            ViewBag.Categories = new SelectList(new[] { "All" }.Concat(ProductCategory.Categories));

            // Set filter values for the view
            ViewBag.SelectedFarmerId = farmerId;
            ViewBag.SelectedCategory = category;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(await productsQuery.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Check if the user has permission to view this product
            if (!await UserCanAccessProduct(product))
            {
                return Forbid();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.FarmerId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Create a new product with default values
            var product = new Product
            {
                ProductionDate = DateTime.Now,
                Unit = "kg",
                Quantity = 1
            };

            ViewBag.Categories = new SelectList(ProductCategory.Categories);
            return View(product);
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create([Bind("Name,Category,ProductionDate,Description,Quantity,Unit")] Product product)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.FarmerId == null)
            {
                // Log the issue for debugging
                Console.WriteLine("User FarmerId is null");
                ModelState.AddModelError("", "Your account is not linked to a farm. Please contact an employee for assistance.");
                ViewBag.Categories = new SelectList(ProductCategory.Categories, product.Category);
                return View(product);
            }

            // Set the FarmerId and DateAdded
            product.FarmerId = user.FarmerId;
            product.DateAdded = DateTime.Now;

            // Clear any existing errors for FarmerId since we're setting it manually
            ModelState.Remove("FarmerId");

            // Check if the model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging
                    Console.WriteLine($"Exception: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while saving the product. Please try again.");
                }
            }
            else
            {
                // Log validation errors for debugging
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        Console.WriteLine($"Error in {state.Key}: {state.Value.Errors[0].ErrorMessage}");
                    }
                }
            }

            ViewBag.Categories = new SelectList(ProductCategory.Categories, product.Category);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Check if the user has permission to edit this product
            if (!await UserCanAccessProduct(product))
            {
                return Forbid();
            }

            ViewBag.Categories = new SelectList(ProductCategory.Categories, product.Category);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,ProductionDate,Description,Quantity,Unit,FarmerId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            // Check if the user has permission to edit this product
            var originalProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (originalProduct == null || !await UserCanAccessProduct(originalProduct))
            {
                return Forbid();
            }

            // Ensure the FarmerId hasn't been tampered with
            if (originalProduct.FarmerId != product.FarmerId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Preserve the DateAdded value
                    product.DateAdded = originalProduct.DateAdded;

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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

            ViewBag.Categories = new SelectList(ProductCategory.Categories, product.Category);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Check if the user has permission to delete this product
            if (!await UserCanAccessProduct(product))
            {
                return Forbid();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Check if the user has permission to delete this product
            if (!await UserCanAccessProduct(product))
            {
                return Forbid();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private async Task<bool> UserCanAccessProduct(Product product)
        {
            // Employees can view any product
            if (User.IsInRole("Employee"))
            {
                // For GET requests (viewing), employees can access any product
                if (HttpContext.Request.Method == "GET")
                {
                    return true;
                }

                // For POST/PUT/DELETE requests (modifying), employees cannot modify products
                return false;
            }

            // Farmers can only access their own products
            if (User.IsInRole("Farmer"))
            {
                var user = await _userManager.GetUserAsync(User);
                return user?.FarmerId == product.FarmerId;
            }

            return false;
        }
    }
}
