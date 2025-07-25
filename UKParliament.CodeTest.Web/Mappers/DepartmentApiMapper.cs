using UKParliament.CodeTest.Services.Dtos;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Mappers;

public interface IDepartmentApiMapper
{
    IEnumerable<DepartmentViewModel> ToViewModels(IEnumerable<DepartmentDto> departments);
}
public class DepartmentApiMapper : IDepartmentApiMapper
{
    public IEnumerable<DepartmentViewModel> ToViewModels(IEnumerable<DepartmentDto> departments)
    {
        if (departments == null)
        {
            return [];
        }
        return departments.Select(dept => new DepartmentViewModel
        {
            Id = dept.Id,
            Name = dept.Name
        });
    }
}
