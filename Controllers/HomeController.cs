using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSTest.Models;
using POSTest.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace POSTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _context.Products.Select(e => new ProductVM
            {
                Id = e.Id,
                Name = e.Name,
                Price = e.Price,
                Picture = e.Picture
                // Size = _context.Sizes.Where(e => e.ProductId == e.Id).ToList().Count
            }).ToListAsync();

            foreach (var product in result)
            {
                product.Size = _context.Sizes.Where(e => e.ProductId == product.Id).ToList().Count;
            }

            return View(result);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM createVM)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var files = Request.Form.Files;

            if (!files.Any())
            {
                ModelState.AddModelError("Picture", "Please select item picture");
                return RedirectToAction(nameof(Index));
            }

            var picture = files.FirstOrDefault();

            if (picture.Length > 1048576)
            {
                ModelState.AddModelError("Picture", "Picture cannot be more than 1 MB");
                return RedirectToAction(nameof(Index));
            }

            using var dataStream = new MemoryStream();
            await picture.CopyToAsync(dataStream);

            await _context.Products.AddAsync(new POSTest.Models.Product
            {
                Name = createVM.Name,
                Price = createVM.Price,
                Picture = dataStream.ToArray()
            });

            _context.SaveChanges();

            var currentProduct = await _context.Products.SingleOrDefaultAsync(e => e.Name == createVM.Name);

            for (int i = 0; i < createVM.SizeName.Count; i++)
            {
                await _context.Sizes.AddAsync(new Size
                {
                    Name = createVM.SizeName[i],
                    Price = createVM.SizePrice[i],
                    ProductId = currentProduct.Id
                });
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var productExists = await _context.Products.FindAsync(id);

            if (productExists == null)
                return NotFound();

            productExists.Sizes = await _context.Sizes.Where(e => e.ProductId == productExists.Id).ToListAsync();

            return View(productExists);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var productExists = await _context.Products.FindAsync(id);

            if (productExists == null)
                return NotFound();

            var sizesRelated = await _context.Sizes.Where(e => e.ProductId == productExists.Id).ToListAsync();

            _context.Products.Remove(productExists);

            _context.Sizes.RemoveRange(sizesRelated);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
