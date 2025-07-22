using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController(IValidator<PersonViewModel> validator) : ControllerBase
{

    [Route("{id:int}")]
    [HttpGet]
    public ActionResult<PersonViewModel> GetById(int id)
    {
        return Ok(new PersonViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonViewModel model)
    {
        var result = await validator.ValidateAsync(model);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }
        return Ok();
    }
}