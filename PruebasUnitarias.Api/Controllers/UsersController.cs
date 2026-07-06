using Microsoft.AspNetCore.Mvc;
using PruebasUnitarias.Api.Application;
using PruebasUnitarias.Api.Domain;

namespace PruebasUnitarias.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var users = await userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var user = await userService.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] User user)
    {
        var createdUser = await userService.CreateAsync(user);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = createdUser.Id }, createdUser);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var existingUser = await userService.GetByIdAsync(id);
        if (existingUser is null)
        {
            return NotFound();
        }

        await userService.DeleteAsync(id);
        return NoContent();
    }
}
