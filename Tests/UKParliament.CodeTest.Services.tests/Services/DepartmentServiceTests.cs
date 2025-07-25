using Moq;
using Shouldly;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Services.Dtos;
using UKParliament.CodeTest.Services.Mappers;

namespace UKParliament.CodeTest.Services.Tests.Services;

[TestFixture]
public class DepartmentServiceTests
{
    private Mock<IDepartmentRepository> _mockRepository;
    private Mock<IDepartmentServiceMapper> _mockMapper;
    private DepartmentService _service;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IDepartmentRepository>();
        _mockMapper = new Mock<IDepartmentServiceMapper>();
        _service = new DepartmentService(_mockRepository.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetAllAsync_WhenCalled_GetsDataFromRepository()
    {
        // Arrange
        var testDepartments = new List<Department> { new() { Id = 1, Name = "Test" } };
        _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(testDepartments);

        // Act
        await _service.GetAllAsync();

        // Assert
        _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetAllAsync_WhenCalled_MapsDataThroughMapper()
    {
        // Arrange
        var testDepartments = new List<Department> { new() { Id = 1, Name = "Test" } };
        var expectedDtos = new List<DepartmentDto> { new() { Id = 1, Name = "Test" } };

        _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(testDepartments);
        _mockMapper.Setup(x => x.ToDtos(testDepartments)).Returns(expectedDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        _mockMapper.Verify(x => x.ToDtos(testDepartments), Times.Once);
        result.ShouldBeEquivalentTo(expectedDtos);
    }

    [Test]
    public async Task GetAllAsync_WhenRepositoryReturnsEmpty_MapsEmptyCollection()
    {
        // Arrange
        var emptyDepartments = Enumerable.Empty<Department>();
        var emptyDtos = Enumerable.Empty<DepartmentDto>();

        _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(emptyDepartments);
        _mockMapper.Setup(x => x.ToDtos(emptyDepartments)).Returns(emptyDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.ShouldBeEmpty();
    }
}