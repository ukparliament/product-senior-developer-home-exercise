using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department> GetByIdAsync(int id);
}

public class DepartmentRepository(PersonManagerContext context) : IDepartmentRepository
{
    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await context.Departments.ToListAsync();
    }

    public async Task<Department> GetByIdAsync(int id)
    {
        return await context.Departments
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == id);
    }
}
