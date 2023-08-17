using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ProniaMVC.DAL;
using ProniaMVC.Models;
using ProniaMVC.Utilies.Extensions;
using ProniaMVC.ViewModels;
using ProniaMVC.ViewModels;

namespace ProniaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {

            return View(_context.Products.Include(c => c.ProductColors).ThenInclude(pc => pc.Color)
                .Include(s => s.ProductSizes).ThenInclude(ps => ps.Size)
                .Include(ctg => ctg.ProductCategories).ThenInclude(pc => pc.Category)
                .Include(img=>img.ProductImages).ToList());
        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            Product existed = _context.Products.Include(p => p.ProductImages).Include(p => p.ProductColors).Include(p => p.ProductSizes).FirstOrDefault(p => p.Id == id);
            if (existed == null) return NotFound();
            foreach (ProductImage image in existed.ProductImages)
            {
                image.ImageUrl.DeleteFile(_env.WebRootPath, "assets/images/product");
            }
            _context.ProductSizes.RemoveRange(existed.ProductSizes);
            _context.ProductColors.RemoveRange(existed.ProductColors);
            _context.ProductImages.RemoveRange(existed.ProductImages);
            _context.Products.Remove(existed);

            existed.IsDeleted = true;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
            ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
            ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductVM cp)
        {
          var coverImg = cp.CoverImage;
          var hoverImg = cp.HoverImage;
          var otherImg = cp.OtherImages;
          if (coverImg?.CheckType("image/") == false)
          {
               ModelState.AddModelError("CoverImage", "Yuklediyiniz fayl shekil deyil");
          }
          if (coverImg?.CheckSize(500) == false)
          {
                ModelState.AddModelError("CoverImage", "Sheklin olcusu 500 kb -dan cox olmamalidir. ");
          }
          if (hoverImg?.CheckType("image/") == false)
          {
              ModelState.AddModelError("HoverImage", "Yuklediyiniz file shekil deyil");
          }
          if (hoverImg?.CheckSize(500) == false)
          {
              ModelState.AddModelError("HoverImage", "Yuklediyiniz shekilin olcusu 500 kb -dan cox olmamalidir ");
          }

         if (!ModelState.IsValid)
         {
                ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
                ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                return View();
         }
         var sizes = _context.Sizes.Where(s => cp.SizeIds.Contains(s.Id));
         var colors = _context.Colors.Where(c => cp.ColorIds.Contains(c.Id));
         var categories = _context.Categories.Where(ctg => cp.CategoryIds.Contains(ctg.Id));

           Product product = new Product
            {
                Name = cp.Name,
                CostPrice = cp.CostPrice,
                SellPrice = cp.SellPrice,
                Description = cp.Description,
                Discount = cp.Discount,
                IsDeleted = false,
                SKU = cp.SKU,
            };

            List<ProductImage> images = new List<ProductImage>();

            images.Add(new ProductImage { ImageUrl = coverImg.SaveFile
                (Path.Combine(_env.WebRootPath, "assets", "images","product")), IsCover = true , Product = product });

            images.Add(new ProductImage { ImageUrl = hoverImg.SaveFile
                (Path.Combine(_env.WebRootPath, "assets", "images","product")), IsCover = false , Product = product });

            foreach (var otherimage in otherImg)
            {
                if (otherimage?.CheckType("image/") == false)
                {
                    ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                    ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
                    ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                    ModelState.AddModelError("OtherImages", "Yuklediyiniz file shekil deyil.");
                }
                if (otherimage?.CheckSize(500) == true)
                {
                    ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                    ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
                    ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                    ModelState.AddModelError("OtherImages", "Yuklediyiniz shekilin olcusu 500 kb - dan cox olmamalidir.");
                }
                images.Add(new ProductImage
                {
                    ImageUrl = otherimage.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images", "product")),
                    IsCover = null,
                    Product = product
                });
                product.ProductImages = images;
                _context.Products.Add(product);
            }
            foreach (var item in colors)
            {
                _context.ProductColors.Add(new ProductColor { Product = product, ColorId = item.Id });
            }
            
            foreach (var item in sizes)
            {
                _context.ProductSizes.Add(new ProductSize { Product = product, SizeId = item.Id });
            }
            
            foreach (var item in categories)
            {
                _context.ProductCategories.Add(new ProductCategory { Product = product, CategoryId = item.Id });
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult UpdateProduct(int? id)
        {
            if (id == null) return BadRequest();
            Product product = _context.Products.Include(pc => pc.ProductColors).Include(pctg => pctg.ProductCategories)
                .Include(ps => ps.ProductSizes).FirstOrDefault(p => p.Id == id);
            if (product is null) return NotFound();
            UpdateProductVM upd = new UpdateProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CostPrice = product.CostPrice,
                SellPrice = product.SellPrice,
                Discount = product.Discount,
                SKU = product.SKU,
                CategoryIds = new List<int>(),
                ColorIds = new List<int>(),
                SizeIds = new List<int>(),

            };
            foreach (var color in product.ProductColors)
            {
                upd.ColorIds.Add(color.ColorId);
            }

            foreach (var category in product.ProductCategories)
            {
                upd.CategoryIds.Add(category.CategoryId);
            }

            foreach (var size in product.ProductSizes)
            {
                upd.SizeIds.Add(size.SizeId);
            }

            ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
            ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
            ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
            return View(upd);
        }

        [HttpPost]
        public IActionResult UpdateProduct(int? id, UpdateProductVM updproduct)
        {
            if (id == null) return BadRequest();
            foreach (int colorId in (updproduct.ColorIds ?? new List<int>()))
            {
                if (!_context.Colors.Any(c => c.Id == colorId))
                {
                    ModelState.AddModelError("ColorIds", "Daxil etdiyiniz deyer yanlishdir.");
                    break;
                }
            }

            foreach (int sizeId in (updproduct.SizeIds ?? new List<int>()))
            {
                if (!_context.Sizes.Any(c => c.Id == sizeId))
                {
                    ModelState.AddModelError("SizeIds", "Daxil etdiyiniz deyer yanlishdir.");
                    break;
                }
            }

            foreach (int categoryId in (updproduct.CategoryIds ?? new List<int>()))
            {
                if (!_context.Categories.Any(c => c.Id == categoryId))
                {
                    ModelState.AddModelError("CategoryIds", "Daxil etdiyiniz deyer yanlishdir.");
                    break;
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
                ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                return View();
            }
            var product = _context.Products.Include(pctg => pctg.ProductCategories).Include(pc => pc.ProductColors)
                .Include(ps => ps.ProductSizes).FirstOrDefault(p => p.Id == id);
            if (product is null) return NotFound();
            foreach (var color in product.ProductColors)
            {
                if (updproduct.ColorIds.Contains(color.ColorId))
                {
                    updproduct.ColorIds.Remove(color.ColorId);
                }
                else
                {
                    _context.ProductColors.Remove(color);
                }
            }

            foreach (var category in product.ProductCategories)
            {
                if (updproduct.CategoryIds.Contains(category.CategoryId))
                {
                    updproduct.CategoryIds.Remove(category.CategoryId);
                }
                else
                {
                    _context.ProductCategories.Remove(category);
                }
            }

            foreach (var size in product.ProductSizes)
            {
                if (updproduct.SizeIds.Contains(size.SizeId))
                {
                    updproduct.SizeIds.Remove(size.SizeId);
                }
                else
                {
                    _context.ProductSizes.Remove(size);
                }
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult UpdateImg(int? id)
        {
            if (id == null) return BadRequest();
            var product = _context.Products.Include(pi => pi.ProductImages).FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            UpdateProductImageVM updimg = new UpdateProductImageVM
            {
                ProductImages = product.ProductImages,
            };
            return View(updimg);
        }
    }
}
