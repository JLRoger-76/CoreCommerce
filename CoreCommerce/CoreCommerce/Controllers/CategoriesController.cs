using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreCommerce.Models;
using CoreCommerce.Repositories;
using Microsoft.AspNetCore.Http;

namespace CoreCommerce.Controllers
{
    public class CategoriesController : Controller
    {
        //private readonly Context _context;
        //public Categories1Controller(Context context)
        // { _context = context; }
        
        private readonly IRepository<Category> repository;
        public CategoriesController(IRepository<Category> repository)
        {
            this.repository = repository;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Categories.ToListAsync());
            if (HttpContext.Session.GetString("User") != "admin")
            {
                return RedirectToAction(actionName: "Connect", controllerName: "Users");
            }
            return View(await repository.GetAll());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
            var category = await repository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CategoryId,Name,ParentId")] Category category)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(category);
                //await _context.SaveChangesAsync();
                repository.Insert(category);
                repository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var category = await _context.Categories.FindAsync(id);
            var category = await repository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CategoryId,Name,ParentId")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //_context.Update(category);
                //await _context.SaveChangesAsync();
                repository.Update(category);
                repository.Save();
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
            var category = await repository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var category = await _context.Categories.FindAsync(id);
            //_context.Categories.Remove(category);
            //await _context.SaveChangesAsync();
            var category = await repository.GetById(id);
            repository.Delete(category);
            repository.Save();
            return RedirectToAction(nameof(Index));
        }      
    }
}
