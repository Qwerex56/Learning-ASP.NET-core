using static System.Console;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Packt.Shared;

static async void FilterAndSort()
{
  using (Northwind db = new())
  {
    if (db.Products is null)
    {
      return;
    }

    DbSet<Product> allProducts = db.Products;
    IQueryable<Product> filteredProducts = allProducts.Where(product => product.UnitPrice < 10M);
    IOrderedQueryable<Product> filteredAndSortedProducts = filteredProducts.OrderByDescending(product => product.UnitPrice);

    var projectedProducts = filteredAndSortedProducts.Select(
      product => new
      {
        product.ProductId,
        product.ProductName,
        product.UnitPrice
      });

    WriteLine("Products that cost less than 10$:");
    // foreach (var p in filteredAndSortedProducts) {
    //   WriteLine("{0}: {1} costs {2:$#,##0.00}",
    //     p.ProductId, p.ProductName, p.UnitPrice);
    // }
    await filteredAndSortedProducts.ForEachAsync(p =>
    {
      WriteLine("{0}: {1} costs {2:$#,##0.00}",
        p.ProductId, p.ProductName, p.UnitPrice);
    });
    WriteLine();
  }
}

static void JoinCategoriesAndProducts()
{
  using (Northwind db = new())
  {
    var queryJoin = db.Categories.Join(
      inner: db.Products,
      outerKeySelector: category => category.CategoryId,
      innerKeySelector: product => product.CategoryId,
      resultSelector: (c, p) => new
      {
        c.CategoryName,
        p.ProductName,
        p.ProductId
      })
      .OrderBy(cp => cp.CategoryName);

    foreach (var item in queryJoin)
    {
      WriteLine("{0}: {1} is in {2}.",
        arg0: item.ProductId,
        arg1: item.ProductName,
        arg2: item.CategoryName);
    }
  }
}

static void GroupJoinCategoriesAndProducts()
{
  using (Northwind db = new())
  {
    var queryGroup = db.Categories.AsEnumerable().GroupJoin(
      inner: db.Products,
      outerKeySelector: category => category.CategoryId,
      innerKeySelector: product => product.CategoryId,
      resultSelector: (c, mathcingProducts) => new
      {
        c.CategoryName,
        Products = mathcingProducts.OrderBy(p => p.ProductName)
      });

    foreach (var category in queryGroup)
    {
      WriteLine("{0} has {1} products.",
        arg0: category.CategoryName,
        arg1: category.Products.Count());
      foreach (var product in category.Products)
      {
        WriteLine($" {product.ProductName}");
      }
    }
  }
}

static void AggregateProducts()
{
  using (Northwind db = new())
  {
    WriteLine("{0,-25} {1,10}",
      arg0: "Product count:",
      arg1: db.Products.Count());
    WriteLine("{0,-25} {1,10:$#,##0.00}",
      arg0: "Highest product price:",
      arg1: db.Products.Max(p => p.UnitPrice));
    WriteLine("{0,-25} {1,10:N0}",
      arg0: "Sum of units in stock:",
      arg1: db.Products.Sum(p => p.UnitsInStock));
    WriteLine("{0,-25} {1,10:N0}",
      arg0: "Sum of units on order:",
      arg1: db.Products.Sum(p => p.UnitsOnOrder));
    WriteLine("{0,-25} {1,10:$#,##0.00}",
      arg0: "Average unit price:",
      arg1: db.Products.Average(p => p.UnitPrice));
    WriteLine("{0,-25} {1,10:$#,##0.00}",
      arg0: "Value of units in stock:",
      arg1: db.Products
        .Sum(p => p.UnitPrice * p.UnitsInStock));
  }
}

static void OutputProductsXML()
{
  using (Northwind db = new())
  {
    Product[] products = db.Products.ToArray();
    XElement xml = new(
      "products",
      from p in products
      select new XElement("product",
      new XAttribute("id", p.ProductId),
      new XAttribute("price", p.UnitPrice),
      new XElement("name", p.ProductName)));
    WriteLine(xml.ToString());
  }
}

static void ProcessingSettings()
{
  XDocument doc = XDocument.Load("settings.xml");
  var appSettings = doc.Descendants("appSettings")
    .Descendants("add")
    .Select(node => new {
      Key = node.Attribute("key")?.Value,
      Value = node.Attribute("value")?.Value
    }).ToArray();
  foreach (var item in appSettings)
  {
    WriteLine($"{item.Key}: {item.Value}");
  }
}

// FilterAndSort();
// JoinCategoriesAndProducts();
// GroupJoinCategoriesAndProducts();
// AggregateProducts();
// OutputProductsXML();
ProcessingSettings();