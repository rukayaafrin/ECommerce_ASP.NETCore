﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Layout.Models
{
    public class CartDetail
    {
        public string CartId { get; set; }
        public string ProductId { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }

    }
}
