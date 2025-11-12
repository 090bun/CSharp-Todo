using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todo.Models;
using todo.Services.Responses;

namespace todo.Services.Validation
{

    public interface IUserIdValidationService 
    {
        bool TryValidateUserId(string UserIdStr, out int UsetId, out ActionResult? errorResult);
    }
    public class UserValidationService : IUserIdValidationService 
    {
        private readonly TodoListContext _todoListContext;

        public UserValidationService(TodoListContext todoListContext)
        {
            _todoListContext = todoListContext;
        }


         public bool TryValidateUserId(string UserIdStr , out int UsetId , out ActionResult? errorResult)
        {
            if (!int.TryParse(UserIdStr, out UsetId))
            {
                errorResult = new NotFoundObjectResult(
    new ErrorResponse($"查無此使用者的待辦事項")
);
                return false;
            }
            
        //這邊會報錯 不接受out值 所以先做轉換
        var userId = UsetId;
        if (!_todoListContext.User.Any(u => u.Id == userId))
        {
            errorResult = new NotFoundObjectResult(new ErrorResponse($"查無此使用者的待辦事項"));
            return false;
        }

        errorResult = null;
        return true;
        }
    }
    
}