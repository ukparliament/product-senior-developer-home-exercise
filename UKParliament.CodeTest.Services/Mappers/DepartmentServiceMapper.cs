using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Dtos;

namespace UKParliament.CodeTest.Services.Mappers;

public interface IDepartmentServiceMapper
{
    IEnumerable<DepartmentDto> ToDtos(IEnumerable<Department> departments);
}

public class DepartmentServiceMapper : IDepartmentServiceMapper
{  
    public IEnumerable<DepartmentDto> ToDtos(IEnumerable<Department> departments)
    {
        return departments.Select(dept => new DepartmentDto
        {
            Id = dept.Id,
            Name = dept.Name
        });
    }      
}
