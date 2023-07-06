using Microsoft.EntityFrameworkCore;
using Packt.Shared;

// See https://aka.ms/new-console-template for more information
Console.WriteLine($"Using {ProjectConstants.DataBaseProvider}");

static void QueryingCategories() {
  using (Northwind db = new()) {
    Console.WriteLine("Categories and how many products they have:");
    IQueryable<Category>? categories = db.Categories?.Include(c => c.Products);

    if ((categories is null) || (!categories.Any())) {
      return;
    }
     foreach (var c in categories) {
      Console.WriteLine($"{c.CategoryName} has {c.Products.Count} products.");
    }
  }
}

QueryingCategories();