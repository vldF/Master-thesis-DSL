using Codegen.Synthesizer;
using me.vldf.jsa.dsl.ir.builder.builder;
using Semantics.Ast2CgIrTranslator;

namespace Launcher;

internal static class Program
{
    public static void Main(string[] args)
    {
        var inputDirPath = args[0];
        var destinationDirPath = args[1];
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

        var standardLibraryPathJsa = "../../../.././StandardLibrary/Standard.jsa";
        var standardLibraryPathJsaTarget = Path.Join(destinationDirPath, "Standard.jsa");
        File.Copy(standardLibraryPathJsa, standardLibraryPathJsaTarget);
    }
}
