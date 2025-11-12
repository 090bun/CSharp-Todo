using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;

        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
        public UserInfo UserInfo { get; set; } = null!;

    }
    
}