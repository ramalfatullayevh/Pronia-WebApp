using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProniaMVC.DAL;
using ProniaMVC.Models;

namespace ProniaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ClientController : Controller
    {
        AppDbContext _context { get;}

        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Clients.ToList());
        }

        public IActionResult Delete(int id)
        {
            Client client = _context.Clients.Find(id);
            if (client is null)
            {
                return NotFound();
            }
            _context.Clients.Remove(client);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Client client)
        {
            if (!ModelState.IsValid) return View();
            _context.Clients.Add(client);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            Client client = _context.Clients.Find(id);
            if (client is null)
            {
                return NotFound();
            }
            return View(client);

        }

        [HttpPost]
        public IActionResult Update(int? id, Client client)
        {
            if (!ModelState.IsValid) return View();
            if (id is null || id != client.Id) return BadRequest();
            Client existclient = _context.Clients.Find(id);
            if (client is null) return NotFound();
            existclient.UserName = client.UserName;
            existclient.UserImg = client.UserImg;
            existclient.Position = client.Position;
            existclient.Comment = client.Comment;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}


