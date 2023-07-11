using Application.Dto;
using Application.Exceptions;
using DataAccess.Models;

namespace Application.Mapping;

public static class UserMapper
{
    public static UserDto ToDto(this OtsIdentityUser user)
    {
        return new UserDto(user.Id, user.UserName ?? throw new UnknownUsernameException());
    }
}