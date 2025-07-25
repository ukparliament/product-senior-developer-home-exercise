using Moq;
using Shouldly;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Services.Dtos;
using UKParliament.CodeTest.Services.Mappers;

namespace UKParliament.CodeTest.Services.Tests.Services;

[TestFixture]
public class PersonServiceTests
{
    private Mock<IPersonRepository> _mockRepository;
    private Mock<IPersonServiceMapper> _mockMapper;
    private PersonService _service;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IPersonRepository>();
        _mockMapper = new Mock<IPersonServiceMapper>();
        _service = new PersonService(_mockRepository.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetAllAsync_ReturnsMappedPersons()
    {
        // Arrange
        var testPersons = new List<Person> { new() { Id = 1 } };
        var expectedDtos = new List<PersonDto> { new() { Id = 1 } };

        _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(testPersons);
        _mockMapper.Setup(x => x.ToDtos(testPersons)).Returns(expectedDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.ShouldBe(expectedDtos);
        _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
        _mockMapper.Verify(x => x.ToDtos(testPersons), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_WithExistingId_ReturnsMappedPerson()
    {
        // Arrange
        var testPerson = new Person { Id = 1 };
        var expectedDto = new PersonDto { Id = 1 };

        _mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(testPerson);
        _mockMapper.Setup(x => x.ToDto(testPerson)).Returns(expectedDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.ShouldBe(expectedDto);
    }

    [Test]
    public async Task AddAsync_ValidPerson_ReturnsDtoWithNewId()
    {
        // Arrange
        var inputDto = new PersonDto { FirstName = "Test" };
        var mappedPerson = new Person { FirstName = "Test" };
        var savedPerson = new Person { Id = 5, FirstName = "Test" };

        _mockMapper.Setup(x => x.ToEntity(inputDto)).Returns(mappedPerson);
        _mockRepository.Setup(x => x.GetNextId()).Returns(5);
        _mockRepository.Setup(x => x.AddAsync(It.Is<Person>(p => p.Id == 5)))
            .Returns(Task.CompletedTask)
            .Callback<Person>(p => savedPerson = p);

        // Act
        var result = await _service.AddAsync(inputDto);

        // Assert
        result.Id.ShouldBe(5);
        result.FirstName.ShouldBe("Test");
        _mockRepository.Verify(x => x.AddAsync(It.Is<Person>(p => p.Id == 5)), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_ExistingPerson_UpdatesAndReturnsTrue()
    {
        // Arrange
        var existingPerson = new Person
        {
            Id = 1,
            FirstName = "Old",
            LastName = "Name",
            Department = new Department()
        };
        var updateDto = new PersonDto
        {
            Id = 1,
            FirstName = "New",
            LastName = "Name",
            DateOfBirth = new DateOnly(2000, 1, 1),
            DepartmentId = 2,
            Email = "new@test.com"
        };

        _mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(existingPerson);
        _mockRepository.Setup(x => x.UpdateAsync(existingPerson)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateAsync(updateDto);

        // Assert
        result.ShouldBeTrue();
        existingPerson.FirstName.ShouldBe("New");
        existingPerson.LastName.ShouldBe("Name");
        existingPerson.DateOfBirth.ShouldBe(new DateOnly(2000, 1, 1));
        existingPerson.DepartmentId.ShouldBe(2);
        existingPerson.Email.ShouldBe("new@test.com");
        existingPerson.Department.ShouldBeNull();
        _mockRepository.Verify(x => x.UpdateAsync(existingPerson), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_ExistingPerson_DeletesAndReturnsTrue()
    {
        // Arrange
        var existingPerson = new Person { Id = 1 };
        _mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(existingPerson);
        _mockRepository.Setup(x => x.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.ShouldBeTrue();
        _mockRepository.Verify(x => x.DeleteAsync(1), Times.Once);
    }
}