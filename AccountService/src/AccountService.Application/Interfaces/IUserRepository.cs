using AccountService.Domain.User;
using AccountService.Domain.User.ValueObjects;

namespace AccountService.Application.Interfaces;


public interface IUserRepository
{
    User GetUser(UserId id);
}