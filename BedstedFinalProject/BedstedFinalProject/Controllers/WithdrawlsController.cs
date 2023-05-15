using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BedstedFinalProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace BedstedFinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WithdrawlsController : ControllerBase
    {
        private readonly FinancialInstitutionContext _context;

        public WithdrawlsController(FinancialInstitutionContext context)
        {
            _context = context;
        }

        // GET: api/Withdrawls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Withdrawl>>> GetWithdrawls()
        {
          if (_context.Withdrawls == null)
          {
              return NotFound();
          }
            return await _context.Withdrawls.ToListAsync();
        }

        // GET: api/Withdrawls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Withdrawl>> GetWithdrawl(string id)
        {
          if (_context.Withdrawls == null)
          {
              return NotFound();
          }
            var withdrawl = await _context.Withdrawls.FindAsync(id);

            if (withdrawl == null)
            {
                return NotFound();
            }

            return withdrawl;
        }

        // PUT: api/Withdrawls/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWithdrawl(string id, Withdrawl withdrawl)
        {
            if (id != withdrawl.WithdrawlId)
            {
                return BadRequest();
            }

            _context.Entry(withdrawl).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WithdrawlExists(id))
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

        // POST: api/Withdrawls
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Withdrawl>> PostWithdrawl(Withdrawl withdrawl)
        {
          if (_context.Withdrawls == null)
          {
              return Problem("Entity set 'FinancialInstitutionContext.Withdrawls'  is null.");
          }
            _context.Withdrawls.Add(withdrawl);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WithdrawlExists(withdrawl.WithdrawlId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWithdrawl", new { id = withdrawl.WithdrawlId }, withdrawl);
        }

        // DELETE: api/Withdrawls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWithdrawl(string id)
        {
            if (_context.Withdrawls == null)
            {
                return NotFound();
            }
            var withdrawl = await _context.Withdrawls.FindAsync(id);
            if (withdrawl == null)
            {
                return NotFound();
            }

            _context.Withdrawls.Remove(withdrawl);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WithdrawlExists(string id)
        {
            return (_context.Withdrawls?.Any(e => e.WithdrawlId == id)).GetValueOrDefault();
        }
    }
}
