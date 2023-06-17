using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace JinglePlanner.Models;

public enum RecipeType
{
    All,
    Christmas,
    Easter,
    Birthday,
}



public class Recipe
{
   
    public int Id { get; set; }
    [Required]
    [DisplayName("Recipe")]
     public string Name { get; set; } = default!;
    [Required]
    [DisplayName("Description")]
    public string Description { get; set; } = default!;
    [Required]
    [DisplayName("Ingredients")]
    public string Ingredients { get; set; } = default!;
    [Required]
    [DisplayName("Instructions")]
    public string Instructions { get; set; } = default!;
    [Required]
    [DisplayName("Type")]
    public RecipeType Type { get; set; }  = default!;
}

// dotnet aspnet-codegenerator controller -name RecipesController -m Recipe -dc JinglePlanner.Data.JinglePlannerContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite