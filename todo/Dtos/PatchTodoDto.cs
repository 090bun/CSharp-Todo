using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo.DTOs
{
    public class PatchTodoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } =null!;
        public string Descript { get; set; }=null!;
        
        public DateTime UpdateAt { get; set; }
        public DateTime ? FinishAt { get; set; }

        public DateTime ? DeleteAt { get; set; }

        public int UserId { get; set; }

    }
}