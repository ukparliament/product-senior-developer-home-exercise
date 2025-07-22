using FluentValidation;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Validators;

public class PersonViewModelValidator: AbstractValidator<PersonViewModel>
{
    public PersonViewModelValidator()
    {
        RuleFor(x => x.FirstName)
             .NotEmpty().WithMessage("First name is required.")
             .Length(2, 50).WithMessage("First name must be 2-50 characters.")
             .Matches(@"^[a-zA-Z\s]+$").WithMessage("First name can only contain letters and spaces.");

        RuleFor(x => x.LastName)
           .NotEmpty().WithMessage("Last name is required.")
           .Length(2, 50).WithMessage("Last name must be 2-50 characters.")
           .Matches(@"^[a-zA-Z\s]+$").WithMessage("Last name can only contain letters and spaces.");

        RuleFor(x => x.DateOfBirth)
           .NotEmpty().WithMessage("Date of birth is required.")
           .Must(BeAtLeast18YearsOld).WithMessage("Person must be at least 18 years old.");

        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Department is required.")
            .GreaterThan(0).WithMessage("Invalid department selected.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");
    }

    private bool BeAtLeast18YearsOld(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age)) age--;
        return age >= 18;
    }
}
