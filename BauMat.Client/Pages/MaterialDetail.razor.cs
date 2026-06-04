using Microsoft.AspNetCore.Components;

namespace BauMat.Client.Pages;

/// <summary>
/// Detailansicht eines einzelnen Materials (Issue #3).
/// Markup liegt in MaterialDetail.razor, Styling in MaterialDetail.razor.css.
/// </summary>
public partial class MaterialDetail
{
    [Parameter] public int Id { get; set; }

    // ── Mock-Daten ────────────────────────────────────────────────────────
    //
    // Inline-Daten für den Prototyp. Aktuell dupliziert mit MaterialList —
    // eine zentrale Datenquelle (Service oder JSON) gehört zu Issue #11
    // und wird dort eingeführt.

    private record Material(
        int Id,
        string Name,
        string Konstruktionstyp,
        string Beschreibung,
        double PetTag,
        double Albedo,
        int Lebensdauer,
        double Thg);

    private static readonly List<Material> AlleMaterialien = new()
    {
        new(1, "Asphalt dunkel", "Versiegelt",
            "Klassischer dunkler Asphaltbelag. Geringe Albedo, hohe Aufheizung am Tag.",
            38.2, 0.08, 25, 45),
        new(2, "Kiesbelag", "Unversiegelt",
            "Lockerer Kiesbelag mit guter Versickerung und mittlerer Albedo.",
            32.1, 0.30, 30, 12),
        new(3, "Rasen", "Begrünt",
            "Natürlicher Rasenbelag mit hoher Kühlwirkung durch Evapotranspiration.",
            28.5, 0.25, 15, 5),
        new(4, "Steinplattenpflästerung", "Versiegelt",
            "Natursteinplatten in Mörtelbett, sehr langlebig, aber THG-intensiv.",
            35.0, 0.20, 60, 65),
        new(5, "Betonpflaster, hell", "Teilversiegelt",
            "Helles Betonpflaster mit moderater Albedo und langer Lebensdauer.",
            33.5, 0.45, 40, 38),
    };

    private Material? Gefunden => AlleMaterialien.FirstOrDefault(m => m.Id == Id);

    // ── Parameter-Liste für die Tabelle ───────────────────────────────────
    //
    // Hier werden die Parameter als Liste aufbereitet — dieselbe Form,
    // die Issue #11 später aus dem zentralen Datenmodell liefern wird.
    // Wenn das echte Modell kommt, ersetzt sich nur der Methoden-Body;
    // das Markup bleibt unverändert.

    private record Parameter(string Name, string Wert, string Einheit);

    private IEnumerable<Parameter> ParameterTabelle()
    {
        if (Gefunden is null) yield break;

        yield return new("PET Tag", Gefunden.PetTag.ToString("0.0"), "°C");
        yield return new("Albedo", Gefunden.Albedo.ToString("0.00"), "-");
        yield return new("Lebensdauer", Gefunden.Lebensdauer.ToString(), "Jahre");
        yield return new("THG", Gefunden.Thg.ToString("0"), "kg CO₂-eq/m²");
    }
}