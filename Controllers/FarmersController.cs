using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10122162_Agri_Energy_Connect_Platform.Data;
using ST10122162_Agri_Energy_Connect_Platform.Models;
using ST10122162_Agri_Energy_Connect_Platform.ViewModels;

namespace ST10122162_Agri_Energy_Connect_Platform.Controllers
{
    [Authorize(Roles = "Employee")]
    public class FarmersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FarmersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Farmers
        public async Task<IActionResult> Index()
        {
            var farmers = await _context.Farmers
                .Include(f => f.Products)
                .ToListAsync();
            return View(farmers);
        }

        // GET: Farmers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .Include(f => f.Products)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        // GET: Farmers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Farmers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FarmerCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Create the farmer
                var farmer = new Farmer
                {
                    FarmName = viewModel.FarmName,
                    Address = viewModel.Address,
                    ContactNumber = viewModel.ContactNumber,
                    RegistrationDate = DateTime.Now
                };

                _context.Add(farmer);
                await _context.SaveChangesAsync();

                // Create the user account for the farmer if requested
                if (viewModel.CreateAccount)
                {
                    var user = new ApplicationUser
                    {
                        UserName = viewModel.Email,
                        Email = viewModel.Email,
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        FarmerId = farmer.Id,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(user, viewModel.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Farmer");
                    }
                    else
                    {
                        // If user creation fails, show errors but keep the farmer record
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(viewModel);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Farmers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }
            return View(farmer);
        }

        // POST: Farmers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FarmName,Address,ContactNumber")] Farmer farmer)
        {
            if (id != farmer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing farmer to preserve the registration date
                    var existingFarmer = await _context.Farmers.FindAsync(id);
                    if (existingFarmer == null)
                    {
                        return NotFound();
                    }

                    // Update only the editable properties
                    existingFarmer.FarmName = farmer.FarmName;
                    existingFarmer.Address = farmer.Address;
                    existingFarmer.ContactNumber = farmer.ContactNumber;

                    _context.Update(existingFarmer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmerExists(farmer.Id))
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
            return View(farmer);
        }

        // GET: Farmers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        // POST: Farmers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer != null)
            {
                _context.Farmers.Remove(farmer);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool FarmerExists(string id)
        {
            return _context.Farmers.Any(e => e.Id == id);
        }
    }
}
