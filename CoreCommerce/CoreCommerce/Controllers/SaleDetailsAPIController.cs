using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreCommerce.Models;

namespace CoreCommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleDetailsAPIController : ControllerBase
    {
        private readonly Context _context;

        public SaleDetailsAPIController(Context context)
        {
            _context = context;
        }

        // GET: api/SaleDetailsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDetail>>> GetSaleDetails()
        {
            return await _context.SaleDetails.ToListAsync();
        }

        // GET: api/SaleDetailsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleDetail>> GetSaleDetail(int id)
        {
            var saleDetail = await _context.SaleDetails.FindAsync(id);

            if (saleDetail == null)
            {
                return NotFound();
            }

            return saleDetail;
        }

        // PUT: api/SaleDetailsAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaleDetail(int id, SaleDetail saleDetail)
        {
            if (id != saleDetail.SaleDetailId)
            {
                return BadRequest();
            }

            _context.Entry(saleDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SaleDetailsAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SaleDetail>> PostSaleDetail(SaleDetail saleDetail)
        {
            _context.SaleDetails.Add(saleDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSaleDetail", new { id = saleDetail.SaleDetailId }, saleDetail);
        }

        // DELETE: api/SaleDetailsAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SaleDetail>> DeleteSaleDetail(int id)
        {
            var saleDetail = await _context.SaleDetails.FindAsync(id);
            if (saleDetail == null)
            {
                return NotFound();
            }

            _context.SaleDetails.Remove(saleDetail);
            await _context.SaveChangesAsync();

            return saleDetail;
        }

        private bool SaleDetailExists(int id)
        {
            return _context.SaleDetails.Any(e => e.SaleDetailId == id);
        }
    }
}
