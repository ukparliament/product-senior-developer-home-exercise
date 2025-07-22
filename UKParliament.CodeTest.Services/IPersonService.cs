using UKParliament.CodeTest.Services.Dtos;

namespace UKParliament.CodeTest.Services;

public interface IPersonService
{
    Task<IEnumerable<PersonDto>> GetAllAsync();
    Task<PersonDto?> GetByIdAsync(int id);
    Task<PersonDto> AddAsync(PersonDto dto);
    Task<bool> UpdateAsync(PersonDto dto);
    Task<bool> DeleteAsync(int id);
}