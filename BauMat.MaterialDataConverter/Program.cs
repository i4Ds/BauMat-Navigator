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

if (!workbook.Worksheets.TryGetWorksheet("Daten_Alle", out var worksheet))
{
    Console.Error.WriteLine("Worksheet 'Daten_Alle' not found in the Excel file.");
    return;
}


var firstMaterialColumn = 4;
var lastUsedColumn = worksheet.LastColumnUsed();

if (lastUsedColumn is null)
{
    Console.Error.WriteLine("Daten_Alle ist Leer");
    return;
}

var lastUsedColumnNumber = lastUsedColumn.ColumnNumber();

for (var column = firstMaterialColumn; column <= lastUsedColumnNumber; column++)
{
    var materialId = worksheet.Cell(2, column).GetString().Trim();

    if (string.IsNullOrWhiteSpace(materialId))
    {
        break;
    }

    var materialName = worksheet.Cell(3, column).GetString().Trim();

    Console.WriteLine($"Material Name: {materialName}, MaterialId: {materialId}");
}

Console.WriteLine("Worksheet 'Daten_Alle' found.");
Console.WriteLine("Excel File opened successfully.");
Console.WriteLine($"Input File: {inputFile}");
Console.WriteLine($"Output File: {outputFile}");