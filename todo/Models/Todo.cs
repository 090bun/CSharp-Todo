using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo.Models
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string  Description { get; set; } = null!;

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }
        public DateTime ? FinishAt { get; set; }

        public DateTime ? DeleteAt { get; set; }
        public int UserId { get; set; }

        public User ? User { get; set; } 


    }
}