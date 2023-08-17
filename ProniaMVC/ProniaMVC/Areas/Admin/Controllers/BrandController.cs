using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProniaMVC.DAL;
using ProniaMVC.Models;
using ProniaMVC.ViewModels;

namespace ProniaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        AppDbContext _context { get;}

        public BrandController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Brands.ToList());
        }

        public IActionResult Delete(int id)
        {
            Brand brand = _context.Brands.Find(id);
            if (brand is null)
            {
                return NotFound();
            }
            _context.Brands.Remove(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBrandVM brandVM)
        {
            if (!ModelState.IsValid) return View();

            IFormFile file = brandVM.Image;
            if (!file.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "Yuklediyiniz fayl shekil formatinda olmalidir.");
                return View();
            }
            if (file.Length> 3 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Yuklediyiniz shekilin hecmi 3 megabayt -dan artiq olmamalidir");
                return View();
            }
            string fileName = Guid.NewGuid() + file.FileName;
            using (var stream = new FileStream("C:\\Users\\Ramal\\Desktop\\Pronia\\ProniaMVC\\ProniaMVC\\wwwroot\\assets\\images\\" + fileName , FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Brand brand = new Brand
            {
                ImgUrl = fileName
            };
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            Brand brand = _context.Brands.Find(id);
            if (brand is null)
            {
                return NotFound();
            }
            return View(brand);

        }

        [HttpPost]
        public IActionResult Update(int? id, Brand brand)
        {
            if (!ModelState.IsValid) return View();
            if (id is null || id != brand.Id) return BadRequest();
            Brand existbrand = _context.Brands.Find(id);
            if (brand is null) return NotFound();
            existbrand.ImgUrl = brand.ImgUrl;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
