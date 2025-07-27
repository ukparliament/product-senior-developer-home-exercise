using Microsoft.EntityFrameworkCore;
using Shouldly;
using UKParliament.CodeTest.Data.Repositories;

namespace UKParliament.CodeTest.Data.Tests.Repositories;
[TestFixture]
public class DepartmentRepositoryTests
{
    private PersonManagerContext _context;
    private DepartmentRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<PersonManagerContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new PersonManagerContext(options);

        _context.Database.EnsureCreated();

        _repository = new DepartmentRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllSeededDepartments()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(4);
        result.Select(d => d.Name).ShouldContain("Sales");
        result.Select(d => d.Name).ShouldContain("Marketing");
    }
}
