using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Layout.Models
{
    public class Session
    {
        [Required]
        [MaxLength(36)]
        public string Id { get; set; }

        [Required]
        public long Timestamp { get; set; }

        [Required]
        [MaxLength(36)]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
