using ClosedXML.Excel;


if (args.Length < 2)
{
    Console.WriteLine("Usage: BauMat.MaterialDataConverter <inputFile> <outputFile>");
    return;
}

var inputFile = args[0];
var outputFile = args[1];

if (!File.Exists(inputFile))
{
    Console.Error.WriteLine($"Input file '{inputFile}' does not exist.");
    return;
}

using var workbook = new XLWorkbook(inputFile);

Console.WriteLine("Excel File opened successfully.");

if (!workbook.Worksheets.TryGetWorksheet("Daten_alle", out var worksheet))
{
    Console.Error.WriteLine("Worksheet 'Daten_alle' not found in the Excel file.");
    return;
}

Console.WriteLine("Worksheet 'Daten_alle' found.");

var firstMaterialColumn = 4;
var lastUsedColumn = worksheet.LastColumnUsed();

if (lastUsedColumn is null)
{
    Console.Error.WriteLine("Daten_alle is empty.");
    return;
}

var lastUsedColumnNumber = lastUsedColumn.ColumnNumber();

var materials = new List<Material>();

var firstMetricRow = 4;

for (var column = firstMaterialColumn; column <= lastUsedColumnNumber; column++)
{
    var materialId = worksheet.Cell(2, column).GetString().Trim();
    var materialName = worksheet.Cell(3, column).GetString().Trim();

    if (string.IsNullOrWhiteSpace(materialId))
    {
        break;
    }

    var metrics = new List<Metric>();

    for (var row = firstMetricRow; ; row++)
    {
        var metricName = worksheet.Cell(row, 1).GetString().Trim();
        var metricSymbol = worksheet.Cell(row, 2).GetString().Trim();
        var metricUnit = worksheet.Cell(row, 3).GetString().Trim();

        if (string.IsNullOrWhiteSpace(metricName) && string.IsNullOrWhiteSpace(metricSymbol) && string.IsNullOrWhiteSpace(metricUnit))
        {
            break;
        }

        var metricValue = worksheet.Cell(row, column).GetString().Trim();

        var metric = new Metric(
        metricName,
        metricSymbol,
        metricUnit,
        metricValue
        );

        metrics.Add(metric);
    }

    var material = new Material(
        materialId,
        materialName,
        metrics
    );

    materials.Add(material);
}

foreach (var material in materials)
{
    Console.WriteLine($"Material ID: {material.Id}, Material Name: {material.Name}");

    foreach (var metric in material.Metrics)
    {
        Console.WriteLine($"Metric: {metric.Name}, Symbol: {metric.Symbol}, Unit: {metric.Unit}, Value: {metric.Value}");
    }
}

Console.WriteLine($"Input File: {inputFile}");
Console.WriteLine($"Output File: {outputFile}");

public record Material(

    string Id,
    string Name,
    List<Metric> Metrics
);

public record Metric(
    string Id,
    string Name,
    string? Symbol,
    string? Unit,
    double? NumericValue,
    string? TextValue,
    string? RawValue,
    string? Remark,
    string? Link,
    List<Reference> References
);

public record Reference(
    string? Key,
    string? Citation
);