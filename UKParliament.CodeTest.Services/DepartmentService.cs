using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Services.Dtos;
using UKParliament.CodeTest.Services.Mappers;

namespace UKParliament.CodeTest.Services;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
}

public class DepartmentService(IDepartmentRepository departmentRepository, IDepartmentServiceMapper departmentServiceMapper) : IDepartmentService
{
    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await departmentRepository.GetAllAsync();
        return departmentServiceMapper.ToDtos(departments);
    }
}
