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

Console.WriteLine("Worksheet 'Daten_Alle' found.");
Console.WriteLine("Excel File opened successfully.");
Console.WriteLine($"Input File: {inputFile}");
Console.WriteLine($"Output File: {outputFile}");