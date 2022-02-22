using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSTest.ViewModels
{
    public class CartDetailsViewModel
    {
        public IEnumerable<ProductCart> Product { get; set; }
        public decimal OrderTotal { get; set; }
    }
}
