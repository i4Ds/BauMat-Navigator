using Microsoft.AspNetCore.Components;

namespace BauMat.Client.Pages;

/// <summary>
/// Katalog-Listenansicht einer Material-Kategorie (Issue #3).
/// Markup liegt in MaterialList.razor, Styling in MaterialList.razor.css.
/// </summary>
public partial class MaterialList
{
    [Parameter] public string Kategorie { get; set; } = "";

    // ── Mock-Daten ────────────────────────────────────────────────────────
    //
    // Inline-Daten für den Prototyp. Echte Daten aus dem PDF-Katalog
    // werden in Issue #11 (JSON + Deserialisierung) angebunden.

    private record Material(
        int Id,
        string Name,
        string Konstruktionstyp,
        double PetTag,        // °C, tiefer = besser
        double Albedo,        // 0..1, höher = mehr Reflexion
        int Lebensdauer,      // Jahre
        double Thg);          // kg CO2-eq/m², tiefer = besser

    private readonly List<Material> Materialien = new()
    {
        new(1, "Asphalt dunkel",          "Versiegelt",     38.2, 0.08, 25, 45),
        new(2, "Kiesbelag",               "Unversiegelt",   32.1, 0.30, 30, 12),
        new(3, "Rasen",                   "Begrünt",        28.5, 0.25, 15,  5),
        new(4, "Steinplattenpflästerung", "Versiegelt",     35.0, 0.20, 60, 65),
        new(5, "Betonpflaster, hell",     "Teilversiegelt", 33.5, 0.45, 40, 38),
    };

    // ── Filter ────────────────────────────────────────────────────────────

    private const string Alle = "Alle";
    private string aktiverTyp = Alle;

    private IEnumerable<string> Konstruktionstypen =>
        new[] { Alle }
            .Concat(Materialien.Select(m => m.Konstruktionstyp).Distinct().OrderBy(s => s));

    private IEnumerable<Material> GefilterteMaterialien =>
        aktiverTyp == Alle
            ? Materialien
            : Materialien.Where(m => m.Konstruktionstyp == aktiverTyp);

    private string FilterStatus
    {
        get
        {
            var total = Materialien.Count;
            var gezeigt = GefilterteMaterialien.Count();
            return aktiverTyp == Alle
                ? $"{total} Materialien"
                : $"{gezeigt} von {total} Materialien";
        }
    }

    private void SetzeFilter(string typ) => aktiverTyp = typ;


    // ── Panel-Sichtbarkeit ────────────────────────────────────────────────

    private bool filterOffen = false;
    private bool gewichtungOffen = true;

    private void ToggleFilter() => filterOffen = !filterOffen;
    private void ToggleGewichtung() => gewichtungOffen = !gewichtungOffen;

    // ── Gewichtung ────────────────────────────────────────────────────────
    //
    // TODO: Die Gewichte werden noch nicht auf die Tabellen-Sortierung
    // angewendet.die eigentliche gewichtete Sortierung folgt in einem späteren Schritt (MCDM).

    private static readonly string[] Gewichtsparameter =
        { "PET Tag", "Albedo", "Lebensdauer", "THG" };

    private readonly Dictionary<string, int> gewichte = new();

    protected override void OnInitialized()
    {
        foreach (var p in Gewichtsparameter) gewichte[p] = 0;
    }

    private void SetzeGewicht(string parameter, object? value)
    {
        if (int.TryParse(value?.ToString(), out var g))
        {
            gewichte[parameter] = Math.Clamp(g, 0, 5);
        }
    }

    private void GewichteZuruecksetzen()
    {
        foreach (var p in Gewichtsparameter) gewichte[p] = 0;
    }

    // ── Navigation zur Detail-Seite ───────────────────────────────────────

    [Inject] private NavigationManager Nav { get; set; } = default!;

    private void OeffneDetail(int id) => Nav.NavigateTo($"/material/{id}");

}
