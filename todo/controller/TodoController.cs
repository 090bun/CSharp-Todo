

using Microsoft.AspNetCore.Mvc;
using todo.Dtos;
using todo.Services.Validation;
using todo.Services.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
namespace todo.controller
{

    
    [Authorize(Roles = "admin,user")]
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
        [HttpGet]
        public ActionResult? GetTodos()
        {
            Console.WriteLine("=== DEBUG START ===");
            Console.WriteLine("Authenticated? " + User.Identity.IsAuthenticated);

            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"CLAIM => {claim.Type} : {claim.Value}");
            }

            Console.WriteLine("=== DEBUG END ===");

            var UserId = User.FindFirst("UserId")?.Value;
            if (UserId == null)
            {
                return Unauthorized("Token無包含UserId");
            }
            if(!_userIdValidationService.TryValidateUserId(UserId , out int intUserid, out var errorResult))
            {
                return Unauthorized(errorResult);
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
        [HttpPost]
        public ActionResult? CreateTodo([FromBody] TodoDto todoDto)
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

            var todo = TodoExtensions.ToModel(todoDto);
            todo.UserId = intUserid;
            
            _todoListContext.Todo.Add(todo);
            _todoListContext.SaveChanges(); 

            var resultTodo = TodoExtensions.FromModel(todo);
            return CreatedAtAction(nameof(CreateTodo), new ApiResponse<TodoDto>(resultTodo, "新增成功"));
        }

        //User修改todo內容
        [HttpPatch]
        public ActionResult? PatchTodo([FromBody] List<PatchTodoDto> PatchTodoDto)
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

            var ids = PatchTodoDto.Select(d => d.Id).ToList();
            var todos = _todoListContext.Todo
            .Where(t => ids.Contains(t.Id)&&t.UserId == intUserid && t.DeleteAt == null)
                .ToList();
                //這邊要先轉List 下面再用foreach去跑  

            if (todos == null|| todos.Count == 0)
            {
                return NotFound(new ErrorResponse(){Message ="沒有提供任何可修改的資料"});
            }

            foreach(var item in todos)
            {
                var dto = PatchTodoDto.First(d=>d.Id == item.Id);
                item.Title = dto.Title;
                item.Description = dto.Description;
                item.UpdateAt = DateTime.Now;
            }
            _todoListContext.SaveChanges();

            var result = todos.Select(t =>PatchTodoExtenstions.FromModel(t))
            .ToList();
            return Ok(new ApiResponse<List<PatchTodoDto>>(result, "修改成功"));
        }


        //User刪除特定todo
        
        [HttpDelete]
        public ActionResult? DeleteTodo([FromBody] List<DeleteTodoDto> DeleteTodoDto)
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

            var ids = DeleteTodoDto.Select(d=>d.Id).ToList();
            var todos = _todoListContext.Todo
            .Where(t=>ids.Contains(t.Id) && t.UserId==intUserid && t.DeleteAt==null).ToList();
            if (todos == null)
            {
                return NotFound(new ErrorResponse(){Message ="沒有提供任何可修改的資料"});
            }

            foreach(var item in todos)
            {
                var dto = DeleteTodoDto.First(d=>d.Id == item.Id);
                item.DeleteAt = DateTime.Now;
            }
            _todoListContext.SaveChanges();
            return Ok(new ApiResponse<List<DeleteTodoDto>>(DeleteTodoDto, "刪除成功"));
        }
        
    
        //User完成特定todo
        [HttpPatch("finish")]
        public ActionResult? FinishTodo([FromBody] List<FinishTodoDto> FinishTodoDto)
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

            var ids = FinishTodoDto.Select(d=>d.Id).ToList();
            var todos = _todoListContext.Todo
                .Where(t => ids.Contains(t.Id) && t.UserId == intUserid && t.FinishAt == null && t.DeleteAt == null).ToList();
            if (todos==null)
            {
                return NotFound(new ErrorResponse("待辦事項不存在"));
            }

            foreach(var item in todos)
            {
                var dto = FinishTodoDto.First(t=>t.Id == item.Id);
                item.FinishAt = DateTime.Now;
            }
            _todoListContext.SaveChanges();
// new ApiResponse<List<FinishTodoDto>>(FinishTodoDto,"待辦事項已完成")
            return Ok();
        }
    }
}