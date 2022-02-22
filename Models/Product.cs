using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POSTest.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public byte[] Picture { get; set; }

        public IEnumerable<Size> Sizes { get; set; }
    }
}
