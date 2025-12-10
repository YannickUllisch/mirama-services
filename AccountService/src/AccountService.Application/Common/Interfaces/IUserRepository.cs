
using AccountService.Application.Domain.User;
using AccountService.Application.Domain.User.ValueObjects;

namespace AccountService.Application.Common.Interfaces;

public interface IUserRepository
{
    User GetUser(UserId id);
}