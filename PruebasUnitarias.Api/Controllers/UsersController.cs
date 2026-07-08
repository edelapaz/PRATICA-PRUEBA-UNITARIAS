using Microsoft.AspNetCore.Mvc;
using PruebasUnitarias.Api.Application;
using PruebasUnitarias.Api.Contracts;
using PruebasUnitarias.Api.Domain;

namespace PruebasUnitarias.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController(IUserService userService) : ControllerBase
{

    // TODO: Implement the controller methods for user management (CRUD operations) using the IUserService.
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var users = await userService.GetAllAsync();
        return Ok(users);
    }

    // TODO: Implement the GetByIdAsync method to retrieve a user by their ID.

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
    // TODO: Implement the CreateAsync method to create a new user.
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email
        };

        var createdUser = await userService.CreateAsync(user);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = createdUser.Id }, createdUser);
    }
    // TODO: Implement the UpdateAsync method to update an existing user.
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
