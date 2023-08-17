using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProniaMVC.DAL;
using ProniaMVC.Models;
using ProniaMVC.ViewModels;
using ProniaMVC.ViewModels;

namespace ProniaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        AppDbContext _context { get; }

        public SliderController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Sliders.ToList());
        }

        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.Find(id);
            if (slider is null)
            {
                return NotFound();
            }
            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateSliderVM sliderVM)
        {
            if (!ModelState.IsValid) return View();
                while (_context.Sliders.Any(s=>s.Order==sliderVM.Order))
              {
                 sliderVM.Order++;
              }
            IFormFile file = sliderVM.Image;
            if (!file.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "Yuklediyiniz fayl shekil formatinda deyil.");
                return View();
            }
            if (file.Length > 3 * 1024 * 1024 )
            {
                ModelState.AddModelError("Image", "Yuklediyiniz shekilin hecmi 3 megabayt - dan chox olmamalidir.");
                return View();
            }
            string fileName = Guid.NewGuid() + file.FileName;
            using (var stream = new FileStream("C:\\Users\\Ramal\\Desktop\\Pronia-Part2\\ProniaMVC-Part2\\ProniaMVC\\wwwroot\\assets\\images\\" + fileName , FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Slider slider = new Slider
            {
                Name = sliderVM.Name,
                DiscountPercent = sliderVM.DiscountPercent,
                ImgUrl = fileName,
                Order = sliderVM.Order,
            };
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            Slider slider = _context.Sliders.Find(id);
            if (slider is null)
            {
                return NotFound();
            }
            return View(slider);

        }

        [HttpPost]
        public IActionResult Update(int? id, Slider slider)
        {
            if (!ModelState.IsValid) return View();
            if (id is null || id != slider.Id) return BadRequest();
            Slider anotherslider = _context.Sliders.FirstOrDefault(s=>s.Order==slider.Order);
            if (anotherslider != null)
            {
                anotherslider.Order = _context.Sliders.Find(id).Order;
            }
            Slider existslider = _context.Sliders.Find(id);
            if (slider is null) return NotFound();
            existslider.Name = slider.Name;
            existslider.DiscountPercent = slider.DiscountPercent;
            existslider.ImgUrl = slider.ImgUrl;
            existslider.Order = slider.Order;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
