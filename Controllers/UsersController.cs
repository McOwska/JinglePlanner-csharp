using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JinglePlanner.Data;
using JinglePlanner.Models;

namespace JinglePlanner.Controllers
{
    public class UsersController : Controller
    {
        private readonly JinglePlannerContext _context;

        public UsersController(JinglePlannerContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetString("IsAdmin") == "true"){
                return _context.User != null ? 
                          View(await _context.User.ToListAsync()) :
                          Problem("Entity set 'JinglePlannerContext.User'  is null.");
            }
            return RedirectToAction("Index", "Home");
              
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "true"){

            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.UserName == "admin")
            {
                TempData["ErrorMessage"] = "Admin user cannot be deleted.";
                Console.WriteLine("Admin user cannot be deleted.");
                return View(user);
            }

            return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("IsAdmin") == "true"){
            return View();
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,Email")] User user)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "true"){
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "true"){
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (id == 1){
                TempData["ErrorMessage"] = "Admin user cannot be edited.";
                 return RedirectToAction("Index", "Users");
            }
            return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,Email")] User user)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "true"){
            if (id != user.Id)
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
                    if (!UserExists(user.Id))
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
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "true"){
                if( await _context.User.Select(u=>u.Id == id).FirstOrDefaultAsync())
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (id == 1){
                TempData["ErrorMessage"] = "Admin user cannot be deleted.";
                 return RedirectToAction("Index", "Users");
            }

            return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "true"){
            if (_context.User == null)
            {
                return Problem("Entity set 'JinglePlannerContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Home");
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                var allUsers = _context.User.Select(u=>u.UserName).Distinct().ToList();
                if(allUsers.Contains(model.UserName))
                {
                    ModelState.AddModelError("UserName", "Username already exists");
                    return View(model);
                }
                var user = new User { UserName = model.UserName, Email = model.Email, Password = model.Password };
                var result = await _context.User.AddAsync(user);
                if (result != null)
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Logged", "Home");
                }
            }
            return View(model);
        }
        
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.UserName == model.UserName && u.Password == model.Password);
                if (user != null)
                {
                    if(user.UserName == "admin")
                    {
                        HttpContext.Session.SetString("IsAdmin", "true");
                        HttpContext.Session.SetString("IsLoggedIn", "true");
                        HttpContext.Session.SetString("UserName", user.UserName);
                        // HttpContext.Session.SetString("UserId", user.Id);
                        return RedirectToAction("Admin", "Home");
                    }
                    HttpContext.Session.SetString("IsLoggedIn", "true");
                    HttpContext.Session.SetString("IsAdmin", "false");
                    HttpContext.Session.SetString("UserName", user.UserName);
                    return RedirectToAction("Logged", "Home");
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
