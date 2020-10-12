using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Layout.Models
{
    public class ActivationKey
    {
        public int Id { get; set; }
        public string PurchaseDetailPurchaseId { get; set; }
        public int PurchaseDetailProductId { get; set; }
        public string PdtAtvKey { get; set; }
    }
}
