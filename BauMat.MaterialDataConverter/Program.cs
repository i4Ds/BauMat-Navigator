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

Console.WriteLine($"Input File: {inputFile}");
Console.WriteLine($"Output File: {outputFile}");