using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Mappers;

public interface IMapper
{
    PersonViewModel ToViewModel(Person person);
    Person ToEntity(PersonViewModel viewModel);
    IEnumerable<PersonViewModel> ToViewModel(IEnumerable<Person> persons);
    DepartmentViewModel ToViewModel(Department department);
    IEnumerable<DepartmentViewModel> ToViewModel(IEnumerable<Department> departments);
}

public class Mapper : IMapper
{
    public PersonViewModel ToViewModel(Person person)
    {
        return new PersonViewModel
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DepartmentId = person.DepartmentId,
            DepartmentName = person.Department?.Name,
            Email = person.Email
        };
    }

    public Person ToEntity(PersonViewModel viewModel)
    {
        return new Person
        {
            Id = viewModel.Id,
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            DateOfBirth = viewModel.DateOfBirth,
            DepartmentId = viewModel.DepartmentId,
            Email = viewModel.Email
        };
    }

    public IEnumerable<PersonViewModel> ToViewModel(IEnumerable<Person> persons)
    {
        return persons.Select(person => new PersonViewModel
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DepartmentId = person.DepartmentId,
            DepartmentName = person.Department?.Name,
            Email = person.Email
        });
    }

    public DepartmentViewModel ToViewModel(Department department)
    {
        return new DepartmentViewModel
        {
            Id = department.Id,
            Name = department.Name
        };
    }

    public IEnumerable<DepartmentViewModel> ToViewModel(IEnumerable<Department> departments)
    {
        return departments.Select(department => new DepartmentViewModel
        {
            Id = department.Id,
            Name = department.Name
        });
    }
}
