using LearningEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using LearningEFCore.Entities;

Console.WriteLine($"Using {Constants.DatabaseProvider} database provider.");

//QueringCategories();
//FilteredIncludes();
//QueringProducts();
//QueringProductsWithLike();

//if(addProduct(6, "Bob's Burgers", 500M)) {
//   Console.WriteLine("Product Added Succesfull");
//}

//if (increasePrice("Bob", 20M)) {
//   Console.WriteLine("Update product price successful.");
//}


//ListProducts();

int deleted = deleteProducts( "Bob");
Console.WriteLine($"{deleted} product(s) were deleted.");

//Quering
static void QueringCategories() {
    using (Northwind db = new()) {

        ILoggerFactory loggerFactory = db.GetService<ILoggerFactory>();
        loggerFactory.AddProvider(new ConsoleLoggerProvider());

        Console.WriteLine("Categories & Number of Their Products : ");
        var categories = db.Categories?.Include(c => c.Products);

        if (categories is not null) {
            foreach (var c in categories) {
                Console.WriteLine($"{c.CategoryName} has {c.Products.Count} products");
            }
            return;
        }
        Console.WriteLine("No categories found.");
    }
}

static void FilteredIncludes() {
    using (Northwind db = new()) {

        Console.WriteLine("Minimum of Units in Stock ?");
        string unitInStock = Console.ReadLine() ?? "9";
        int stock = int.Parse(unitInStock);

        var categories = db.Categories?.Include(c => c.Products.Where(p => p.Stock >= stock));

        if (categories is not null) {
            //Console.WriteLine($"ToQueryString: {categories.ToQueryString()}");
            foreach (var c in categories) {
                Console.WriteLine($"{c.CategoryName} has {c.Products.Count} products with a minimum of { stock} units in stock.");
                foreach (var p in c.Products) {
                    Console.WriteLine($"\t{p.ProductName} has {p.Stock} units in stock.");
                }
            }
            return;
        }
        Console.WriteLine("No categories found.");
    }
}

static void QueringProducts() {
    using Northwind db = new();

    ILoggerFactory loggerFactory = db.GetService<ILoggerFactory>();
    loggerFactory.AddProvider(new ConsoleLoggerProvider());

    Console.WriteLine("Products with that cost more that price , highest at top");
    string? input;
    decimal price;

    do {
        Console.WriteLine("product price? ");
        input = Console.ReadLine();
    } while (!decimal.TryParse(input, out price));

    var products = db.Products?
        .Where(p => p.Cost > price)
        .OrderByDescending(p => p.Cost);

    if (products is not null) {
        foreach (var p in products) {
            Console.WriteLine("{0}: {1} costs {2:$#,##0.00} and has {3} in stock."
                , p.ProductId, p.ProductName, p.Cost, p.Stock);
        }
        return;
    }
    Console.WriteLine("No products found.");
}

static void QueringProductsWithLike() {
    using Northwind db = new();

    Console.WriteLine("A part of product name ?");
    string? input = Console.ReadLine();

    var products = db.Products.Where(p => EF.Functions.Like(p.ProductName, $"%{input}%"));

    if (products is not null) {
        foreach (var p in products) {
            Console.WriteLine("{0} - {1} units in stock - Discontinued : {2}", p.ProductName, p.Stock, p.Discontinued);
        }
        return;
    }
    Console.WriteLine("No products found.");
}

static void ListProducts() {
    using Northwind db = new();

    var products = db.Products.OrderByDescending(p => p.Cost);

    Console.WriteLine("{0,-3} {1,-35} {2,8} {3,5} {4}", "Id", "Product Name", "Cost", "Stock", "Disc.");
    foreach (var p in products) {
        Console.WriteLine("{0:000} {1,-35} {2,8:$#,##0.00} {3,5} {4}", p.ProductId, p.ProductName, p.Cost, p.Stock, p.Discontinued);

    }
}

//inserting
static bool addProduct(int categoryId, string name, decimal price) {
    using Northwind db = new();

    Product p = new() {
        CategoryId = categoryId,
        ProductName = name,
        Cost = price
    };
    db.Products.Add(p);

    int affected = db.SaveChanges();
    return (affected == 1);
}

//updating
static bool increasePrice(string nameStartWith, decimal amount) {
    using Northwind db = new();

    Product productToUpdate = db.Products.First(p => p.ProductName.StartsWith(nameStartWith));
    productToUpdate.Cost += amount;

    int affected = db.SaveChanges();
    return (affected == 1);
}

//deleting
static int deleteProducts(string nameStartWith) {
    using Northwind db = new();

    int affected = -1;
    var productsToDelete = db.Products.Where(p => p.ProductName.StartsWith(nameStartWith));

    if (productsToDelete is not null) {
        db.Products.RemoveRange(productsToDelete);
        affected = db.SaveChanges();
        return affected;
    }
    Console.WriteLine("No products found to delete.");
    return 0;
}

static bool deleteProduct(string name) {
    using Northwind db = new();

    var productToDelete = db.Products.First(p => p.ProductName == name);

    if (productToDelete is not null) {
        // Delete related records in Order Details table , remove() doesn't delete related records
        //var orderDetailsToDelete = db.OrderDetails.Where(od => od.ProductID == productToDelete.ProductID);
        //db.OrderDetails.RemoveRange(orderDetailsToDelete);

        db.Products.Remove(productToDelete);
        return  db.SaveChanges() == 1;
    }
    return false;
}