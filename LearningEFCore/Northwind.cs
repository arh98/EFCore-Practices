using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningEFCore;
public class NorthWind : DbContext {
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

}

