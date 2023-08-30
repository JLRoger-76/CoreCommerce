using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreCommerce.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CoreCommerce.Repositories;
using Microsoft.AspNetCore.Http;

namespace CoreCommerce.Controllers
{
   
    public class ProductsController : Controller
    {
        //private readonly Context _context;
        
        private readonly IRepository<Product> repository;
        private readonly IRepository<Category> categoryrepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        //public ProductsController(Context context)
        // { _context = context; }
        public ProductsController(IRepository<Product> repository, IRepository<Category> categoryrepository, IWebHostEnvironment hostEnvironment)
        {
            this.repository = repository;
            this.categoryrepository = categoryrepository;
            _hostEnvironment = hostEnvironment;
        }
        
        // GET: Products
        public async Task<IActionResult> Index()
        {
            //var context = _context.Products.Include(p => p.Category);
            //return View(await context.ToListAsync());
            if (HttpContext.Session.GetString("User") != "admin")
            {
                return RedirectToAction(actionName: "Connect", controllerName: "Users");
            }
            ViewData["CategoryId"] = await categoryrepository.GetAll();
            return View(await repository.GetAllIncludes(c=>c.Category));
        }        

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var product = await _context.Products
            //    .Include(p => p.Category)
            //    .FirstOrDefaultAsync(m => m.ProductId == id);
            //var product = await repository.GetById(id);
            var product = await repository.GetByIdIncludes(p=>p.ProductId==id,p=>p.Category);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            //ViewData["CategoryId"] = _context.Categories.ToList();
            ViewData["CategoryId"] = await categoryrepository.GetAll();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Price,Detail,Stock,ImageFile,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    //Save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    product.Image = fileName = fileName + extension;
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }
                }
                //_context.Add(product);
                //await _context.SaveChangesAsync();
                repository.Insert(product);
                repository.Save();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["CategoryId"] = categoryrepository.GetAll();
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.Products.FindAsync(id);
            var product = await repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            //ViewData["CategoryId"] = _context.Categories.ToList();
            ViewData["CategoryId"] = await categoryrepository.GetAll();
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Price,Detail,ImageFile,Stock,CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (product.ImageFile!=null)
                {
                    //Save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    product.Image = fileName = fileName + extension;
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }
                }
                
                //_context.Update(product);
                //await _context.SaveChangesAsync();
                repository.Update(product);
                repository.Save();              
                
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["CategoryId"] = categoryrepository.GetAll();
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.Products
            //    .Include(p => p.Category)
            //    .FirstOrDefaultAsync(m => m.ProductId == id);
            var product = await repository.GetByIdIncludes(p => p.ProductId == id, p => p.Category);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var product = await _context.Products.FindAsync(id);
            //_context.Products.Remove(product);
            //await _context.SaveChangesAsync();
            var product = await repository.GetById(id);
            repository.Delete(product);
            repository.Save();
            
            //delete image from wwwroot/image
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", product.Image);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            
            return RedirectToAction(nameof(Index));
        }
      
    }
}
