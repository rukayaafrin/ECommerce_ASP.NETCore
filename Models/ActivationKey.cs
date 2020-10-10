using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Layout.Models
{
    public class ActivationKey
    {
        public string Id { get; set; }
        public string PurchaseId { get; set; }
        public int ProductId { get; set; }
    }

    List<ActivationKey> temp = Db.ActivationKeys.Where(x => x.PurchaseId = purchaseid && x.ProductId = productid)
                            .ToList();
}
