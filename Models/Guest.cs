using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinglePlanner.Models;

public class Guest
{
    
    public int Id { get; set; }
    [Required]
    [DisplayName("Guest")]
    public string Name { get; set; }
    [DisplayName("Email")]   
    public string Email { get; set; }
    [DataType(DataType.Date)]
    [DisplayName("Arrival")]
    public DateTime Arrival { get; set; }
    [DisplayName("Departure")]
    [DataType(DataType.Date)]
    public DateTime Departure { get; set; }
    [DisplayName("Party")]
    
    public string PartyName { get; set; }

    public string Responsible { get; set; }

}


// dotnet aspnet-codegenerator controller -name GuestsController -m Guest -dc JinglePlanner.Data.JinglePlannerContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite
