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
    public class TransactionsController : ControllerBase
    {
        private readonly FinancialInstitutionContext _context;
        private readonly JwtAuthenticationManager jwtAuthenticationManager;

        public TransactionsController(JwtAuthenticationManager jwtAuthenticationManager, FinancialInstitutionContext context)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            _context = context;
        }

        // GET: api/Transactions
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transactions>>> GetTransactions()
        {
          if (_context.Transactions == null)
          {
              return NotFound();
          }
            return await _context.Transactions.ToListAsync();
        }

        // GET: api/Transactions/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Transactions>> GetTransaction(string id)
        {
          if (_context.Transactions == null)
          {
              return NotFound();
          }
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(string id, Transactions transaction)
        {
            if (id != transaction.TransactionId)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Transactions>> PostTransaction(Transactions transaction)
        {
          if (_context.Transactions == null)
          {
              return Problem("Entity set 'FinancialInstitutionContext.Transactions'  is null.");
          }
            _context.Transactions.Add(transaction);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TransactionExists(transaction.TransactionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTransaction", new { id = transaction.TransactionId }, transaction);
        }

        // DELETE: api/Transactions/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(string id)
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(string id)
        {
            return (_context.Transactions?.Any(e => e.TransactionId == id)).GetValueOrDefault();
        }

        [AllowAnonymous]
        [HttpPost("Authorize")]
        public IActionResult AuthUser([FromBody] LoginPerson usr)
        {
            var token = jwtAuthenticationManager.Authenicate(usr.username, usr.password);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}
