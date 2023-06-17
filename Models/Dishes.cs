using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinglePlanner.Models;

public class Dish
{
    public int DishId { get; set; }
    [Required]
    [DisplayName("Dish")]
    public string Name { get; set; } = default!;
    public string GuestAndParty { get; set; } = default!;
    [DisplayName("Party")]
    public string? PartyName { get; set; } = default!;
    [DisplayName("Guest")]
    public string? GuestName { get; set; } = default!;
    [DisplayName("Description")]
    public string Description { get; set; } = default!;
    [DisplayName("Recipe Title")]
    public string? RecipeId { get; set; }
    public Recipe? Recipe { get; set; }

}


// dotnet aspnet-codegenerator controller -name DishesController -m Dish -dc JinglePlanner.Data.JinglePlannerContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite
