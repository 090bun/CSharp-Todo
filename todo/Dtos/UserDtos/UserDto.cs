using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Dtos.UserInfoDtos;
using todo.Models;

namespace todo.Dtos.UserDtos{

public class UserDto
{
    public int Id { get; set; }
    public string? Account { get; set; }
    public string? Role { get; set; }
    public string? Name { get; set; }
    public UserInfoDto? UserInfo { get; set; }
}
    public static class UserExtenstions
    {
        public static UserDto FromModel(User model)
        {
           return new UserDto
           {

               Id = model.Id,
               Account = model.Account,
               Role = model.Role,
               Name = model.Name,
               UserInfo = model.UserInfo == null ? null : UserInfoExtenstions.FromModel(model.UserInfo)
           };
        }
    }
}