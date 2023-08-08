using LearningEFCore;
using LearningEFCore.AutoGen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

Console.WriteLine($"Using {Constants.DatabaseProvider} database provider.");
//QueringCategories();
//FilteredIncludes();
QueringProducts();

static void QueringCategories() {
    using (NorthWind db = new()) {

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
    using (NorthWind db = new()) {

        Console.WriteLine("Minimum of Units in Stock ?");
        string unitInStock = Console.ReadLine() ?? "9";
        int stock = int.Parse(unitInStock);

        var categories = db.Categories?.Include(c => c.Products.Where(p => p.Stock >= stock));

        if (categories is not null) {
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
    using (NorthWind db = new()) {
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
}