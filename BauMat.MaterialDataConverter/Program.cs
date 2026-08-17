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

Console.WriteLine("Excel File opened successfully.");


using var workbook = new XLWorkbook(inputFile);

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

for (var column = firstMaterialColumn; column <= lastUsedColumnNumber; column++)
{
    var materialId = worksheet.Cell(2, column).GetString().Trim();

    if (string.IsNullOrWhiteSpace(materialId))
    {
        break;
    }

    var materialName = worksheet.Cell(3, column).GetString().Trim();
    var metrics = new List<Metric>();
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
}

Console.WriteLine($"Input File: {inputFile}");
Console.WriteLine($"Output File: {outputFile}");

var metricName = worksheet.Cell(4, 1).GetString().Trim();
var metricSymbol = worksheet.Cell(4, 2).GetString().Trim();
var metricUnit = worksheet.Cell(4, 3).GetString().Trim();
var metricValue = worksheet.Cell(4, 4).GetString().Trim();

Console.WriteLine($"Metric: {metricName}, Symbol: {metricSymbol}, Unit: {metricUnit}, Value: {metricValue}");
public record Material(
    string Id, 
    string Name,
    List<Metric> Metrics
    );

public record Metric(
    string Name,
    string Symbol,
    string Unit,
    string Value
    );