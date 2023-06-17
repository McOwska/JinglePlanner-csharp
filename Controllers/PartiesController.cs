using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JinglePlanner.Data;
using JinglePlanner.Models;
using Microsoft.AspNetCore.Http;

namespace JinglePlanner.Controllers
{
    public class PartiesController : Controller
    {
        private readonly JinglePlannerContext _context;

        public PartiesController(JinglePlannerContext context)
        {
            _context = context;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        private string UserName(){
            if(HttpContext.Session.GetString("UserName") == null){
                return "";
            }
            return HttpContext.Session.GetString("UserName");
        }
        // GET: Parties
        public async Task<IActionResult> Index()
        {
            string userName = UserName();
            if(userName == "") return RedirectToAction("Index", "Home");

            if(userName == "admin"){
                return _context.Party != null ? 
                          View(await _context.Party.ToListAsync()) :
                          Problem("Entity set 'JinglePlannerContext.Party'  is null.");
            }
              
            var parties = _context.Party.Where(p => p.Owner == userName).ToList();
            return View(parties);
        }

        // GET: Parties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Party == null)
            {
                return NotFound();
            }

            var party = await _context.Party
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        // GET: Parties/Create
        public IActionResult Create()
        {
            var users = _context.User.Select(u => u.UserName).Distinct().ToList();
            ViewBag.Users = new SelectList(users);
            return View();
        }

        // POST: Parties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,DateFrom,DateTo,Location, Owner")] Party party)
        {
            if (ModelState.IsValid)
            {
                if(PartyExists(party.Name)){
                    TempData["ErrorMessage"] = "Party with this name and host already exists.";
                    return RedirectToAction("Index", "Parties");
                }
                if(party.DateFrom > party.DateTo){
                    TempData["ErrorMessage"] = "Date from cannot be after date to.";
                    return RedirectToAction("Index", "Parties");
                }
                party.Owner = HttpContext.Session.GetString("UserName");
                _context.Add(party);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }
     
        public bool PartyExists(string PartyName)
        {
            string UserName = HttpContext.Session.GetString("UserName");
            return _context.Party.Any(p => p.Name == PartyName && p.Owner == UserName);
        }

        // GET: Parties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Party == null)
            {
                return NotFound();
            }

            var party = await _context.Party.FindAsync(id);
            if (party == null)
            {
                return NotFound();
            }
            return View(party);
        }

        // POST: Parties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,DateFrom,DateTo,Location,Owner")] Party party)
        {
            if (id != party.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(party);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartyExists(party.Id))
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
            return View(party);
        }

        // GET: Parties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Party == null)
            {
                return NotFound();
            }

            var user = HttpContext.Session.GetString("UserName");
            var partyOwner = _context.Party.Where(p => p.Id == id).Select(p => p.Owner).FirstOrDefault();
            
            if (user != partyOwner && user != "admin")
            {
                string message = $"Your are not allowed to delete this party.";
                TempData["ErrorMessage"] = message;
                return RedirectToAction("Index", "Parties");
            }

            var party = await _context.Party
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        // POST: Parties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Party == null)
            {
                return Problem("Entity set 'JinglePlannerContext.Party'  is null.");
            }
            var party = await _context.Party.FindAsync(id);
            if (party != null)
            {
                _context.Party.Remove(party);
                //remove all guests which take part in this party
                var guests = _context.Guest.Where(g => g.PartyName== party.Name).ToList();
                if(guests != null){
                    _context.Guest.RemoveRange(guests);
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartyExists(int id)
        {
          return (_context.Party?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public List<string> AllParties(){
            var parties = _context.Party.Select(p=>p.Name).Distinct().ToList();
            return parties;
        }

    
    }
}
