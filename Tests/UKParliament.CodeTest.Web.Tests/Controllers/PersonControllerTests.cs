using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Dtos;
using UKParliament.CodeTest.Web.Controllers;
using UKParliament.CodeTest.Web.Mappers;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Tests.Controllers;

[TestFixture]
public class PersonControllerTests
{
    private Mock<IPersonService> _mockService;
    private Mock<IValidator<PersonViewModel>> _mockValidator;
    private Mock<IPersonApiMapper> _mockMapper;
    private PersonController _controller;

    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<IPersonService>();
        _mockValidator = new Mock<IValidator<PersonViewModel>>();
        _mockMapper = new Mock<IPersonApiMapper>();
        _controller = new PersonController(
            _mockService.Object,
            _mockValidator.Object,
            _mockMapper.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOkWithMappedPersons()
    {
        // Arrange
        var testDtos = new List<PersonDto> { new() { Id = 1 } };
        var expectedViewModels = new List<PersonViewModel> { new() { Id = 1 } };

        _mockService.Setup(x => x.GetAllAsync()).ReturnsAsync(testDtos);
        _mockMapper.Setup(x => x.ToViewModels(testDtos)).Returns(expectedViewModels);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.ShouldBe(expectedViewModels);
    }

    [Test]
    public async Task GetById_ExistingId_ReturnsOkWithPerson()
    {
        // Arrange
        var testDto = new PersonDto { Id = 1 };
        var expectedViewModel = new PersonViewModel { Id = 1 };

        _mockService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(testDto);
        _mockMapper.Setup(x => x.ToViewModel(testDto)).Returns(expectedViewModel);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.ShouldBe(expectedViewModel);
    }

    [Test]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((PersonDto)null);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Add_ValidModel_ReturnsCreatedResponse()
    {
        // Arrange
        var viewModel = new PersonViewModel { FirstName = "Test" };
        var dto = new PersonDto { FirstName = "Test" };
        var createdDto = new PersonDto { Id = 5, FirstName = "Test" };
        var responseViewModel = new PersonViewModel { Id = 5, FirstName = "Test" };

        _mockValidator.Setup(x => x.ValidateAsync(viewModel, default))
            .ReturnsAsync(new ValidationResult());
        _mockMapper.Setup(x => x.ToDto(viewModel)).Returns(dto);
        _mockService.Setup(x => x.AddAsync(dto)).ReturnsAsync(createdDto);
        _mockMapper.Setup(x => x.ToViewModel(createdDto)).Returns(responseViewModel);

        // Act
        var result = await _controller.Add(viewModel);

        // Assert
        result.Result.ShouldBeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.ActionName.ShouldBe(nameof(PersonController.GetById));
        createdResult.RouteValues["id"].ShouldBe(5);
        createdResult.Value.ShouldBe(responseViewModel);
    }

    [Test]
    public async Task Add_InvalidModel_ReturnsBadRequestWithErrors()
    {
        // Arrange
        var viewModel = new PersonViewModel();
        var validationErrors = new List<ValidationFailure>
        {
            new() { PropertyName = "FirstName", ErrorMessage = "Required" }
        };

        _mockValidator.Setup(x => x.ValidateAsync(viewModel, default))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _controller.Add(viewModel);

        // Assert
        result.Result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = result.Result as BadRequestObjectResult;
        (badRequestResult.Value as IEnumerable<string>).ShouldContain("Required");
    }

    [Test]
    public async Task Update_ValidModel_ReturnsNoContent()
    {
        // Arrange
        var viewModel = new PersonViewModel { Id = 1 };
        var dto = new PersonDto { Id = 1 };

        _mockValidator.Setup(x => x.ValidateAsync(viewModel, default))
            .ReturnsAsync(new ValidationResult());
        _mockMapper.Setup(x => x.ToDto(viewModel)).Returns(dto);
        _mockService.Setup(x => x.UpdateAsync(dto)).ReturnsAsync(true);

        // Act
        var result = await _controller.Update(1, viewModel);

        // Assert
        result.Result.ShouldBeOfType<NoContentResult>();
    }

    [Test]
    public async Task Update_IdMismatch_ReturnsBadRequest()
    {
        // Arrange
        var viewModel = new PersonViewModel { Id = 2 };

        // Act
        var result = await _controller.Update(1, viewModel);

        // Assert
        result.Result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Value.ShouldBe("ID mismatch");
    }

    [Test]
    public async Task Update_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var viewModel = new PersonViewModel { Id = 1 };
        var validationErrors = new List<ValidationFailure>
        {
            new() { PropertyName = "FirstName", ErrorMessage = "Required" }
        };

        _mockValidator.Setup(x => x.ValidateAsync(viewModel, default))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _controller.Update(1, viewModel);

        // Assert
        result.Result.ShouldBeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task Update_NonExistingPerson_ReturnsNotFound()
    {
        // Arrange
        var viewModel = new PersonViewModel { Id = 1 };
        var dto = new PersonDto { Id = 1 };

        _mockValidator.Setup(x => x.ValidateAsync(viewModel, default))
            .ReturnsAsync(new ValidationResult());
        _mockMapper.Setup(x => x.ToDto(viewModel)).Returns(dto);
        _mockService.Setup(x => x.UpdateAsync(dto)).ReturnsAsync(false);

        // Act
        var result = await _controller.Update(1, viewModel);

        // Assert
        result.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        // Arrange
        _mockService.Setup(x => x.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.ShouldBeOfType<NoContentResult>();
    }

    [Test]
    public async Task Delete_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(x => x.DeleteAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.ShouldBeOfType<NotFoundResult>();
    }
}