using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Models;

namespace todo.Dtos
{
    public  class PatchTodoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime UpdateAt { get; set; }
        public DateTime? FinishAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        public int UserId { get; set; }
    }

    public static class PatchTodoExtenstions
    {
        //model => Dto
        public static PatchTodoDto FromModel(Todo model)
        {
            return new PatchTodoDto
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                UpdateAt = model.UpdateAt,
                FinishAt = model.FinishAt,
                DeleteAt = model.DeleteAt,
                UserId = model.UserId
            };
        }

        // Dto => Model
        public static void ToModel(this Todo model, PatchTodoDto dto)
        {
        // 修改現有 model
        model.Title = dto.Title ?? model.Title;
        model.Description = dto.Description ?? model.Description;
        model.UpdateAt = DateTime.Now;
        model.FinishAt = model.FinishAt;
        model.DeleteAt = model.DeleteAt;
        }
    }
}