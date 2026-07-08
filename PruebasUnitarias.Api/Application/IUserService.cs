using PruebasUnitarias.Api.Domain;

namespace PruebasUnitarias.Api.Application;

public interface IUserService
{
    Task<IReadOnlyList<User>> GetAllAsync();

    Task<User?> GetByIdAsync(int id);

    Task<User> CreateAsync(User user);

    Task DeleteAsync(int id);
    Task UpdateAsync(User existingUser);
}
