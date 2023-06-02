using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JinglePlanner.Data;
using JinglePlanner.Models;
using NuGet.Packaging;

namespace JinglePlanner.Controllers
{
    public class RecipesController : Controller
    {
        private readonly JinglePlannerContext _context;

        public RecipesController(JinglePlannerContext context)
        {
            _context = context;
        }

        // GET: Recipes
        public async Task<IActionResult> Index(string? recipeType, string? searchString)
        {
            
           
            ViewBag.recipeTypes = new SelectList(Enum.GetNames(typeof(RecipeType)));

            var recipes = from m in _context.Recipe
                         select m;
            
            
            if(!String.IsNullOrEmpty(recipeType))
            {
                RecipeType enumValue = (RecipeType)Enum.Parse(typeof(RecipeType), recipeType);

                if (recipeType != "All")
                {
                    recipes = recipes.Where(x => x.Type == enumValue);
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    recipes = recipes.Where(s => s.Name.Contains(searchString));
                }
            }

            

            return View(recipes);
            
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(HttpContext.Session.GetString("IsLoggedIn") == "true"){
                if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
            }
            return RedirectToAction("Index", "Home");
            
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("IsLoggedIn") == "true"){
                var types = Enum.GetValues(typeof(RecipeType)).Cast<RecipeType>();
                ViewBag.Types = new SelectList(types);
                return View();
            }

            
            return RedirectToAction("Index", "Home");
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Ingredients,Instructions,Type")] Recipe recipe)
        {
            if(HttpContext.Session.GetString("IsLoggedIn") == "true"){
            if (ModelState.IsValid)
            {
                var allRecipes = _context.Recipe.ToList();
                var recipeExists = allRecipes.Any(x => x.Name == recipe.Name);
                if (recipeExists)
                {
                    TempData["ErrorMessage"] = "Recipe already exists.";
                   
                    return RedirectToAction("Index", "Recipes");
                }
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "true"){
                if (id == null || _context.Recipe == null)
                {
                    return NotFound();
                }

                var recipe = await _context.Recipe.FindAsync(id);
                if (recipe == null)
                {
                    return NotFound();
                }
                return View(recipe);
            }
            return RedirectToAction("Index", "Recipes");
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Ingredients,Instructions,Type")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
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
            TempData["ErrorMessage"] = "Only admin can edit recipes";
            return View(recipe);
        }


        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "true"){
                if (id == null || _context.Recipe == null)
                {
                    return NotFound();
                }

                var recipe = await _context.Recipe
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (recipe == null)
                {
                    return NotFound();
                }

                return View(recipe);
            }
            TempData["ErrorMessage"] = "Only admin can delete recipes";
            return RedirectToAction("Index", "Recipes");
            
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "true"){
                if (_context.Recipe == null)
                {
                    return Problem("Entity set 'JinglePlannerContext.Recipe'  is null.");
                }
                var recipe = await _context.Recipe.FindAsync(id);
                if (recipe != null)
                {
                    _context.Recipe.Remove(recipe);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Nie masz uprawnień do usunięcia przepisu.";
            return RedirectToAction("Index", "Recipes");
            
        }

        private bool RecipeExists(int id)
        {
          return (_context.Recipe?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
