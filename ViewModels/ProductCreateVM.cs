using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSTest.ViewModels
{
    public class ProductCreateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public byte[] Picture { get; set; }

        public List<string> SizeName { get; set; }
        public List<decimal> SizePrice { get; set; }
    }
}
