using Microsoft.EntityFrameworkCore;
using Shouldly;
using UKParliament.CodeTest.Data.Repositories;

namespace UKParliament.CodeTest.Data.Tests.Repositories;

[TestFixture]
public class PersonRepositoryTests
{
    private PersonManagerContext _context;
    private PersonRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<PersonManagerContext>()
            .UseInMemoryDatabase(databaseName: $"PersonTestDb_{Guid.NewGuid()}")
            .Options;

        _context = new PersonManagerContext(options);

        _context.Database.EnsureCreated();

        _repository = new PersonRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllPeopleWithDepartments()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
        result.ShouldAllBe(p => p.Department != null);
    }

    [Test]
    public async Task GetByIdAsync_ExistingId_ReturnsPersonWithDepartment()
    {
        // Arrange
        const int existingId = 1;

        // Act
        var result = await _repository.GetByIdAsync(existingId);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(existingId);
        result.FirstName.ShouldBe("John");
        result.Department.ShouldNotBeNull();
        result.Department.Name.ShouldBe("Sales");
    }

    [Test]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        const int nonExistingId = 999;

        // Act
        var result = await _repository.GetByIdAsync(nonExistingId);

        // Assert
        result.ShouldBeNull();
    }

    [Test]
    public async Task AddAsync_ValidPerson_AddsToDatabase()
    {
        // Arrange
        var newPerson = new Person
        {
            FirstName = "New",
            LastName = "Person",
            DateOfBirth = new DateOnly(2000, 1, 1),
            DepartmentId = 1,
            Email = "new.person@example.com"
        };

        // Act
        await _repository.AddAsync(newPerson);

        // Assert
        var dbPerson = await _context.People.FindAsync(newPerson.Id);
        dbPerson.ShouldNotBeNull();
        dbPerson.FirstName.ShouldBe("New");
    }

    [Test]
    public async Task UpdateAsync_ExistingPerson_UpdatesValues()
    {
        // Arrange
        var existingPerson = await _context.People.FindAsync(1);
        existingPerson.FirstName = "Updated";

        // Act
        await _repository.UpdateAsync(existingPerson);

        // Assert
        var updatedPerson = await _context.People.FindAsync(1);
        updatedPerson.FirstName.ShouldBe("Updated");
    }

    [Test]
    public async Task DeleteAsync_ExistingId_RemovesPerson()
    {
        // Arrange
        const int existingId = 1;

        // Act
        await _repository.DeleteAsync(existingId);

        // Assert
        var deletedPerson = await _context.People.FindAsync(existingId);
        deletedPerson.ShouldBeNull();
    }

    [Test]
    public void GetNextId_WithPeople_ReturnsMaxIdPlus1()
    {
        // Arrange
        var maxId = _context.People.Max(p => p.Id);

        // Act
        var result = _repository.GetNextId();

        // Assert
        result.ShouldBe(maxId + 1);
    }
}