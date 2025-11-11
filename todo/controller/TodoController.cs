using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todo.DTOs;

namespace todo.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly Models.TodoListContext _todoListContext;

        public TodoController(Models.TodoListContext TodoListContext)
        {
            _todoListContext = TodoListContext;
        }

        //查詢User所有todo的項目
        [HttpGet("{Userid}")]
        public ActionResult<List<TodoDto>> GetTodos(string Userid)
        {
            int intUserid;
            if (!int.TryParse(Userid, out intUserid))
            {
                return BadRequest(new { Message = "使用者ID格式錯誤" });
            }

            if (!_todoListContext.User.Any(u => u.Id == intUserid))
            {
                return NotFound(new { Message = $"未搜尋到使用者ID:{intUserid}資料" });
            }
            var todos = _todoListContext.Todo
                .Where(t => t.UserId == intUserid)
                .Select(t => new DTOs.TodoDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Descript = t.Descript,
                    CreateAt = t.CreateAt,
                    UpdateAt = t.UpdateAt,
                    FinishAt = t.FinishAt,
                    DeleteAt = t.DeleteAt,
                    UserId = intUserid
                }).ToList();
            if (todos.Count == 0)
            {
                return NotFound(new { Message = $"未搜尋到ID:{intUserid}資料" });
            }
            return Ok(todos);
        }

    }
}