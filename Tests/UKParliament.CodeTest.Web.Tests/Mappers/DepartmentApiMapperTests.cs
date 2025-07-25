using Shouldly;
using UKParliament.CodeTest.Services.Dtos;
using UKParliament.CodeTest.Web.Mappers;

namespace UKParliament.CodeTest.Web.Tests.Mappers;

[TestFixture]
public class DepartmentApiMapperTests
{
    private DepartmentApiMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mapper = new DepartmentApiMapper();
    }

    [Test]
    public void ToViewModels_WithNullInput_ReturnsEmptyCollection()
    {
        // Arrange
        IEnumerable<DepartmentDto> nullDepartments = null;

        // Act
        var result = _mapper.ToViewModels(nullDepartments);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Test]
    public void ToViewModels_WithMultipleDepartments_ReturnsCorrectViewModels()
    {
        // Arrange
        var departments = new List<DepartmentDto>
        {
            new() { Id = 1, Name = "Department A" },
            new() { Id = 2, Name = "Department B" },
            new() { Id = 3, Name = "Department C" }
        };

        // Act
        var result = _mapper.ToViewModels(departments).ToList();

        // Assert
        result.Count.ShouldBe(3);
        result.ShouldContain(d => d.Id == 1 && d.Name == "Department A");
        result.ShouldContain(d => d.Id == 2 && d.Name == "Department B");
        result.ShouldContain(d => d.Id == 3 && d.Name == "Department C");
    }
}