using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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

        //查看User資料
        [HttpGet("{Userid}")]
        public ActionResult? GetUser(string Userid)
        {
            if (!_userIdValidationService.TryValidateUserId(Userid, out int intUserid, out var errorResult))
            {
                return errorResult;
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

        //修改User資料
        [HttpPatch("{Userid}")]
        public ActionResult? PatchUser(string Userid, [FromBody] UserPatchDto dto)
        {
            if (!_userIdValidationService.TryValidateUserId(Userid, out int intUserid, out var errorResult))
            {
                return errorResult;
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
        [HttpPost]
        public ActionResult? PostUSer([FromBody] UserPostDto dto)
        {
            var user = UserPostExtenstions.ToModel(dto);
            _todoListContext.User.Add(user);
            _todoListContext.SaveChanges();
            var result = UserExtenstions.FromModel(user);
            return CreatedAtAction(nameof(PostUSer),new ApiResponse<UserDto>(result,"新增成功"));
        }

    }
}