using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data.Repositories;
public interface IPersonRepository
{
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person> GetByIdAsync(int id);
    Task AddAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(int id);
    int GetNextId();
}

public class PersonRepository(PersonManagerContext context) : IPersonRepository
{
    private readonly PersonManagerContext _context = context;

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await _context.People.Include(p => p.Department).ToListAsync();
    }

    public async Task<Person> GetByIdAsync(int id)
    {
        return await _context.People
                .Include(p => p.Department)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == id);
    }
    public async Task AddAsync(Person person)
    {
        _context.People.Add(person);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Person person)
    {
        _context.People.Update(person);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var person = await GetByIdAsync(id);
        if (person != null)
        {
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
        }
    }

    public int GetNextId()
    {
        return _context.People.Any() ? _context.People.Max(p => p.Id) + 1 : 1;
    }
}

