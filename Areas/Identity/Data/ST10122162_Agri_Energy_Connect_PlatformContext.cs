using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ST10122162_Agri_Energy_Connect_Platform.Models;

namespace ST10122162_Agri_Energy_Connect_Platform.Data;

public class ST10122162_Agri_Energy_Connect_PlatformContext : IdentityDbContext<ApplicationUser>
{
    public ST10122162_Agri_Energy_Connect_PlatformContext(DbContextOptions<ST10122162_Agri_Energy_Connect_PlatformContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
