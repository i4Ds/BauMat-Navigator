namespace BauMat.Client.Models;

public enum ComparisonView
{
    Table,
    Cards
}

public static class ComparisonViewRoutes
{
    public static string Href(this ComparisonView view) =>
        view == ComparisonView.Cards
            ? "vergleich2"
            : "vergleich";
}
