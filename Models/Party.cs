using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinglePlanner.Models;

public class Party
{
   
    public int Id { get; set; }
    [Required]
  
    public string Name { get; set; }
    public string? Description { get; set; }
    [DataType(DataType.Date)]
    public DateTime DateFrom { get; set; }
    [DataType(DataType.Date)]
    public DateTime DateTo { get; set; }
    public string? Location { get; set; }
   
    public string Owner { get; set; }

    public int NumberOfGuests { get; set; } = 0;
}


// dotnet aspnet-codegenerator controller -name PartiesController -m Party -dc JinglePlanner.Data.JinglePlannerContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite --force
