using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProniaMVC.DAL;
using ProniaMVC.Models;
using ProniaMVC.ViewModels;

namespace ProniaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BannerController : Controller
    {
        AppDbContext _context { get; }
        public BannerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Banners.ToList());
        }

        public IActionResult Create()
        {
            if (_context.Banners.Count()<4)
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
            
        }

        [HttpPost]
        public IActionResult Create(CreateBannerVM bannerVM)
        {
            if (!ModelState.IsValid) return View();
            IFormFile file = bannerVM.Image;
            if (!file.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "Yuklediyiniz faylin formati shekil formatinda olmalidir.");
                return View();
            }
            if (file.Length > 3 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Yuklediyiniz shekilin hecmi 3 megabayt - dan cox olmamalidir.");
                return View();
            }
            string fileName = Guid.NewGuid() + file.FileName;
            using (var stream = new FileStream("C:\\Users\\Ramal\\Desktop\\Pronia\\ProniaMVC\\ProniaMVC\\wwwroot\\assets\\images\\" + fileName, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Banner banner = new Banner
            {
                PrimaryTitle = bannerVM.PrimaryTitle,
                SecondTitle = bannerVM.SecondaryTitle,
                ImageUrl = fileName,
            };
            _context.Banners.Add(banner);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int?id)
        {
            if (id is null) return BadRequest();
            Banner banner = _context.Banners.Find(id);
            if (banner is null) return NotFound();
            return View(banner);
        }

        [HttpPost]
        public IActionResult Update(int? id , Banner banner)
        {
            if (!ModelState.IsValid) return View();
            if(id is null || id!=banner.Id) return BadRequest();
            Banner exsistbanner = _context.Banners.Find(id);
            if(banner is null) return NotFound();
            exsistbanner.PrimaryTitle = banner.PrimaryTitle;
            exsistbanner.SecondTitle = banner.SecondTitle;
            exsistbanner.ImageUrl = banner.ImageUrl;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Delete(int id)
        {
            Banner banner = _context.Banners.Find(id);
            if (banner is null ) return NotFound();
            _context.Banners.Remove(banner);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
