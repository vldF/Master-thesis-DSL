using Codegen.Synthesizer;
using me.vldf.jsa.dsl.ir.builder.builder;
using NUnit.Framework;
using TestPlatform;

namespace Semantics.Ast2CgIrTranslator.Tests;

public class PackageTests : PackageCodegenTestBase
{
    [SetUp]
    public void Setup()
    {
        UpdateTests = false;
    }

    private static IEnumerable<TestCaseData> GetAllPackageTests()
    {
        return Directory
            .EnumerateDirectories(_testDataProvider.GetInputDirPath())
            .Select(Path.GetFileNameWithoutExtension)
            .Select(fileName => new TestCaseData(fileName)
            {
                TestName = fileName
            });
    }

    [TestCaseSource(nameof(GetAllPackageTests))]
    public void PackageTest(string testName)
    {
        var astBuilder = new AstBuilder();

        var inputDataDir = Path.Combine(_testDataProvider.GetInputDirPath(), testName);
        var inputFiles = Directory.EnumerateFiles(inputDataDir);

        var codes = inputFiles
            .Select(inputFile => Path.Combine(inputDataDir, inputFile))
            .Select(f => (Path.GetFileName(f), File.ReadAllText(f)))
            .ToList();

        var file = astBuilder.FromStrings(codes);
        foreach (var fileAstNode in file)
        {
            Console.WriteLine(fileAstNode.String());

            var translator = new Ast2IrTranslator();
            var cgFile = translator.Translate(fileAstNode);

            Validate(cgFile);
        }
    }
}
