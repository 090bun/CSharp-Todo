using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Dtos.UserInfoDtos;
using todo.Models;

namespace todo.Dtos.UserDtos
{
    public class UserRolDto
    {
        public int Id { get; set; }
        public string? Account { get; set; }
        public string? Role { get; set; }
        public string? Name { get; set; }

    }
    public static class UserRoleExtenstions
    {
     public static void ToModel(this User user , UserRolDto userRolDto)
        {
             user.Role = userRolDto.Role;
        }
        
     public static UserRolDto FromModel(User user)
    {
        return new UserRolDto
        {
            Id = user.Id,
            Account = user.Account,
            Role = user.Role,
            Name = user.Name
        };
    }
    
    }
}