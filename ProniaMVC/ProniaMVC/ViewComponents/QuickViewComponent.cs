using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVC.DAL;
using ProniaMVC.Models;

namespace ProniaMVC.ViewComponents
{
    public class QuickViewComponent:ViewComponent
    {
        readonly AppDbContext _context;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var product = _context.Products.Include(pi => pi.ProductImages).Include(pctg => pctg.ProductCategories)
                .ThenInclude(ctg => ctg.Category).Include(pc => pc.ProductColors).ThenInclude(pc => pc.Color)
                .Include(ps => ps.ProductSizes).ThenInclude(ps => ps.Size).ToList();

            return View(product);
        }
        
    }
}
