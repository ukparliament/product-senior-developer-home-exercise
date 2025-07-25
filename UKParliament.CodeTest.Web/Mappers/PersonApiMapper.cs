using UKParliament.CodeTest.Services.Dtos;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Mappers;

public interface IPersonApiMapper
{
    PersonViewModel ToViewModel(PersonDto person);
    PersonDto ToDto(PersonViewModel viewModel);
    IEnumerable<PersonViewModel> ToViewModels(IEnumerable<PersonDto> people);
}

public class PersonApiMapper : IPersonApiMapper
{
    public PersonViewModel ToViewModel(PersonDto person)
    {
        return new PersonViewModel
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DepartmentId = person.DepartmentId,
            DepartmentName = person.DepartmentName,
            Email = person.Email
        };
    }

    public PersonDto ToDto(PersonViewModel viewModel)
    {
        return new PersonDto
        {
            Id = viewModel.Id,
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            DateOfBirth = viewModel.DateOfBirth,
            DepartmentId = viewModel.DepartmentId,
            Email = viewModel.Email
        };
    }

    public IEnumerable<PersonViewModel> ToViewModels(IEnumerable<PersonDto> people)
    {
        if (people is null)
        {
            return [];
        }
        return people.Select(ToViewModel);
    }
}
