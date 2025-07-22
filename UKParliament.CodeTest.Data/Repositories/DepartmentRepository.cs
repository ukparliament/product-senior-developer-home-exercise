using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
}

public class DepartmentRepository(PersonManagerContext context) : IDepartmentRepository
{
    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await context.Departments.ToListAsync();
    }
}
