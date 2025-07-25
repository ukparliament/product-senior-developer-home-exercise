using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Dtos;

namespace UKParliament.CodeTest.Services.Mappers;
public interface IPersonServiceMapper
{
    PersonDto? ToDto(Person person);
    Person? ToEntity(PersonDto viewModel);
    IEnumerable<PersonDto> ToDtos(IEnumerable<Person> persons);
}

public class PersonServiceMapper : IPersonServiceMapper
{
    public PersonDto? ToDto(Person person)
    {
        if (person == null)
        {
            return null;
        }

        return new PersonDto
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


    public Person? ToEntity(PersonDto dto)
    {
        if (dto == null)
        {
            return null;
        }

        return new Person
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            DepartmentId = dto.DepartmentId,
            Email = dto.Email
        };
    }

    public IEnumerable<PersonDto> ToDtos(IEnumerable<Person> persons)
    {
        if (persons is null)
        {
            return [];
        }

        return persons.Select(person => new PersonDto
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
}

