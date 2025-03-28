using System.Text.RegularExpressions;

namespace Semantics.Ast2CgIrTranslator.Tests.options;

public partial class InputDataOptionsParser
{
    public IReadOnlyCollection<IInputDataOption> ParseFileOptions(string filePath)
    {
        var optionPattern = MyRegex();
        var fileText = File.ReadLines(filePath);
        var optionLines = fileText.TakeWhile(l => l.StartsWith("//"));
        return optionLines.Select(optLine =>
        {
            var match = optionPattern.Match(optLine);
            var argsText = match.Groups["args"].Value;
            var args = argsText.Split(',', StringSplitOptions.TrimEntries);

            var name = match.Groups["name"].Value;

            return IInputDataOption.Parser(name, args);
        }).ToList();
    }

    public IReadOnlyCollection<IInputDataOption> ParseDirOptions(string dirPath)
    {
        var directoryInfo = new DirectoryInfo(dirPath);
        return directoryInfo
            .EnumerateFiles("*.jsadsl", new EnumerationOptions())
            .SelectMany(f => ParseFileOptions(f.FullName))
            .ToList();
    }

    [GeneratedRegex(@"\/\/\s*#(?<name>[a-zA-Z0-9-_\.]+)\s*(\((?<args>[a-zA-Z_0-9\-,\s]*)\))?")]
    private static partial Regex MyRegex();
}
