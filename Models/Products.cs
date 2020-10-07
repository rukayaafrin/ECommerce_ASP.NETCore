using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Layout.DBContext;

namespace Layout.Models
{
    public class Products
    {

        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        [MaxLength(500)]
        public string Image { get; set; }

    }
}
