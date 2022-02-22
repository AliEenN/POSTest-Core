using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POSTest.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }

        [Required, StringLength(250)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public byte[] Picture { get; set; }
        public int Size { get; set; }
    }
}
