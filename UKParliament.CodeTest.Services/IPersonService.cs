using UKParliament.CodeTest.Services.Dtos;

namespace UKParliament.CodeTest.Services;

public interface IPersonService
{
    Task<IEnumerable<PersonDto>> GetAllAsync();
    Task<PersonDto> GetByIdAsync(int id);
    Task AddAsync(PersonDto dto);
    Task UpdateAsync(PersonDto dto);
    Task DeleteAsync(int id);
}