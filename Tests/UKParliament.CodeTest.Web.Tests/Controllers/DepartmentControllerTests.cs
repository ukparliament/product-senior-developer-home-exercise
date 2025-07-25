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
public class DepartmentControllerTests
{
    private Mock<IDepartmentService> _mockService;
    private DepartmentApiMapper _mapper;
    private DepartmentController _controller;

    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<IDepartmentService>();
        _mapper = new DepartmentApiMapper();
        _controller = new DepartmentController(_mockService.Object, _mapper);
    }

    [Test]
    public async Task GetAll_WhenCalled_ReturnsOkResultWithDepartments()
    {
        // Arrange  
        var testDepartments = new List<DepartmentDto>
        {
            new() { Id = 1, Name = "Sales" },
            new() { Id = 2, Name = "Marketing" }
        };

        var viewModels = _mapper.ToViewModels(testDepartments);
        _mockService.Setup(x => x.GetAllAsync())
            .ReturnsAsync(testDepartments);

        // Act  
        var result = await _controller.GetAll();

        // Assert  
        result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();

        var okResult = result.Result as OkObjectResult;
        okResult.Value.ShouldBeEquivalentTo(viewModels);

        _mockService.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetAll_WhenServiceReturnsEmpty_ReturnsOkWithEmptyList()
    {
        // Arrange  
        _mockService.Setup(x => x.GetAllAsync())
            .ReturnsAsync([]);

        // Act  
        var result = await _controller.GetAll();

        // Assert  
        result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<OkObjectResult>();

        var okResult = result.Result as OkObjectResult;
        (okResult.Value as IEnumerable<DepartmentViewModel>).ShouldBeEmpty();
    }
}