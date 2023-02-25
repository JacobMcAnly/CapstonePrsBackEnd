using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsBackEnd.Models;

namespace PrsBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PrsDbContext _context;

        public UsersController(PrsDbContext context)
        {
            _context = context;
        }

        //[Route("/login")]
        //[HttpPost]
        //public async Task<ActionResult<User>> LoginUser(string userName, string password)
        //{
        //    var user = await _context.Users.Where(u => u.Username == userName && u.Password == password).FirstOrDefaultAsync();

        //    if (user == null)
        //    {
        //        return NotFound(); //404
        //    }
        //    return user; // best practice: only return whats needed
        //}
        

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        [Route("/login")]
        [HttpPost]
        public async Task<ActionResult<User>> LoginUser([FromBody] UserPasswordObject upo)
        {
            var user = await _context.Users.Where(u => u.Username == upo.username && u.Password == upo.password).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();  // 404
            }

            return user;

            //return new { Firstname = user.Firstname, Lastname = user.Lastname, Id = user.Id, IsAdmin = user.IsAdmin }; //best practice: only return what's needed!

            // return new { Id = user.Id, Firstname = user.Firstname, Lastname = user.Lastname, LogOnTime = DateTime.Now}
        }

    }
}
