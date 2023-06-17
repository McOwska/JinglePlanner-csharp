using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JinglePlanner.Models;

public class Guest
{
    
    public int Id { get; set; }
    [Required]
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    [DataType(DataType.Date)]
    public DateTime Arrival { get; set; }
    [DataType(DataType.Date)]
    public DateTime Departure { get; set; }
    
    public string PartyName { get; set; }

    public string Responsible { get; set; }

}


// dotnet aspnet-codegenerator controller -name GuestsController -m Guest -dc JinglePlanner.Data.JinglePlannerContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite


/*
Dodać tabelę znajomi tak, by móc dodawać do swoich imprez tylko znajomych
Zapraszanie do znajomych na zasadzie podwójnego zaproszenia - osoba A do osoby B i osoba B do osoby A

Wyświetlanie imprez tylko użytjkownika + tych na które użytkownik jest zaproszony

Dodanie ograniczeń na daty (od <= do)


*/