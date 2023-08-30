using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreCommerce.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CoreCommerce.Controllers
{
    public class UsersController : Controller
    {
        private readonly Context _context;

        public UsersController(Context context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("User")!="admin") {
                return RedirectToAction(nameof(Connect));
            }
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Pseudo,Password,Role")] User user)
        {
            using (SHA256 sha256Hash = SHA256.Create())
                if (ModelState.IsValid)
            {
                    // Create a SHA256  
                    // ComputeHash - returns byte array 
                    user.Password = BitConverter.ToString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(user.Password))).Replace("-", String.Empty);
                    _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        // GET: Users/Init
        public ActionResult Init()
        {
            return View();
        }

        // GET: Users/Connect
        public ActionResult Connect([Bind("UserId,Pseudo,Password")] User user)
        {
            using (SHA256 sha256Hash = SHA256.Create())
                if (ModelState.IsValid)
                {
                    //hash the password in sha256
                    string HashPassword = BitConverter.ToString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(user.Password))).Replace("-", String.Empty);
                    User userConnected = _context.Users.FirstOrDefault(u => u.Password == HashPassword);
                    if (userConnected != null)
                    {
                        HttpContext.Session.SetString("User",userConnected.Role);

                        if (userConnected.Role == "admin")
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            return RedirectToAction(nameof(Connect));
                        }
                    }
                }
            return View(user);
        }
 
        // POST: Users/Init
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Init([Bind("UserId,Pseudo,Password")] User user)
        {
            User userModified = _context.Users.SingleOrDefault(a => a.Pseudo == user.Pseudo);
            using (SHA256 sha256Hash = SHA256.Create())
                if (userModified != null)
                {
                    // Create a SHA256  
                    // ComputeHash - returns byte array 
                    userModified.Password = BitConverter.ToString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(user.Password))).Replace("-", String.Empty);
                    _context.Entry(userModified).State = EntityState.Modified;
                    _context.SaveChanges();

                    return RedirectToAction("Connect");
                }
            return View(user);
        }


        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Pseudo,Password,Role")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
