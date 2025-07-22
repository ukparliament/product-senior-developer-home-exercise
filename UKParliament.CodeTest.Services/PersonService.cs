using System.ComponentModel.DataAnnotations;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services;

public class PersonService : IPersonService
{
    public Task<ValidationResult> AddAsync(Person viewModel)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Person>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Person> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ValidationResult> UpdateAsync(Person viewModel)
    {
        throw new NotImplementedException();
    }
}