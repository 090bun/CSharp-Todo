using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Dtos.UserInfoDtos;
using todo.Models;

namespace todo.Dtos.UserDtos{

public class UserPatchDto
    {
    public string? Account { get; set; }
    public string? Role { get; set; }
    public string? Name { get; set; }
    public UserInfoDto? UserInfo  { get; set; }
}
    public static class UserPatchExtenstions
    {
        public static UserPatchDto FromModel(User model)
        {
            return new UserPatchDto
            {
                Account = model.Account,
                Role = model.Role,
                Name = model.Name,
                UserInfo = model.UserInfo == null ? null : UserInfoExtenstions.FromModel(model.UserInfo)
            };
        }
        public static void ToModel(this User model , UserPatchDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Name))
                model.Name = dto.Name;

            if (dto.UserInfo != null)
            {
                model.UserInfo ??= new UserInfo();
                model.UserInfo.ToModel(dto.UserInfo);
            }
        }
    }
}