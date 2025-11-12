using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using todo.Models;

namespace todo.Dtos.UserInfoDtos
{
    public class UserInfoDto
    {
        public string? Address { get; set; }

        public string? Birthday { get; set; }
        [RegularExpression(@"^09\d{8}",ErrorMessage ="手機格是錯誤")]
        public string? Phone { get; set; }
        public int UserId { get; set; }
    }
    public static class UserInfoExtenstions
    {
        // Model =>Dto
        public static UserInfoDto FromModel(UserInfo model)
        {
            return new UserInfoDto
            {
                Address = model.Address,
                Birthday = model.Birthday,
                Phone = model.Phone,
                UserId = model.UserId
            };
        }

        //Dto => Model(patch)
        public static void ToModel(this UserInfo data, UserInfoDto dto)
        {
            data.Address = dto.Address ?? data.Address;
            data.Birthday = data.Birthday ?? data.Birthday;
            data.Phone = dto.Phone ?? data.Phone;
        }
        
    }
}