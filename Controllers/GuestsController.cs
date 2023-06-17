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
    public class GuestsController : Controller
    {
        private readonly JinglePlannerContext _context;

        public GuestsController(JinglePlannerContext context)
        {
            _context = context;
        }

        private string UserName(){
            if(HttpContext.Session.GetString("UserName") == null){
                return "";
            }
            return HttpContext.Session.GetString("UserName");
        }

        // GET: Guests
        public async Task<IActionResult> Index(string partyName, string searchString)
        {   
            string userName = UserName();
            if(userName == "") return RedirectToAction("Index", "Home");

            var parties = _context.Party.Where(p => p.Owner == userName).Select(p=>p.Name).Distinct().ToList();
            ViewBag.Parties = new SelectList(parties);

            if (!String.IsNullOrEmpty(partyName))
            {
                var guests = _context.Guest.Where(g => g.PartyName == partyName).Where(g=>parties.Contains(g.PartyName));
                guests = guests.OrderBy(g => g.Name);
                return View(await guests.ToListAsync());
            }

            if(!String.IsNullOrEmpty(searchString))
            {
                var guests = _context.Guest.Where(g => g.Name.Contains(searchString)).Where(g=>parties.Contains(g.PartyName));
                return View(await guests.ToListAsync());
            }

            if(userName == "admin"){
                var guestsAllAdmin = _context.Guest.Select(g=>g);
                guestsAllAdmin = guestsAllAdmin.OrderBy(g => g.Name);
                return View(await guestsAllAdmin.ToListAsync());
            }

            var guestsAll = _context.Guest.Where(g=>parties.Contains(g.PartyName));
            guestsAll = guestsAll.OrderBy(g => g.Name);
            return View(await guestsAll.ToListAsync());

        }

        // GET: Guests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Guest == null)
            {
                return NotFound();
            }

            var guest = await _context.Guest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // GET: Guests/Create
        public IActionResult Create()
        {
            string userName = UserName();
            var parties = _context.Party.Where(p => p.Owner == userName).Select(p=>p.Name).Distinct().ToList();
            ViewBag.Parties = new SelectList(parties);
            var users = _context.User.Select(u => u.UserName).Distinct().ToList();
            ViewBag.Users = new SelectList(users);
            return View();
        }

        // POST: Guests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Arrival,Departure,PartyName,Responsible")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                if(GuestAtPartyExists(guest.PartyName, guest.Name)){
                    TempData["ErrorMessage"] = "Guest already exists at this party.";
                    return RedirectToAction("Index", "Guests");
                }
                guest.Responsible = "test";
                AddGuestToParty(guest.PartyName, guest.Name);
                _context.Add(guest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(guest);
        }

        public void AddGuestToParty(string partyName, string guestName){
            //methods increas NumberOfGuests in Party
            var party = _context.Party.Where(p => p.Name == partyName).FirstOrDefault();
            if(party == null) return;
            party.NumberOfGuests++;
        }

        public bool GuestAtPartyExists(string partyName, string guestName)
        {
            return _context.Guest.Any(e => e.PartyName == partyName && e.Name == guestName);
        }

        // GET: Guests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Guest == null)
            {
                return NotFound();
            }

            var guest = await _context.Guest.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }
            return View(guest);
        }

        // POST: Guests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Arrival,Departure,PartyName")] Guest guest)
        {
            if (id != guest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestExists(guest.Id))
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
            return View(guest);
        }

        // GET: Guests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Guest == null)
            {
                return NotFound();
            }

            var guest = await _context.Guest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Guest == null)
            {
                return Problem("Entity set 'JinglePlannerContext.Guest'  is null.");
            }
            var guest = await _context.Guest.FindAsync(id);
            if (guest != null)
            {
                _context.Guest.Remove(guest);
                //methods decrease NumberOfGuests in Party
                var party = _context.Party.Where(p => p.Name == guest.PartyName).FirstOrDefault();
                if(party != null) party.NumberOfGuests--;
                //methods deletes all dishes for guest
                var dishes = _context.Dish.Where(d => d.GuestName == guest.Name).Where(d=>d.PartyName == guest.PartyName).ToList();
                if(dishes != null){
                    foreach(var dish in dishes){
                        _context.Dish.Remove(dish);
                    }
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuestExists(int id)
        {
          return (_context.Guest?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        
    }
}
