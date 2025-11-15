using Microsoft.AspNetCore.Mvc;
using todo.Models;
using todo.Dtos.UserDtos; 
using todo.Services;
using todo.Services.Responses;
using todo.Dtos;

namespace todo.Controller
{
    [Route("api")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly TodoListContext _todoListContext;
        private readonly JwtService _jwtService;

        public TokenController(
            TodoListContext todoListContext,
            JwtService jwtService
        )
        {
            _todoListContext = todoListContext;
            _jwtService = jwtService;
        }

        // POST api/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var user = _todoListContext.User
                .FirstOrDefault(x => x.Account == loginDto.Account);
               if (user.Password == null)
               {
                return Unauthorized(new ErrorResponse("帳號或密碼錯誤"));
               }
            var passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);

            if (user == null || !passwordValid )
                return Unauthorized(new ErrorResponse("帳號或密碼錯誤"));

            var token = _jwtService.GenerateToken(user.Id, user.Role);

            return Ok(new 
            {
                Message = "登入成功",
                Token = token
            });
        }
    }
}
