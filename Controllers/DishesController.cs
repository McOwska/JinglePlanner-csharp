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
    public class DishesController : Controller
    {
        private readonly JinglePlannerContext _context;

        public DishesController(JinglePlannerContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            string userName = UserName();
            var parties = _context.Party.Where(p => p.Owner == userName).Select(p=>p.Name).Distinct().ToList();

            var guestsAll = _context.Guest.Where(g=>parties.Contains(g.PartyName)).Select(g=>g.Name).Distinct().ToList();
           
            var dishes = _context.Dish.Where(d=>guestsAll.Contains(d.GuestName));

            return View(await dishes.ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dish == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.Recipe)
                .FirstOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }
        private string UserName(){
            if(HttpContext.Session.GetString("UserName") == null){
                return "";
            }
            return HttpContext.Session.GetString("UserName");
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            string userName = UserName();
            var recipies = _context.Recipe.Select(r => r.Name).ToList();
            var recipiesSelectList = new SelectList(recipies);
            //recipiesSelectList.Append(new SelectListItem { Text = "New Recipe", Value = "New Recipe" });
            ViewBag.Recipies = recipiesSelectList;
            var parties = _context.Party.Where(p => p.Owner == userName).Select(p=>p.Name).ToList();
           // var guests = _context.Guest.Where(g=>parties.Contains(g.PartyName));
            var guests = _context.Guest.Select(g=>g.Name);
            var guestsWithParties = new List<string>();
            foreach(var guest in guests){
                guestsWithParties.Add(guest + " - " + _context.Guest.Where(g=>g.Name == guest).Select(g=>g.PartyName).FirstOrDefault());
            }
            ViewBag.Guests = new SelectList(guestsWithParties);
            return View();

        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishId,Name,GuestName,Description,RecipeId")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Description", dish.RecipeId);
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dish == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Description", dish.RecipeId);
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DishId,Name,GuestName,Description,RecipeId")] Dish dish)
        {
            if (id != dish.DishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.DishId))
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
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Description", dish.RecipeId);
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dish == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.Recipe)
                .FirstOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dish == null)
            {
                return Problem("Entity set 'JinglePlannerContext.Dish'  is null.");
            }
            var dish = await _context.Dish.FindAsync(id);
            if (dish != null)
            {
                _context.Dish.Remove(dish);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
          return (_context.Dish?.Any(e => e.DishId == id)).GetValueOrDefault();
        }
    }
}
