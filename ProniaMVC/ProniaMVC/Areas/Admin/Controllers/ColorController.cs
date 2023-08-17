using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProniaMVC.DAL;
using ProniaMVC.Models;

namespace ProniaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ColorController : Controller
    {
        AppDbContext _context { get; }

        public ColorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Colors.ToList());
        }

        public IActionResult Delete(int id)
        {
            Color color = _context.Colors.Find(id);
            if (color is null)
            {
                return NotFound();
            }
            _context.Colors.Remove(color);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Create(Color color)
        {
            if (!ModelState.IsValid) return View();
            _context.Colors.Add(color);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            Color color = _context.Colors.Find(id);
            if (color is null)
            {
                return NotFound();
            }
            return View(color);

        }

        [HttpPost]
        public IActionResult Update(int? id, Color color)
        {
            if (!ModelState.IsValid) return View();
            if (id is null || id != color.Id) return BadRequest();
            Color existcolor = _context.Colors.Find(id);
            if (color is null) return NotFound();
            existcolor.Name = color.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }


    }
}
