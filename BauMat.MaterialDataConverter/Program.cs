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

if (!workbook.Worksheets.TryGetWorksheet("ZW_Bet_KD", out var materialWorksheet))
{
    Console.Error.WriteLine("Worksheet 'ZW_Bet_KD' not found in the Excel file.");
    return;
}

Console.WriteLine($"Worksheet 'ZW_Bet_KD' found: {materialWorksheet.Name}");

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

        var cell = worksheet.Cell(row, column);

        var rawValue = cell.GetString().Trim();

        double? numericValue = null;
        string? textValue = null;

        if (cell.DataType == XLDataType.Number)
        { 
            numericValue = cell.GetDouble();

        }    

        else if (!string.IsNullOrWhiteSpace(rawValue) && !IsMissingValue(rawValue))
        {
            textValue = rawValue;
        }

        var metric = new Metric(
            Id: metricName,
            Name: metricName,
            Symbol: string.IsNullOrWhiteSpace(metricSymbol) ? null : metricSymbol,
            Unit: string.IsNullOrWhiteSpace(metricUnit) ? null : metricUnit,
            NumericValue: numericValue,
            TextValue: textValue,
            RawValue: rawValue,
            Remark: null,
            Link: null,
            References: new List<Reference>()
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
        Console.WriteLine(
            $"Metric: {metric.Name}, Numeric: {metric.NumericValue}, Text: {metric.TextValue}, Raw: {metric.RawValue}"
        );
    }
}

var albedoReference = materialWorksheet.Cell(17, 14).GetString().Trim();

Console.WriteLine($"Albedo Reference: {albedoReference}");

Console.WriteLine($"Input File: {inputFile}");
Console.WriteLine($"Output File: {outputFile}");

static bool IsMissingValue(string value)
{
    var normalizedValue = value?.Trim().ToLowerInvariant();

    return normalizedValue is
        "n. a."
        or "n. a"
        or "n.a."
        or "n.a"
        or "#value!"
        or "#n/a";
}

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