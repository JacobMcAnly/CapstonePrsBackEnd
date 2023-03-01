using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using PrsBackEnd.Models;

namespace PrsBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private const String NEW = "New";
        private const String REVIEW = "Review";
        private const String APPROVED = "Approved";
        private const String REJECTED = "Rejected";
        private const String REOPENED = "Reopened";

        private readonly PrsDbContext _context;

        public RequestsController(PrsDbContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.Include(r => r.User).ToListAsync();
        }

        // GET: Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            //var request = await _context.Requests.FindAsync(id);

             var request = await _context.Requests.Where(r => r.Id == id)
                                .Include(r => r.User)
                                .FirstOrDefaultAsync();


            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        // POST: Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }

        //Approve Request
        [HttpPut]
        [Route("/approve")]
        public async Task<IActionResult> Approve([FromBody] Request approvedRequest)
        {
            var request = await _context.Requests.FindAsync(approvedRequest.Id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = APPROVED;

            await _context.SaveChangesAsync();

            return Ok(request);
        }

        [HttpPut]
        [Route("/reject")]
        public async Task<IActionResult> Reject([FromBody] Request rejectedRequest )
        {
            var request = await _context.Requests.FindAsync(rejectedRequest.Id);
            if (request == null)
            
                {
                    return NotFound();
                }

                request.Status = REJECTED;

                await _context.SaveChangesAsync();

                return Ok(request);
            
        }

        //Re-open Request
        [HttpPut]
        [Route("/re-open")]
        public async Task<IActionResult> Reopen([FromBody] Request reopenRequest)
        {
            var request = await _context.Requests.FindAsync(reopenRequest.Id);
            if (request == null)

                {
                     return NotFound();
                }

            request.Status = REOPENED;

            await _context.SaveChangesAsync();

            return Ok(request);
        }


        //Submit Request for Review
        [HttpPut]
        [Route("/submit-for-review")]
        public async Task<IActionResult> Review([FromBody] Request reviewRequest)
        {
            var request = await _context.Requests.FindAsync(reviewRequest.Id);

            if (request == null)
        {
            return NotFound();
        }

        request.Status = request.Total <= 50 ? APPROVED : REVIEW;
        request.SubmittedDate = DateTime.Now;

        await _context.SaveChangesAsync();

        return Ok(request);
        }

        //Get a list of Request where status = review || User can't view their own request
        [HttpGet]
        [Route("/list-review/{userId}")]
        public async Task<ActionResult<IEnumerable<Request>>> getRequestsForReview(int userId)
        {
            return await _context.Requests.Include(r => r.User)
                   .Where(r => r.Status.Equals("Review") &&! r.UserId.Equals(userId))
                   .ToListAsync();
        }

    }
}
