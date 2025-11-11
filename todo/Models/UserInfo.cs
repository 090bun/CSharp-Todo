using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace todo.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        [Required]
        public string Address { get; set; } = null!;

        [RegularExpression(@"^(19|20)\d{2}\/\d{2}\/(0[1-9]|1[0-2])\/(0[1-9]|[12]\d|3[01])$",ErrorMessage = "格式需要為 YYYY/MM/DD, 例如:1990/06/13")]
        public string Birthday { get; set; }= null!;
        [RegularExpression(@"^09\d{8}",ErrorMessage ="手機格是錯誤")]
        public string Phone { get; set; } = null!;
        

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}