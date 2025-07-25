using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Services.Dtos;
using UKParliament.CodeTest.Services.Mappers;

namespace UKParliament.CodeTest.Services;

public interface IPersonService
{
    Task<IEnumerable<PersonDto>> GetAllAsync();
    Task<PersonDto?> GetByIdAsync(int id);
    Task<PersonDto> AddAsync(PersonDto dto);
    Task<bool> UpdateAsync(PersonDto dto);
    Task<bool> DeleteAsync(int id);
}

public class PersonService(IPersonRepository repository, IPersonServiceMapper mapper) : IPersonService
{
    public async Task<IEnumerable<PersonDto>> GetAllAsync()
    {
        var persons = await repository.GetAllAsync();
        return mapper.ToDtos(persons);
    }

    public async Task<PersonDto?> GetByIdAsync(int id)
    {
        var person = await repository.GetByIdAsync(id);

        return mapper.ToDto(person);
    }

    public async Task<bool> UpdateAsync(PersonDto dto)
    {
        var person = await repository.GetByIdAsync(dto.Id);
        if (person == null)
        {
            return false;
        }

        person.FirstName = dto.FirstName;
        person.LastName = dto.LastName;
        person.DateOfBirth = dto.DateOfBirth;
        person.DepartmentId = dto.DepartmentId;
        person.Email = dto.Email;
        person.Department = null;

        await repository.UpdateAsync(person);
        return true;
    }

    public async Task<PersonDto> AddAsync(PersonDto dto)
    {
        var person = mapper.ToEntity(dto);
        person.Id = repository.GetNextId();
        await repository.AddAsync(person);

        dto.Id = person.Id;
        return dto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var person = await repository.GetByIdAsync(id);
        if (person == null)
        {
            return false;
        }

        await repository.DeleteAsync(id);
        return true;
    }
}