using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data;

public class PersonManagerContext : DbContext
{
    public PersonManagerContext(DbContextOptions<PersonManagerContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Sales" },
            new Department { Id = 2, Name = "Marketing" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "HR" });

        modelBuilder.Entity<Person>().HasData(
            new Person
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1990, 2, 15),
                DepartmentId = 1, 
                Email = "john.doe@example.com"
            },
            new Person
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateOnly(1993, 5, 22),
                DepartmentId = 2, 
                Email = "jane.smith@example.com"
            });
    }

    public DbSet<Person> People { get; set; }

    public DbSet<Department> Departments { get; set; }
}