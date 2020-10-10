using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Layout.Models
{
    public class PurchaseDetail
    {
        public string PurchaseId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }

        public virtual Purchase Purchase { get; set; }
        public virtual Product Product { get; set; }
    }
}
