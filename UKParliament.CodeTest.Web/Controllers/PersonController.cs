using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Mappers;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController(IPersonService personService,
    IValidator<PersonViewModel> validator,
    IPersonApiMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonViewModel>>> GetAll()
    {
        var people = await personService.GetAllAsync();
        var result = mapper.ToViewModels(people);
        return Ok(result);
    }

    [Route("{id:int}")]
    [HttpGet]
    public async Task<ActionResult<PersonViewModel>> GetById(int id)
    {
        var person = await personService.GetByIdAsync(id);
        if (person == null)
        {
            return NotFound();
        }
        var result = mapper.ToViewModel(person);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PersonViewModel>> Add(PersonViewModel model)
    {
        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            return BadRequest(errors);
        }

        var personDto = mapper.ToDto(model);
        var createdPerson = await personService.AddAsync(personDto);
        var response = mapper.ToViewModel(createdPerson);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PersonViewModel>> Update(int id, PersonViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest("ID mismatch");
        }

        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var personDto = mapper.ToDto(model);
        var success = await personService.UpdateAsync(personDto);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await personService.DeleteAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}