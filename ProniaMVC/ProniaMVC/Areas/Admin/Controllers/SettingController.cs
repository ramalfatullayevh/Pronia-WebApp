using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVC.DAL;
using ProniaMVC.Models;

namespace ProniaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Settings.ToList());
        }
        public IActionResult Update(int? id)
        {
            if (id is null) return BadRequest();
            Setting setting = _context.Settings.Find(id);
            if (setting is null) return NotFound();
            return View(setting);
        }

        [HttpPost]
        public IActionResult Update(int? id, Setting setting)
        {
            if (!ModelState.IsValid) return View();
            if (id is null || id != setting.Id) return BadRequest();
            Setting existsetting = _context.Settings.Find(id);
            if (setting is null) return NotFound();
            existsetting.Key = setting.Key;
            existsetting.Value = setting.Value;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
