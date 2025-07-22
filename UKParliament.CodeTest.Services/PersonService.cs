using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Services.Dtos;
using UKParliament.CodeTest.Services.Mappers;

namespace UKParliament.CodeTest.Services;

public class PersonService(IPersonRepository repository, IPersonServiceMapper mapper) : IPersonService
{
    public async Task<IEnumerable<PersonDto>> GetAllAsync()
    {
        var persons = await repository.GetAllAsync();
        return mapper.ToDtos(persons);
    }

    public async Task<PersonDto> GetByIdAsync(int id)
    {
        var person = await repository.GetByIdAsync(id);
        return mapper.ToDto(person);
    }

    public async Task UpdateAsync(PersonDto dto)
    {
        var person = await repository.GetByIdAsync(dto.Id);
        person.FirstName = dto.FirstName;
        person.LastName = dto.LastName;
        person.DateOfBirth = dto.DateOfBirth;
        person.DepartmentId = dto.DepartmentId;
        person.Email = dto.Email;

        await repository.UpdateAsync(person);
    }

    public async Task AddAsync(PersonDto dto)
    {
        var person = mapper.ToEntity(dto);
        await repository.AddAsync(person);
    }

    public async Task DeleteAsync(int id)
    {
        await repository.DeleteAsync(id);
    }
}