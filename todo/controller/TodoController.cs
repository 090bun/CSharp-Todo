using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todo.Dtos;
using todo.Services.Validation;
using todo.Services.Responses;
using Microsoft.EntityFrameworkCore;

namespace todo.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly Models.TodoListContext _todoListContext;
        private readonly IUserIdValidationService _userIdValidationService;
        public TodoController(Models.TodoListContext TodoListContext , IUserIdValidationService userIdValidationService)
        {
            _todoListContext = TodoListContext;
            _userIdValidationService = userIdValidationService;
        }

        //查詢User所有todo的項目
        [HttpGet("{Userid}")]
        public ActionResult? GetTodos(string Userid)
        {
            if (!_userIdValidationService.TryValidateUserId(Userid, out int intUserid, out var errorResult))
            {
                return errorResult;
            }

            var query = _todoListContext.Todo
            .AsNoTracking()
            .Where(t => t.UserId == intUserid);
            
            if (!query.Any())
            {
                return NotFound(new ErrorResponse($"查無此使用者的待辦事項"));
            }

            var todos = query
                .Select(t => TodoExtensions.FromModel(t))
                .ToList();
    
            return Ok(new ApiResponse<List<TodoDto>>(todos,"查詢成功"));
        }


        //User新增todo的項目
        [HttpPost("{Userid}")]
        public ActionResult? CreateTodo([FromRoute] string Userid, [FromBody] TodoDto todoDto)
        {
             if (!_userIdValidationService.TryValidateUserId(Userid, out int intUserid, out var errorResult))
            {
                return errorResult;
            }

            var todo = TodoExtensions.ToModel(todoDto);
            todo.UserId = intUserid;
            
            _todoListContext.Todo.Add(todo);
            _todoListContext.SaveChanges(); 

            var resultTodo = TodoExtensions.FromModel(todo);
            return CreatedAtAction(nameof(CreateTodo), new ApiResponse<TodoDto>(resultTodo, "新增成功"));
        }

        //User修改特定todo內容
        [HttpPatch("{Userid}")]
        public ActionResult? PatchTodo([FromRoute] string Userid, [FromBody] PatchTodoDto PatchTodoDto)
        {
            if (!_userIdValidationService.TryValidateUserId(Userid, out int intUserid, out var errorResult))
            {
                return errorResult;
            }

            var todo = _todoListContext.Todo.FirstOrDefault(t=>t.Id == PatchTodoDto.Id && t.UserId == intUserid && t.DeleteAt == null);
            if (todo == null)
            {
                return NotFound(new ErrorResponse(){Message = $"待辦事項ID:{PatchTodoDto.Id}不存在"});
            }
            todo.ToModel(PatchTodoDto);
            
            _todoListContext.SaveChanges();

            var result = PatchTodoExtenstions.FromModel(todo);
            return Ok(new ApiResponse<PatchTodoDto>(result, "修改成功"));
        }


        //User刪除特定todo
        [HttpDelete("{Userid}")]
        public ActionResult? DeleteTodo([FromRoute] string Userid, [FromBody] DeleteTodoDto DeleteTodoDto)
        {
             if (!_userIdValidationService.TryValidateUserId(Userid, out int intUserid, out var errorResult))
            {
                return errorResult;
            }

            var todo = _todoListContext.Todo.FirstOrDefault(t=>t.Id==DeleteTodoDto.Id && t.UserId==intUserid && t.DeleteAt==null);
            if (todo == null)
            {
                return NotFound(new ErrorResponse(){ Message = $"待辦事項ID:{DeleteTodoDto.Id}不存在" });
            }

            todo.ToModel(DeleteTodoDto);
            _todoListContext.SaveChanges();
            return Ok(new ApiResponse<DeleteTodoDto>(DeleteTodoDto, "刪除成功"));
        }
        
    
        //User完成特定todo
        [HttpPatch("finish/{Userid}")]
        public ActionResult? FinishTodo([FromRoute]string Userid,[FromBody] FinishTodoDto FinishTodoDto)
        {
            if (!_userIdValidationService.TryValidateUserId(Userid, out int intUserid, out var errorResult))
            {
                return errorResult;
            }
            
            var todo = _todoListContext.Todo
                .FirstOrDefault(t => t.Id == FinishTodoDto.Id && t.UserId == intUserid && t.FinishAt == null && t.DeleteAt == null);
            if (todo==null)
            {
                return NotFound(new ErrorResponse($"待辦事項ID:{FinishTodoDto.Id}不存在"));
            }


            todo.ToModel(FinishTodoDto);
            _todoListContext.SaveChanges();
            return Ok(new { Message = $"待辦事項ID:{FinishTodoDto.Id}已完成" });
        }
    }
}