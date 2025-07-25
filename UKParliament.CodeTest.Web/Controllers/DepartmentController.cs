using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Mappers;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController(IDepartmentService departmentService, IDepartmentApiMapper departmentApiMapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentViewModel>>> GetAll()
    {
        var departments = await departmentService.GetAllAsync();
        var result = departmentApiMapper.ToViewModels(departments);
        return Ok(result);
    }
}
