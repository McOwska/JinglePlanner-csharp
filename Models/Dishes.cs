using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinglePlanner.Models;

public class Dish
{
    public int DishId { get; set; }
    [Required]
    public string Name { get; set; } = default!;
    public string GuestName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int? RecipeId { get; set; }
    public Recipe? Recipe { get; set; }

}


// dotnet aspnet-codegenerator controller -name DishesController -m Dish -dc JinglePlanner.Data.JinglePlannerContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite
