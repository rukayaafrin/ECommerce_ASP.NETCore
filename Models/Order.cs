﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Layout.Models
{
    public class Cart
    {
        public string Id { get; set; }
        public bool PaidFor { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CartDetail> CartDetails { get; set; }
    }

  /*  public class Purchase
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public long Timestamp { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }*/