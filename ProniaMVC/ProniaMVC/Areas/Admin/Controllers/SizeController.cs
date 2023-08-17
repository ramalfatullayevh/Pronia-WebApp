using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProniaMVC.DAL;
using ProniaMVC.Models;

namespace ProniaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SizeController : Controller
    {
        AppDbContext _context { get; }

        public SizeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Sizes.ToList());
        }

        public IActionResult Delete(int id)
        {
            Size size = _context.Sizes.Find(id);
            if (size is null)
            {
                return NotFound();
            }
            _context.Sizes.Remove(size);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Size size)
        {
            if (!ModelState.IsValid) return View();
            _context.Sizes.Add(size);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            Size size = _context.Sizes.Find(id);
            if (size is null)
            {
                return NotFound();
            }
            return View(size);

        }

        [HttpPost]
        public IActionResult Update(int? id, Size size)
        {
            if (!ModelState.IsValid) return View();
            if (id is null || id != size.Id) return BadRequest();
            Size existsize = _context.Sizes.Find(id);
            if (size is null) return NotFound();
            existsize.Name = size.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }


    }
}

