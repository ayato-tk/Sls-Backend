using Sales.Domain.Common;
using Sales.Domain.Entities;

namespace Sales.Application.Interfaces;

public interface IAuthService
{
    Task<Result<User>> SignIn(string email, string password);
}
