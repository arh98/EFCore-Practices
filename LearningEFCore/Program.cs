using LearningEFCore;
using LearningEFCore.AutoGen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

Console.WriteLine($"Using {Constants.DatabaseProvider} database provider.");
//QueringCategories();
FilteredIncludes();


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