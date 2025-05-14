using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ST10122162_Agri_Energy_Connect_Platform.Models;

namespace ST10122162_Agri_Energy_Connect_Platform.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("DbInitializer");
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    // Ensure database is created and apply migrations
                    context.Database.Migrate();

                    // Create roles if they don't exist
                    await CreateRoles(roleManager);

                    // Create sample users and data
                    await CreateSampleData(context, userManager);

                    logger.LogInformation("Database initialization completed successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database.");
            }
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Farmer", "Employee" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task CreateSampleData(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            // Create sample employee user if it doesn't exist
            if (await userManager.FindByEmailAsync("employee@agrienergyconnect.com") == null)
            {
                var employee = new ApplicationUser
                {
                    UserName = "employee@agrienergyconnect.com",
                    Email = "employee@agrienergyconnect.com",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User"
                };

                var result = await userManager.CreateAsync(employee, "Employee1!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(employee, "Employee");
                }
            }

            // Create sample farmers and their products if no farmers exist
            if (!context.Farmers.Any())
            {
                // Create first farmer
                var farmer1 = new Farmer
                {
                    FarmName = "Green Valley Farm",
                    Address = "123 Farm Road, Farmville",
                    ContactNumber = "0123456789",
                    RegistrationDate = DateTime.Now.AddMonths(-6)
                };

                context.Farmers.Add(farmer1);
                await context.SaveChangesAsync();

                // Create user for first farmer
                var farmerUser1 = new ApplicationUser
                {
                    UserName = "farmer1@agrienergyconnect.com",
                    Email = "farmer1@agrienergyconnect.com",
                    EmailConfirmed = true,
                    FirstName = "John",
                    LastName = "Farmer",
                    FarmerId = farmer1.Id,
                };

                var result1 = await userManager.CreateAsync(farmerUser1, "Farmer1!");
                if (result1.Succeeded)
                {
                    await userManager.AddToRoleAsync(farmerUser1, "Farmer");
                }

                // Create products for first farmer
                var products1 = new List<Product>
                {
                    new Product
                    {
                        Name = "Organic Apples",
                        Category = "Fruits",
                        ProductionDate = DateTime.Now.AddDays(-10),
                        Description = "Fresh organic apples from Green Valley Farm",
                        Quantity = 100,
                        Unit = "kg",
                        FarmerId = farmer1.Id
                    },
                    new Product
                    {
                        Name = "Sweet Corn",
                        Category = "Vegetables",
                        ProductionDate = DateTime.Now.AddDays(-5),
                        Description = "Locally grown sweet corn",
                        Quantity = 50,
                        Unit = "kg",
                        FarmerId = farmer1.Id
                    }
                };

                context.Products.AddRange(products1);

                // Create second farmer
                var farmer2 = new Farmer
                {
                    FarmName = "Sunrise Organic Farm",
                    Address = "456 Country Lane, Ruraltown",
                    ContactNumber = "0987654321",
                    RegistrationDate = DateTime.Now.AddMonths(-3)
                };

                context.Farmers.Add(farmer2);
                await context.SaveChangesAsync();

                // Create user for second farmer
                var farmerUser2 = new ApplicationUser
                {
                    UserName = "farmer2@agrienergyconnect.com",
                    Email = "farmer2@agrienergyconnect.com",
                    EmailConfirmed = true,
                    FirstName = "Sarah",
                    LastName = "Smith",
                    FarmerId = farmer2.Id,
                };

                var result2 = await userManager.CreateAsync(farmerUser2, "Farmer2!");
                if (result2.Succeeded)
                {
                    await userManager.AddToRoleAsync(farmerUser2, "Farmer");
                }

                // Create products for second farmer
                var products2 = new List<Product>
                {
                    new Product
                    {
                        Name = "Free-Range Eggs",
                        Category = "Eggs",
                        ProductionDate = DateTime.Now.AddDays(-2),
                        Description = "Free-range eggs from happy chickens",
                        Quantity = 200,
                        Unit = "dozen",
                        FarmerId = farmer2.Id
                    },
                    new Product
                    {
                        Name = "Organic Milk",
                        Category = "Dairy",
                        ProductionDate = DateTime.Now.AddDays(-1),
                        Description = "Fresh organic milk",
                        Quantity = 100,
                        Unit = "liter",
                        FarmerId = farmer2.Id
                    }
                };

                context.Products.AddRange(products2);
                await context.SaveChangesAsync();
            }
        }
    }
}
