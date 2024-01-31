using Sales.Domain.Entities;

namespace Sales.Infra.Data.Interfaces;

public interface IUserRepository
{
    Task<User> SignInUser(string email, string password);
}
