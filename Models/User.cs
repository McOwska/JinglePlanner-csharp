using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JinglePlanner.Models;

public class User
{
    
    public int Id { get; set; }
    [Required]
 
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    
    public string? Email { get; set; }
}


// dotnet aspnet-codegenerator controller -name UsersController -m User -dc JinglePlanner.Data.JinglePlannerContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite
