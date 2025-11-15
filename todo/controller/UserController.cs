using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sprache;
using todo.Dtos.UserDtos;
using todo.Dtos.UserInfoDtos;
using todo.Models;
using todo.Services.Responses;
using todo.Services.Validation;

namespace todo.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Models.TodoListContext _todoListContext;
        private readonly IUserIdValidationService _userIdValidationService;

        public UserController(Models.TodoListContext todoListContext, IUserIdValidationService userIdValidationService)
        {
            _todoListContext = todoListContext;
            _userIdValidationService = userIdValidationService;
        }

        //查看User自己的資料
        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public ActionResult? GetUser()
        {
            var UserId = User.FindFirst("UserId")?.Value;
            if (UserId == null)
            {
                return Unauthorized("Token無包含UserId");
            }
            if(!_userIdValidationService.TryValidateUserId(UserId , out int intUserid, out var errorResult))
            {
                return Unauthorized(errorResult);
            }


            var user = _todoListContext.User
                .Include(u => u.UserInfo)
                .AsNoTracking()
                .Where(u => u.Id == intUserid);

            if (!user.Any())
            {
                return NotFound(new ErrorResponse("查無資料"));
            }

            var userdata = user
            .Select(u => UserExtenstions.FromModel(u))
            .ToList();
            return Ok(new ApiResponse<List<UserDto>>(userdata, "查詢成功"));
        }
        [Authorize(Roles = "admin,user")]
        //修改User資料
        [HttpPatch]
        public ActionResult? PatchUser( [FromBody] UserPatchDto dto)
        {
            var UserId = User.FindFirst("UserId")?.Value;
            if (UserId == null)
            {
                return Unauthorized("Token無包含UserId");
            }
            if(!_userIdValidationService.TryValidateUserId(UserId , out int intUserid, out var errorResult))
            {
                return Unauthorized(errorResult);
            }

            var user = _todoListContext.User
            .Include(u => u.UserInfo).FirstOrDefault(u => u.Id == intUserid);

            if (user == null)
            {
                return NotFound(new ErrorResponse("查無資料"));
            }

            user.ToModel(dto);
            _todoListContext.SaveChanges();

            var result = UserPatchExtenstions.FromModel(user);
            return Ok(new ApiResponse<UserPatchDto>(result, "成功修改"));
        }

        //新增User
        [AllowAnonymous]
        [HttpPost]
        public IActionResult? PostUSer([FromBody] UserPostDto dto)
        {
            var user = UserPostExtenstions.ToModel(dto);

            _todoListContext.User.Add(user);
            _todoListContext.SaveChanges();
            var result = UserExtenstions.FromModel(user);
            return CreatedAtAction(nameof(PostUSer),new ApiResponse<UserDto>(result,"新增成功"));
        }


        //修改使用者權限
        [Authorize(Roles = "admin")]
        [HttpPatch("role")]
        public IActionResult ChangRole([FromBody] List<UserRolDto> dtos)
        {
            var UserId = User.FindFirst("UserId")?.Value;
            if (UserId == null)
            {
                return Unauthorized("Token無包含UserId");
            }
            if(!_userIdValidationService.TryValidateUserId(UserId , out int intUserid, out var errorResult))
            {
                return Unauthorized(errorResult);
            }

            //取要變更權限的Id        
            var changUsers=dtos.Select(x => x.Id).ToList();

            //資料庫撈資料
            var users = _todoListContext.User.Where(x => changUsers.Contains(x.Id)).ToList();
            

            if (users==null)
            {
                return NotFound(new ErrorResponse("輸入錯誤"));
            }
            foreach(var Item in users)
            {
                var dto = dtos.First(x=>x.Id == Item.Id);
                dto.Role = Item.Role;
            }
            _todoListContext.SaveChanges();

            var result = users.Select(u=>UserRoleExtenstions.FromModel(u)).ToList();
            return Ok(new ApiResponse<List<UserRolDto>>(result,"修改成功"));
        }
    }
}