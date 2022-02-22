using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POSTest.Helpers;
using POSTest.Models;
using POSTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSTest.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static List<ProductShoppingCartVM> ProductShoppingCartList = new List<ProductShoppingCartVM>();

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(e => e.Sizes).ToListAsync();
            var result = new List<ItemVM>();

            foreach (var product in products)
            {
                foreach (var item in product.Sizes)
                {
                    result.Add(new ItemVM
                    {
                        Id = product.Id,
                        Name = product.Name + " - " + item.Name,
                        Price = product.Price,
                        Picture = product.Picture
                    });
                }
            }

            return View(result);
        }

        public async Task<IActionResult> AddToShoppingCarts(int? id)
        {
            if (id == null)
                return BadRequest();

            var productExist = await _context.Products.FindAsync(id);

            if (productExist == null)
                return NotFound();

            ProductShoppingCartList.Add(new ProductShoppingCartVM
            {
                Id = productExist.Id,
                Name = productExist.Name,
                Price = productExist.Price,
                Picture = productExist.Picture
            });

            HttpContext.Session.SetComplexData("itemInSession", ProductShoppingCartList);

            return RedirectToAction(nameof(Index));
        }
    }
}
