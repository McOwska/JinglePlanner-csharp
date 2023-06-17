using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinglePlanner.Models;

public class Party
{
   
    public int Id { get; set; }
    [Required]
    [DisplayName("Party")]
    public string Name { get; set; }
    [DisplayName("Description")]
    public string? Description { get; set; }
    [DataType(DataType.Date)]
    [DisplayName("Date From")]
    public DateTime DateFrom { get; set; }
    [DataType(DataType.Date)]
    [DisplayName("Date To")]
    public DateTime DateTo { get; set; }
    [DisplayName("Location")]
    public string? Location { get; set; }
    [DisplayName("Host")]
   
    public string Owner { get; set; }
    [DisplayName("Number of Guests")]
    public int NumberOfGuests { get; set; } = 0;
}


// dotnet aspnet-codegenerator controller -name PartiesController -m Party -dc JinglePlanner.Data.JinglePlannerContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite --force
