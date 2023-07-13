using Microsoft.EntityFrameworkCore;
using static System.Console;

namespace Packt.Shared;

public class Northwind : DbContext
{
  public DbSet<Category>? Categories { get; set; }
  public DbSet<Product>? Products { get; set; }
  
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseLazyLoadingProxies();
    if (ProjectConstants.DataBaseProvider == "SQLite") {
      string path = Path.Combine(Environment.CurrentDirectory, "Northwind.db");
      WriteLine($"using {path} database file");
      optionsBuilder.UseSqlite($"Filename={path}");
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Category>()
      .Property(category => category.CategoryName)
      .IsRequired()
      .HasMaxLength(15);
    if (ProjectConstants.DataBaseProvider == "SQLite") {
      modelBuilder.Entity<Product>()
        .Property(product => product.Cost)
        .HasConversion<double>();
    }

    // Global Filter
    modelBuilder.Entity<Product>()
      .HasQueryFilter(p => !p.Discontinued);
  }
}