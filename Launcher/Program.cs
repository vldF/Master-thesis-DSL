using Codegen.Synthesizer;
using me.vldf.jsa.dsl.ir.builder.builder;
using Semantics.Ast2CgIrTranslator;

namespace Launcher;

internal static class Program
{
    public static void Main(string[] args)
    {
        var inputDirPath = args[0];
        var inputAdditionalFilesPaths = args[1];
        var destinationDirPath = args[2];
        var inputDir = new DirectoryInfo(inputDirPath);

        if (!inputDir.Exists)
        {
            throw new InvalidOperationException("directory doesn't exist");
        }

        var searchOptions = new EnumerationOptions
        {
            IgnoreInaccessible = true,
        };
        searchOptions.IgnoreInaccessible = true;
        var inputFiles = inputDir.EnumerateFiles("*.jsadsl", searchOptions);

        var codes = inputFiles
            .Select(inputFile => inputFile.FullName)
            .Select(f => (Path.GetFileName(f), File.ReadAllText(f)))
            .ToList();

        var standardLibraryPath = "../../../.././StandardLibrary/Standard.jsadsl";
        codes.Add((standardLibraryPath, File.ReadAllText(standardLibraryPath)));

        var astBuilder = new AstBuilder();
        var result = astBuilder.FromStrings(codes);

        if (result.Errors != null && result.Errors.Count != 0)
        {
            foreach (var resultError in result.Errors)
            {
                Console.Error.WriteLine(resultError!.Format());
            }

            Environment.Exit(1);
        }

        var outDir = new DirectoryInfo(destinationDirPath);
        if (outDir.Exists)
        {
            outDir.Delete(recursive: true);
        }
        outDir.Create();

        foreach (var fileAstNode in result.Files!)
        {
            if (Path.GetFileNameWithoutExtension(fileAstNode.FileName) == "Standard")
            {
                continue;
            }

            var translator = new Ast2IrTranslator();
            var cgFile = translator.Translate(fileAstNode);
            var resFilePath = Path.Join(destinationDirPath, cgFile.Name);
            var codegenSynthesizer = new CodegenSynthesizer();
            var actualCode = codegenSynthesizer.Synthesize(cgFile);

            File.WriteAllText(resFilePath, actualCode);
        }

        var additionalFiles = inputAdditionalFilesPaths.Split(",", StringSplitOptions.TrimEntries);
        var genFiles = result.Files.Select(f => Path.Join(destinationDirPath, f.FileName)).ToList();

        foreach (var additionalFilePath in additionalFiles)
        {
            var dir = new DirectoryInfo(additionalFilePath);
            if (dir.Exists)
            {
                CopyFilesRecursively(dir, new DirectoryInfo(destinationDirPath));
                continue;
            }

            var genFileIfFileIsCompl = GetGeneratedFileByComplementionName(additionalFilePath, genFiles);

            if (genFileIfFileIsCompl != null)
            {
                var complFileName = Path.GetFileName(additionalFilePath);
                var complFileNameWithoutExt = Path.GetFileNameWithoutExtension(additionalFilePath);
                var existingContent = File.ReadAllText(genFileIfFileIsCompl);
                var loadStr = $"#load \"{complFileName}\"\n";

                existingContent = loadStr + existingContent;
                existingContent += "\n";
                existingContent += $"// complementing content from {complFileName}\n";
                existingContent += $"Init{complFileNameWithoutExt}();\n";

                File.WriteAllText(genFileIfFileIsCompl, existingContent);
            }

            var file = new FileInfo(additionalFilePath);
            if (file.Exists)
            {
                File.Copy(additionalFilePath, Path.Join(destinationDirPath, file.Name));
            }
        }
    }

    private static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target) {
        foreach (var dir in source.GetDirectories())
            CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));

        foreach (var file in source.GetFiles())
            file.CopyTo(Path.Combine(target.FullName, file.Name));
    }

    private static string? GetGeneratedFileByComplementionName(string complName, IReadOnlyCollection<string> generatedFiles)
    {
        var nameWithoutExt = Path.GetFileNameWithoutExtension(complName);
        if (!nameWithoutExt.EndsWith("Complemention"))
        {
            return null;
        }

        var genFileName = nameWithoutExt.Replace("Complemention", "");
        var suitableFiles = generatedFiles
            .Where(f => Path.GetFileNameWithoutExtension(f) == genFileName)
            .ToList();
        return suitableFiles.Count == 0
            ? null
            : suitableFiles.Single().Replace(".jsadsl", ".jsa");
    }
}
