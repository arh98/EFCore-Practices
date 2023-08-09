using LearningEFCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningEFCore;
public class Northwind : DbContext {
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Product>? Products { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

        if (Constants.DatabaseProvider == "SQLite") {
            string path = Path.Combine(
            Environment.CurrentDirectory, "Northwind.db");
            Console.WriteLine($"Using {path} database file.");
            optionsBuilder.UseSqlite($"Filename={path}");
        }
        else {
            string connection = "Data Source=.;" +
            "Initial Catalog=Northwind;" +
            "Integrated Security=true;" +
            "MultipleActiveResultSets=true;";
            optionsBuilder.UseSqlServer(connection);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        modelBuilder.Entity<Category>()
            .Property(c => c.CategoryName)
            .IsRequired()
            .HasMaxLength(20);

        //"fix" the lack of decimal support in SQLite
        modelBuilder.Entity<Product>()
            .Property(p => p.Cost)
            .HasConversion<double>();

        modelBuilder.Entity<Product>()
            .HasQueryFilter(p => !p.Discontinued);
    }

}

