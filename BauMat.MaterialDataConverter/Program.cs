// The converter is meant to be run once against the source spreadsheet; the
// resulting JSON is then committed to the repo, so no arguments are needed.
var inputFile = "Data/materials.xlsx";
var outputFile = "../BauMat.Client/wwwroot/data/materials.json";

if (!File.Exists(inputFile))
{
    Console.Error.WriteLine($"Input file '{inputFile}' does not exist.");
    return;
}

Console.WriteLine($"Input File: {inputFile}");
Console.WriteLine($"Output File: {outputFile}");