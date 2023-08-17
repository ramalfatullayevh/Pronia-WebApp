﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVC.DAL;
using ProniaMVC.Models;

namespace ProniaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        AppDbContext _context { get; }

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Categories.ToList());
        }

        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            Category category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);

        }

        [HttpPost]
        public IActionResult Update(int? id, Category category)
        {
            if (!ModelState.IsValid) return View();
            if (id is null || id != category.Id) return BadRequest();
            Category existcategory = _context.Categories.Find(id);
            if (category is null) return NotFound();
            existcategory.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

    }
}
