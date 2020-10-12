using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Layout.Models
{
    public class PurGroupedByPdt
    {
        public int ProductId { get; set; }

        public List<PurchaseDetail> PurchasedItems { get; set; }

        public int TotalQuantity { get; set; }
    }
}
