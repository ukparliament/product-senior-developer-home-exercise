using UKParliament.CodeTest.Services.Dtos;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Mappers;

public interface IPersonApiMapper
{
    PersonViewModel ToViewModel(PersonDto person);
    PersonDto ToEntity(PersonViewModel viewModel);
    IEnumerable<PersonViewModel> ToViewModel(IEnumerable<PersonDto> persons);
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

    public PersonDto ToEntity(PersonViewModel viewModel)
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

    public IEnumerable<PersonViewModel> ToViewModel(IEnumerable<PersonDto> persons)
    {
        return persons.Select(person => new PersonViewModel
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DepartmentId = person.DepartmentId,
            DepartmentName = person.DepartmentName,
            Email = person.Email
        });
    }


}
