using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Models;
public class Academy : DbContext {
    public DbSet<Student>? Students { get; set; }
    public DbSet<Course>? Courses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

        optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=Academy;Integrated Security=true;MultipleActiveResultSets=true;");
        /*string path = Path.Combine(
        Environment.CurrentDirectory, "Academy.db");
        Console.WriteLine($"Using {path} database file.");
        optionsBuilder.UseSqlite($"Filename={path}");*/
    }
    protected override void OnModelCreating(ModelBuilder mBuilder) {

        mBuilder.Entity<Student>().Property(s => s.LastName).HasMaxLength(30).IsRequired();

        Student alice = new() {
            StudentId = 1,
            FirstName = "Alice",
            LastName = "Jones"
        };
        Student bob = new() {
            StudentId = 2,
            FirstName = "Bob",
            LastName = "Smith"
        };
        Student cecilia = new() {
            StudentId = 3,
            FirstName = "Cecilia",
            LastName = "Ramirez"
        };
        Course csharp = new() {
            CourseId = 1,
            Title = "C# 10 and .NET 6",
        };
        Course webdev = new() {
            CourseId = 2,
            Title = "Web Development",
        };
        Course python = new() {
            CourseId = 3,
            Title = "Python for Beginners",
        };

        mBuilder.Entity<Student>().HasData(alice, bob, cecilia);
        mBuilder.Entity<Course>().HasData(csharp, webdev, python);

        mBuilder.Entity<Course>()
            .HasMany(s => s.Students)
            .WithMany(c => c.Courses)
            .UsingEntity(e => e.HasData(
                // cs 
                new { CoursesCourseId = 1, StudentsStudentId = 1 },
                new { CoursesCourseId = 1, StudentsStudentId = 2 },
                new { CoursesCourseId = 1, StudentsStudentId = 3 },
                // Web Dev 
                new { CoursesCourseId = 2, StudentsStudentId = 2 },
                // Py
                new { CoursesCourseId = 3, StudentsStudentId = 3 }
                ));
    }
}

