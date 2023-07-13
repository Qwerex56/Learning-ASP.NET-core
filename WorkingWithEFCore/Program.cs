using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Packt.Shared;

// See https://aka.ms/new-console-template for more information
Console.WriteLine($"Using {ProjectConstants.DataBaseProvider}");

static void QueryingCategories()
{
  using (Northwind db = new())
  {
    ILoggerFactory loggerFactory = db.GetService<ILoggerFactory>();
    loggerFactory.AddProvider(new ConsoleLoggerProvider());

    Console.WriteLine("Categories and how many products they have:");
    IQueryable<Category>? categories = db.Categories; // ?.Include(c => c.Products);

    db.ChangeTracker.LazyLoadingEnabled = false;

    Console.Write("Enable eager loading? ");

    bool eagerLoading = (Console.ReadKey().Key == ConsoleKey.Y);
    bool explicitLoading = false;

    Console.WriteLine();

    if (eagerLoading)
    {
      categories = db.Categories?.Include(c => c.Products);
    }
    else
    {
      categories = db.Categories;
      Console.Write("Enable explicit loading? ");
      explicitLoading = (Console.ReadKey().Key == ConsoleKey.Y);
      Console.WriteLine();
    }

    if ((categories is null) || (!categories.Any()))
    {
      return;
    }
    foreach (var c in categories)
    {
      Console.Write($"Explicitly load products for {c.CategoryName}? (Y/N): ");
      ConsoleKeyInfo key = Console.ReadKey();
      Console.WriteLine();

      if (key.Key == ConsoleKey.Y)
      {
        CollectionEntry<Category, Product> products =
          db.Entry(c).Collection(c2 => c2.Products);
        if (!products.IsLoaded) products.Load();
      }
      Console.WriteLine($"{c.CategoryName} has {c.Products.Count} products.");
    }
  }
}

static void FilteredIncludes()
{
  using (Northwind db = new())
  {
    Console.Write("Enter a minimum for units in stock: ");
    string unitsInStock = Console.ReadLine() ?? "10";
    int stock = int.Parse(unitsInStock);
    IQueryable<Category>? categories = db.Categories?.Include(c => c.Products.Where(p => p.Stock >= stock));

    if (categories is null)
    {
      return;
    }

    Console.WriteLine($"To query string: {categories.ToQueryString()}");
    foreach (var c in categories)
    {
      Console.WriteLine($"{c.CategoryName} has {c.Products.Count} products with a minimum of {stock} units in stock.");
      foreach (var p in c.Products)
      {
        Console.WriteLine($"  {p.ProductName} has {p.Stock} units in stock.");
      }
    }
  }
}
static void QueryfingProducts()
{
  using (Northwind db = new())
  {
    ILoggerFactory loggerFactory = db.GetService<ILoggerFactory>();
    loggerFactory.AddProvider(new ConsoleLoggerProvider());

    string? input;
    decimal price;
    do
    {
      Console.Write("Enter product price: ");
      input = Console.ReadLine();
    } while (!decimal.TryParse(input, out price));

    // Equivalent of SELECT from SQL database
    IQueryable<Product>? products = db.Products?
      .TagWith("Products filtereed by price and sorted")
      .Where(product => product.Cost > price)
      .OrderByDescending(product => product.Cost);

    if (products is null)
    {
      return;
    }
    foreach (var p in products)
    {
      Console.WriteLine(
        "{0}: {1} costs {2:$#,##0.00} and has {3} in stock.",
        p.ProductId, p.ProductName, p.Cost, p.Stock);
    }
  }
}

static void QueryfyingWithLike()
{
  using (Northwind db = new())
  {
    ILoggerFactory loggerFactory = db.GetService<ILoggerFactory>();
    loggerFactory.AddProvider(new ConsoleLoggerProvider());

    Console.Write("Enter part of product name: ");
    string? input = Console.ReadLine();

    IQueryable<Product>? products = db.Products?
      .Where(p => EF.Functions.Like(p.ProductName, $"%{input}%"));

    if (products is null)
    {
      return;
    }

    foreach (Product p in products)
    {
      Console.WriteLine("{0} has {1} units in stock. Discontinued? {2}",
        p.ProductName, p.Stock, p.Discontinued);
    }
  }
}

static bool AddProduct(
  int categoryId, string productName, decimal? price
)
{
  using (Northwind db = new())
  {
    Product p = new()
    {
      CategoryId = categoryId,
      ProductName = productName,
      Cost = price,
    };

    db.Products.Add(p);

    int affected = db.SaveChanges();
    return affected == 1;
  }
}

static void ListProducts()
{
  using (Northwind db = new())
  {
    Console.WriteLine("{0,-3} {1,-35} {2,8} {3,5} {4}",
      "Id", "Product Name", "Cost", "Stock", "Disc.");
    foreach (Product p in db.Products
      .OrderByDescending(product => product.Cost))
    {
      Console.WriteLine("{0:000} {1,-35} {2,8:$#,##0.00} {3,5} {4}",
        p.ProductId, p.ProductName, p.Cost, p.Stock, p.Discontinued);
    }
  }
}

// QueryingCategories();
// FilteredIncludes();
// QueryfingProducts();
// QueryfyingWithLike();

if (AddProduct(categoryId: 6, productName: "Bob's Burger", price: 500M))
{
  Console.WriteLine("Succesfully added");
}

ListProducts();