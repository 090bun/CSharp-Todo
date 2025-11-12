using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo.Dtos
{
    public class FinishTodoDto
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public DateTime FinishAt { get; set; }
        public DateTime DeleteAt { get; set; }
    }
    
    public static class FinishTodoExtensions
    {
        public static void ToModel(this Models.Todo model, FinishTodoDto FinishTodo)
        {
            model.Id = model.Id;
            model.UserId = model.UserId;
            model.FinishAt = DateTime.Now;
        }
    }
}