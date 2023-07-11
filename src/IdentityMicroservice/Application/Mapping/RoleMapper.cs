using Application.Dto;
using Application.Exceptions;
using DataAccess.Models;

namespace Application.Mapping;

public static class RoleMapper
{
    public static RoleDto ToDto(this OtsIdentityRole role)
    {
        return new RoleDto(role.Id, role.Name ?? throw new UnknownRoleException());
    }
}