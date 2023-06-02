using System.ComponentModel.DataAnnotations;

namespace JinglePlanner.Models;

public enum RecipeType
{
    Christams,
    Easter,
    Birthday,
    Other,
    All
}



public class Recipe
{
   
    public int Id { get; set; }
    [Required]
 
    public string Name { get; set; } = default!;
    [Required]
    public string Description { get; set; } = default!;
    [Required]
    public string Ingredients { get; set; } = default!;
    [Required]
    public string Instructions { get; set; } = default!;
    [Required]
    public RecipeType Type { get; set; }  
}

// dotnet aspnet-codegenerator controller -name RecipesController -m Recipe -dc JinglePlanner.Data.JinglePlannerContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite