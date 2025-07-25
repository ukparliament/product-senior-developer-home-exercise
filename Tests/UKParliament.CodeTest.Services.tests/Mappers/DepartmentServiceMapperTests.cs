using Shouldly;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Mappers;

namespace UKParliament.CodeTest.Services.Tests.Mappers;

[TestFixture]
public class DepartmentServiceMapperTests
{
    private DepartmentServiceMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mapper = new DepartmentServiceMapper();
    }

    [Test]
    public void ToDtos_WithNullInput_ReturnsEmptyCollection()
    {
        // Arrange
        IEnumerable<Department> nullDepartments = null;

        // Act
        var result = _mapper.ToDtos(nullDepartments);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Test]
    public void ToDtos_WithSingleDepartment_ReturnsCorrectDto()
    {
        // Arrange
        var departments = new List<Department>
        {
            new() { Id = 1, Name = "Test Department" }
        };

        // Act
        var result = _mapper.ToDtos(departments).ToList();

        // Assert
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(1);
        result[0].Name.ShouldBe("Test Department");
    }

    [Test]
    public void ToDtos_WithMultipleDepartments_ReturnsCorrectDtos()
    {
        // Arrange
        var departments = new List<Department>
        {
            new() { Id = 1, Name = "Department A" },
            new() { Id = 2, Name = "Department B" },
            new() { Id = 3, Name = "Department C" }
        };

        // Act
        var result = _mapper.ToDtos(departments).ToList();

        // Assert
        result.Count.ShouldBe(3);
        result.ShouldContain(d => d.Id == 1 && d.Name == "Department A");
        result.ShouldContain(d => d.Id == 2 && d.Name == "Department B");
        result.ShouldContain(d => d.Id == 3 && d.Name == "Department C");
    }
}