using Microsoft.EntityFrameworkCore;
using CodeFirst.Models;


using (Academy db = new()) {

    bool deleted = await db.Database.EnsureDeletedAsync();
    Console.WriteLine($"Database deleted: {deleted}");

    bool created = await db.Database.EnsureCreatedAsync();
    Console.WriteLine($"Database created: {created}");

    Console.WriteLine("SQL Scripts used to created DataBase : ");
    Console.WriteLine(db.Database.GenerateCreateScript());

    foreach (Student s in db.Students.Include(s => s.Courses)) {

        Console.WriteLine("{0} {1} attends the following {2} courses:", s.FirstName, s.LastName, s.Courses.Count);

        foreach (Course c in s.Courses) {
            Console.WriteLine($" {c.Title}");
        }
    }
}