using PruebasUnitarias.Api.Application;
using PruebasUnitarias.Api.Domain;

namespace PruebasUnitarias.Api.Infrastructure;

public sealed class InMemoryUserService : IUserService
{
    private readonly List<User> _users =
    [
        new User { Id = 1, Name = "Eliazar de la paz", Email = "eliazar.delapaz@micm.gob.do" },
        new User { Id = 2, Name = "Jabin Beriguete Medina", Email = "jabin.beriguete@micm.gob.do" },
        new User { Id = 3, Name = "Juan Miguel Jiménez Torres", Email = "miguel.jimenez@micm.gob.do" },
        new User { Id = 4, Name = "José Eduardo Díaz", Email = "jose.diaz@micm.gob.do" },
        new User { Id = 5, Name = "María Fernanda López", Email = "maria.fernandez@micm.gob.do" }
    ];
    public Task<IReadOnlyList<User>> GetAllAsync()
    {
        return Task.FromResult((IReadOnlyList<User>)_users.ToList());
    }

    public Task<User?> GetByIdAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<User> CreateAsync(User user)
    {
        var nextId = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;
        user.Id = nextId;
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task DeleteAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user is not null)
        {
            _users.Remove(user);
        }

        return Task.CompletedTask;
    }
}
