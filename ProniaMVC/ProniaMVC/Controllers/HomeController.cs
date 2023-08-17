using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProniaMVC.Abstractions;
using ProniaMVC.DAL;
using ProniaMVC.ViewModels;

namespace ProniaMVC.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context { get; }
        IEmailService _emailService { get; }

        public HomeController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Sliders = _context.Sliders.OrderBy(s=>s.Order),
                ShippingAreas = _context.ShippingAreas.ToList(),
                Products = _context.Products.Include(p=>p.ProductImages).ToList(),
                Colors = _context.Colors.ToList(),
                Clients = _context.Clients.ToList(),
                Categories = _context.Categories.ToList(),
                Brands = _context.Brands.ToList(),
                Banners = _context.Banners.ToList(),
            };

            return View(homeVM);

        }


        public IActionResult Shop()
        {
            HomeVM homeVM = new HomeVM
            {
                Products = _context.Products.ToList(),
                Colors = _context.Colors.ToList(),
                Categories = _context.Categories.ToList(),
                
            };
            return View(homeVM);
        }

        [HttpPost]
        public IActionResult LoadProducts(int skip = 4, int take = 4)
        {
            HomeVM home = new HomeVM
            {
                Products = _context.Products.Where(p => !p.IsDeleted).Include(p => p.ProductImages).Skip(skip).Take(take)
            };
            return PartialView("_ProductPartial", home);
        }

        public IActionResult SetSession(string key, string value)
        {
            HttpContext.Session.SetString(key,value);
            return Content ("OK");
        }

        public IActionResult GetSession(string key)
        {
            string value=HttpContext.Session.GetString(key);
            return Content(value);
        }

        public IActionResult SetCookie(string key, string value)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                MaxAge = TimeSpan.MaxValue
            });
            return Content("OK");

        }

        public IActionResult GetCookie(string key)
        {
            return Content(HttpContext.Request.Cookies[key]);
        }
         
        public IActionResult AddBasket(int? id)
        {
            if(!_context.Products.Any(p=>p.Id == id)) return NotFound();
            List<BasketItemVM> items = new List<BasketItemVM>();
            if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["basket"]))
            {
                items = JsonConvert.DeserializeObject<List<BasketItemVM>>(HttpContext.Request.Cookies["basket"]);
            }

            BasketItemVM item = items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                item = new BasketItemVM
                {
                    Id = (int)id,
                    Count = 1
                };
                items.Add(item);
            }
            else
            {
                item.Count++;
            }
            string basket = JsonConvert.SerializeObject(items);
            HttpContext.Response.Cookies.Append("basket", basket, new CookieOptions
            {
                MaxAge = TimeSpan.FromDays(3)
            });
            return RedirectToAction(nameof(Index));
        }

        public IActionResult SendMail()
        {
            _emailService.SendMail("ramalfetullayev8989@gmail.com", "Hello This Is Pronia", "Click the link and change password.");
            return View();
        }


    }
}
