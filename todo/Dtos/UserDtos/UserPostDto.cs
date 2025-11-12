using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Dtos;
using todo.Dtos.UserInfoDtos;
using todo.Models;

namespace todo.Dtos.UserDtos
{
    public class UserPostDto
    {
        public string Name { get; set; } = null!;
         public string Password { get; set; } = null!;
        public UserInfoDto ? UserInfo { get; set; } 

    }

    public static class UserPostExtenstions
    {
        public static User ToModel(UserPostDto dto)
        {
            return new User
            {
                Name = dto.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                UserInfo = dto.UserInfo == null ? null : new UserInfo
                {
                    Address = dto.UserInfo?.Address ?? string.Empty,
                    Birthday = dto.UserInfo?.Birthday ?? string.Empty,
                    Phone = dto.UserInfo?.Phone ?? string.Empty
                }
            };
        }
    }
}