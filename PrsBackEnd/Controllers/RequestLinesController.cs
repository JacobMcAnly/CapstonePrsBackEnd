using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsBackEnd.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PrsBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RequestLinesController : ControllerBase
    {
        private readonly PrsDbContext _context;

        public RequestLinesController(PrsDbContext context)
        {
            _context = context;
        }

        // GET: RequestLines
       [HttpGet] //Attribute
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetRequestLine()
        {
            return await _context.RequestLines
                .Include(r => r.Request).ThenInclude(request => request.User) // do I need to return user?
                .Include(r => r.Product).ThenInclude(product => product.Vendor)
                .ToListAsync();
        }

        // GET: RequestLines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestLine>> GetRequestLine(int id)
        {
            var requestLine = await _context.RequestLines
                .Include(r => r.Request).ThenInclude(request => request.User)
                .Include(r => r.Product).ThenInclude(product => product.Vendor)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (requestLine == null)
            {
                return NotFound();
            }

            return requestLine;
        }

        // PUT: RequestLines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestLine(int id, RequestLine requestLine)
        {
            if (id != requestLine.Id)
            {
                return BadRequest();
            }

            _context.Entry(requestLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                await RecalcRequestTotal(requestLine.RequestId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestLineExists(id))
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

        // POST: RequestLines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestLine>> PostRequestLine(RequestLine requestLine)
        {
            _context.RequestLines.Add(requestLine);
            await _context.SaveChangesAsync();

           await RecalcRequestTotal(requestLine.RequestId);

            return CreatedAtAction("GetRequestLine", new { id = requestLine.Id }, requestLine);
        }

        // DELETE: RequestLines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestLine(int id)
        {
            var requestLine = await _context.RequestLines.FindAsync(id);
            if (requestLine == null)
            {
                return NotFound();
            }

            _context.RequestLines.Remove(requestLine);
            await _context.SaveChangesAsync();

            await RecalcRequestTotal(requestLine.RequestId);

            return NoContent();
        }

        private bool RequestLineExists(int id)
        {
            return _context.RequestLines.Any(e => e.Id == id);
        }

       // Get list of RequestLines by RequestId
        [HttpGet]
        [Route("/lines-for-request/{requestId}")]
        public async Task<List<RequestLine>> GetRequestLinesByRequestId(int requestId)
        {
            List<RequestLine> requestLines = await _context.RequestLines
                .Include(r => r.Request).ThenInclude(request => request.User)
                .Include(r => r.Product).ThenInclude(product => product.Vendor)
                .Where(r => r.RequestId == requestId)
                .ToListAsync();

            return requestLines;
        }

        // Recalculate Request Total - every create, update, or delete on a RequestLine should trigger a recalculateTotal on the associated Request
        private async Task RecalcRequestTotal(int requestId)
        {
            // get the total
            var total = await _context.RequestLines
                .Where(rl => rl.RequestId == requestId)
                .Include(rl => rl.Product)
                .Select(rl => new { linetotal = (rl.Product.Price) * (rl.Quantity) }) //source of all evil
            .SumAsync(s => s.linetotal);
            //Find request
            var theRequest = await _context.Requests.FindAsync(requestId);
            //Update the request
            theRequest.Total = total;
            //Save changes()
            await _context.SaveChangesAsync();
        }
        
    }
}