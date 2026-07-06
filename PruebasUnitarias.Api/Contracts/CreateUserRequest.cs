using System.ComponentModel.DataAnnotations;

namespace PruebasUnitarias.Api.Contracts;

public sealed class CreateUserRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}