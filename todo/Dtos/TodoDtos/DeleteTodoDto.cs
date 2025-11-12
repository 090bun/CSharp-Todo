using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Models;

namespace todo.Dtos
{
    public class DeleteTodoDto
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public DateTime DeleteAt { get; set; }
    }
    public static class DeleteTodoExtensions
    {
        public static void ToModel(this Todo model, DeleteTodoDto DeleteTodo)
        {
            model.Id = model.Id;
            model.UserId = model.UserId;
            model.DeleteAt = DateTime.Now;
        }
    }
}