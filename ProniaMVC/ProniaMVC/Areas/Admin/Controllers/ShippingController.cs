using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProniaMVC.DAL;
using ProniaMVC.Models;

namespace ProniaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class Shipping : Controller
    {
        AppDbContext _context { get; }

        public Shipping(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.ShippingAreas.ToList());
        }

        public IActionResult Delete(int id)
        {
            ShippingArea area = _context.ShippingAreas.Find(id);
            if (area is null)
            {
                return NotFound();
            }
            _context.ShippingAreas.Remove(area);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Create()
        {
            if (GetCount()<3)
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
            
        }

        [HttpPost]
        public IActionResult Create(ShippingArea shippingArea)
        {
            if (!ModelState.IsValid) return View();
            _context.ShippingAreas.Add(shippingArea);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            ShippingArea shippingArea = _context.ShippingAreas.Find(id);
            if (shippingArea is null)
            {
                return NotFound();
            }
            return View(shippingArea);

        }

        [HttpPost]
        public IActionResult Update(int? id, ShippingArea shippingArea)
        {
            if (!ModelState.IsValid) return View();
            if (id is null || id != shippingArea.Id) return BadRequest();
            ShippingArea existshippingArea = _context.ShippingAreas.Find(id);
            if (shippingArea is null) return NotFound();
            existshippingArea.Name = shippingArea.Name;
            existshippingArea.Description = shippingArea.Description;
            existshippingArea.IconUrl = shippingArea.IconUrl;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        public int GetCount()
        {
            return _context.ShippingAreas.Count();
        }
    }
}
