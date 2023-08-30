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
    public class SalesController : Controller
    {
        private readonly Context _context;
        //public Categories1Controller(Context context)
        // { _context = context; }

        private readonly IRepository<Sale> repository;
        private readonly IRepository<SaleDetail> detailrepository;
        public SalesController(Context context, IRepository<Sale> repository, IRepository<SaleDetail> detailrepository)
        {
            _context = context; 
            this.repository = repository;
            this.detailrepository = detailrepository;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Sales.ToListAsync());
            if (HttpContext.Session.GetString("User") != "admin")
            {
                return RedirectToAction(actionName: "Connect", controllerName: "Users");
            }
            return View(await repository.GetAll());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var sale = await _context.SaleDetails.Include(c => c.Sale).Include(p => p.Product)
            //    .Where(m => m.SaleId == id).ToListAsync();
            var sale = await detailrepository.GetManyByIdIncludes(d => d.SaleId == id, d => d.Sale, d => d.Product);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("SaleId,SalePrice,SaleDate")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(sale);
                //await _context.SaveChangesAsync();
                repository.Insert(sale);
                repository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(sale);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           //var sale = await _context.Sales.FindAsync(id);
            var sale = await repository.GetById(id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("SaleId,SalePrice,SaleDate")] Sale sale)
        {
            if (id != sale.SaleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {   
                return RedirectToAction(nameof(Index));
            }
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var sale = await _context.Sales.FirstOrDefaultAsync(m => m.SaleId == id);
            var sale = await repository.GetById(id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {         
            var sale = await repository.GetById(id);
            repository.Delete(sale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
