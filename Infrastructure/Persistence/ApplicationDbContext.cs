using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Domain.Section_Items;
using Domain.Sections;
using Domain.Users;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Section_Items> SectionItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}