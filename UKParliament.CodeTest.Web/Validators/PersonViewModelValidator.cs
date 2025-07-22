using System.ComponentModel.DataAnnotations;
using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Validators;

public interface IPersonViewModelValidator
{
    Task<ValidationResult> ValidateAsync(PersonViewModel viewModel);
}

public class PersonViewModelValidator(IDepartmentRepository departmentRepository) : IPersonViewModelValidator
{
    public async Task<ValidationResult> ValidateAsync(PersonViewModel viewModel)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(viewModel.FirstName))
            result.Errors.Add("First name is required.");
        else if (viewModel.FirstName.Length > 50)
            result.Errors.Add("First name cannot exceed 50 characters.");

        if (string.IsNullOrWhiteSpace(viewModel.LastName))
            result.Errors.Add("Last name is required.");
        else if (viewModel.LastName.Length > 50)
            result.Errors.Add("Last name cannot exceed 50 characters.");

        if (viewModel.DateOfBirth == default)
            result.Errors.Add("Date of birth is required.");
        else if (viewModel.DateOfBirth > DateOnly.FromDateTime(DateTime.Today))
            result.Errors.Add("Date of birth cannot be in the future.");

        if (string.IsNullOrWhiteSpace(viewModel.Email) || !new EmailAddressAttribute().IsValid(viewModel.Email))
            result.Errors.Add("Invalid email format.");

        var departments = await departmentRepository.GetAllAsync();
        if (!departments.Any(d => d.Id == viewModel.DepartmentId))
            result.Errors.Add("Invalid department selected.");

        return result;
    }
}
