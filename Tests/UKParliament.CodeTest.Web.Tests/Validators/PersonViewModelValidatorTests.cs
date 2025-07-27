using FluentValidation.TestHelper;
using Shouldly;
using UKParliament.CodeTest.Web.Validators;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Tests.Validators;

[TestFixture]
public class PersonViewModelValidatorTests
{
    private PersonViewModelValidator _validator;
    private PersonViewModel _validModel;

    [SetUp]
    public void Setup()
    {
        _validator = new PersonViewModelValidator();
        _validModel = new PersonViewModel
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
            DepartmentId = 1,
            Email = "john.doe@example.com"
        };
    }

    [Test]
    public void FirstName_WhenEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _validModel.FirstName = string.Empty;

        // Act & Assert
        var result = _validator.TestValidate(_validModel);
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name is required.");
    }

    [Test]
    public void FirstName_WhenTooShort_ShouldHaveValidationError()
    {
        // Arrange
        _validModel.FirstName = "J";

        // Act & Assert
        var result = _validator.TestValidate(_validModel);
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name must be 2-50 characters.");
    }

    [Test]
    public void FirstName_WhenTooLong_ShouldHaveValidationError()
    {
        // Arrange
        _validModel.FirstName = new string('A', 51);

        // Act & Assert
        var result = _validator.TestValidate(_validModel);
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name must be 2-50 characters.");
    }

    [Test]
    public void FirstName_WhenContainsNumbers_ShouldHaveValidationError()
    {
        // Arrange
        _validModel.FirstName = "John1";

        // Act & Assert
        var result = _validator.TestValidate(_validModel);
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name can only contain letters and spaces.");
    }

    [Test]
    public void FirstName_WhenValid_ShouldNotHaveValidationError()
    {
        // Arrange
        _validModel.FirstName = "John";

        // Act & Assert
        var result = _validator.TestValidate(_validModel);
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
    }

    [Test]
    public void DateOfBirth_WhenUnder18_ShouldHaveValidationError()
    {
        // Arrange
        _validModel.DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-17));

        // Act & Assert
        var result = _validator.TestValidate(_validModel);
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
            .WithErrorMessage("Person must be at least 18 years old.");
    }

    [Test]
    public void DateOfBirth_WhenExactly18_ShouldNotHaveValidationError()
    {
        // Arrange
        _validModel.DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-18));

        // Act & Assert
        var result = _validator.TestValidate(_validModel);
        result.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Test]
    public void DateOfBirth_WhenOver18_ShouldNotHaveValidationError()
    {
        // Arrange
        _validModel.DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-19));

        // Act & Assert
        var result = _validator.TestValidate(_validModel);
        result.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Test]
    public void DepartmentId_WhenZero_ShouldHaveValidationError()
    {
        // Arrange
        _validModel.DepartmentId = 0;

        // Act & Assert
        var result = _validator.TestValidate(_validModel);
        result.ShouldHaveValidationErrorFor(x => x.DepartmentId)
            .WithErrorMessage("Department is required.");
    }

    [Test]
    public void DepartmentId_WhenPositive_ShouldNotHaveValidationError()
    {
        // Arrange
        _validModel.DepartmentId = 1;

        // Act & Assert
        var result = _validator.TestValidate(_validModel);
        result.ShouldNotHaveValidationErrorFor(x => x.DepartmentId);
    }

    [Test]
    public void Email_WhenInvalidFormat_ShouldHaveValidationError()
    {
        // Arrange
        _validModel.Email = "not-an-email";

        // Act & Assert
        var result = _validator.TestValidate(_validModel);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email format.");
    }

    [Test]
    public void ValidModel_ShouldPassAllValidations()
    {
        // Act & Assert
        var result = _validator.TestValidate(_validModel);
        result.IsValid.ShouldBeTrue();
    }
}