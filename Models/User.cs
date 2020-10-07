using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Layout.Models
{
    public class User
    {
        [Required]
        [MaxLength(36)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        public bool IsBlacklisted { get; set; }
    }

}
