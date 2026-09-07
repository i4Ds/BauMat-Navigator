namespace BauMat.Client.Models;

public record Material(
    int Id,
    string Name,
    string ConstructionType,
    double PetTag,
    double Albedo,
    int Lifespan,
    double Ghg,
    string Description,
    string Application,
    string Characteristics,
    string Notes,
    string Interpretation);
