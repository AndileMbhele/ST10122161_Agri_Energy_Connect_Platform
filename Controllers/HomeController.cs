using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10122162_Agri_Energy_Connect_Platform.Data;
using ST10122162_Agri_Energy_Connect_Platform.Models;
using ST10122162_Agri_Energy_Connect_Platform.ViewModels;

namespace ST10122162_Agri_Energy_Connect_Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            // If user is authenticated, show role-specific dashboard
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                var dashboardViewModel = new DashboardViewModel();

                if (User.IsInRole("Farmer"))
                {
                    // Farmer dashboard
                    if (user?.FarmerId != null)
                    {
                        dashboardViewModel.Farmer = await _context.Farmers
                            .FirstOrDefaultAsync(f => f.Id == user.FarmerId);

                        dashboardViewModel.RecentProducts = await _context.Products
                            .Where(p => p.FarmerId == user.FarmerId)
                            .OrderByDescending(p => p.DateAdded)
                            .Take(5)
                            .ToListAsync();

                        dashboardViewModel.TotalProducts = await _context.Products
                            .CountAsync(p => p.FarmerId == user.FarmerId);
                    }
                }
                else if (User.IsInRole("Employee"))
                {
                    // Employee dashboard
                    dashboardViewModel.TotalFarmers = await _context.Farmers.CountAsync();
                    dashboardViewModel.TotalProducts = await _context.Products.CountAsync();

                    dashboardViewModel.RecentFarmers = await _context.Farmers
                        .OrderByDescending(f => f.RegistrationDate)
                        .Take(5)
                        .ToListAsync();

                    dashboardViewModel.RecentProducts = await _context.Products
                        .Include(p => p.Farmer)
                        .OrderByDescending(p => p.DateAdded)
                        .Take(5)
                        .ToListAsync();
                }

                return View("Dashboard", dashboardViewModel);
            }

            // For non-authenticated users, show the default welcome page
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Credentials()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
