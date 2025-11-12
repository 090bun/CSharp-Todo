using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Models;

namespace todo.Dtos
{
    public class TodoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime? FinishAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        public int UserId { get; set; }
    }

    public static class TodoExtensions
    {
        public static TodoDto FromModel(Todo model)
        {
            return new TodoDto
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                CreateAt = model.CreateAt,
                UpdateAt = model.UpdateAt,
                FinishAt = model.FinishAt,
                DeleteAt = model.DeleteAt,
                UserId = model.UserId
            };
        }
        public static Todo ToModel(TodoDto dto)
        {
            return new Todo
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                CreateAt = dto.CreateAt,
                UpdateAt = dto.UpdateAt,
                FinishAt = dto.FinishAt,
                DeleteAt = dto.DeleteAt,
                UserId = dto.UserId
            };
        }
    }
}