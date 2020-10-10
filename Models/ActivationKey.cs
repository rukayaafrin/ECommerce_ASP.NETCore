using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Layout.Models
{
    public class ActivationKey
    {
        public int Id { get; set; }
        public string PurchaseId { get; set; }
        public int ProductId { get; set; }
        public string PdtAtvKey { get; set; }
    }
}
