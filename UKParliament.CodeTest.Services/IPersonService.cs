using System.ComponentModel.DataAnnotations;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services;

public interface IPersonService
{
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person> GetByIdAsync(int id);
    Task<ValidationResult> AddAsync(Person viewModel);
    Task<ValidationResult> UpdateAsync(Person viewModel);
    Task DeleteAsync(int id);
}