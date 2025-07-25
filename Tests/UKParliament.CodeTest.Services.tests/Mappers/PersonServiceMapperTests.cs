using Shouldly;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Dtos;
using UKParliament.CodeTest.Services.Mappers;

namespace UKParliament.CodeTest.Services.Tests.Mappers;

[TestFixture]
public class PersonServiceMapperTests
{
    private PersonServiceMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mapper = new PersonServiceMapper();
    }

    [Test]
    public void ToDto_WithValidPerson_ReturnsCorrectDto()
    {
        // Arrange
        var person = new Person
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateOnly(1990, 1, 1),
            DepartmentId = 1,
            Department = new Department { Id = 1, Name = "Sales" },
            Email = "john.doe@example.com"
        };

        // Act
        var result = _mapper.ToDto(person);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(1);
        result.FirstName.ShouldBe("John");
        result.LastName.ShouldBe("Doe");
        result.DateOfBirth.ShouldBe(new DateOnly(1990, 1, 1));
        result.DepartmentId.ShouldBe(1);
        result.DepartmentName.ShouldBe("Sales");
        result.Email.ShouldBe("john.doe@example.com");
    }

    [Test]
    public void ToEntity_WithNullDto_ReturnsNull()
    {
        // Act
        var result = _mapper.ToEntity(null);

        // Assert
        result.ShouldBeNull();
    }

    [Test]
    public void ToEntity_WithValidDto_ReturnsCorrectPerson()
    {
        // Arrange
        var dto = new PersonDto
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Smith",
            DateOfBirth = new DateOnly(1995, 5, 15),
            DepartmentId = 2,
            Email = "jane.smith@example.com"
        };

        // Act
        var result = _mapper.ToEntity(dto);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(1);
        result.FirstName.ShouldBe("Jane");
        result.LastName.ShouldBe("Smith");
        result.DateOfBirth.ShouldBe(new DateOnly(1995, 5, 15));
        result.DepartmentId.ShouldBe(2);
        result.Email.ShouldBe("jane.smith@example.com");
    }

    [Test]
    public void ToDtos_WithSinglePerson_ReturnsCorrectDto()
    {
        // Arrange
        var persons = new List<Person>
        {
            new()
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateOnly(2000, 1, 1),
                DepartmentId = 1,
                Department = new Department { Name = "HR" },
                Email = "test@example.com"
            }
        };

        // Act
        var result = _mapper.ToDtos(persons).ToList();

        // Assert
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(1);
        result[0].FirstName.ShouldBe("Test");
        result[0].DepartmentName.ShouldBe("HR");
    }

    [Test]
    public void ToDtos_WithMultiplePersons_ReturnsCorrectDtos()
    {
        // Arrange
        var persons = new List<Person>
        {
            new()
            {
                Id = 1,
                FirstName = "Alice",
                DepartmentId = 1,
                Department = new Department { Name = "Sales" }
            },
            new()
            {
                Id = 2,
                FirstName = "Bob",
                DepartmentId = 2,
                Department = null
            }
        };

        // Act
        var result = _mapper.ToDtos(persons).ToList();

        // Assert
        result.Count.ShouldBe(2);
        result[0].FirstName.ShouldBe("Alice");
        result[0].DepartmentName.ShouldBe("Sales");
        result[1].FirstName.ShouldBe("Bob");
        result[1].DepartmentName.ShouldBeNull();
    }
}