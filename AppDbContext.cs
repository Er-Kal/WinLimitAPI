using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WinLimitAPI;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    // Creates the BlockItems table using EntityFrameworkCore
    public DbSet<BlockItem> BlockItems {get;set;}
    public DbSet<BlockItem> RecommendedBlockApps {get;set;}
}