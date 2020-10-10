using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Layout.Models
{
    public class ActivationKey
    {
        //id is the actual activation key for each item purchased
        public string Id { get; set; }
        public string PurchaseId { get; set; }
        public int ProductId { get; set; }
    }
}
