using me.vldf.jsa.dsl.ir.builder.builder;
using NUnit.Framework;
using Semantics.Ast2CgIrTranslator.Tests.options;
using TestPlatform;

namespace Semantics.Ast2CgIrTranslator.Tests;

public class PackageTests : PackageCodegenTestBase
{
    private readonly InputDataOptionsParser _optionsParser = new();

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

        var standardLibraryPath = _testDataProvider.GetStandardLibrary();
        var standardLibraryCode = File.ReadAllText(standardLibraryPath);

        var inputDataDir = Path.Combine(_testDataProvider.GetInputDirPath(), testName);
        var inputFiles = Directory.EnumerateFiles(inputDataDir);

        var codes = inputFiles
            .Select(inputFile => Path.Combine(inputDataDir, inputFile))
            .Select(f => (Path.GetFileName(f), File.ReadAllText(f)))
            .ToList();

        var standardLibraryFileName = Path.GetFileNameWithoutExtension(standardLibraryPath);
        codes.Add((standardLibraryFileName, standardLibraryCode));

        var astBuildingResult = astBuilder.FromStrings(codes);
        var options = _optionsParser.ParseDirOptions(inputDataDir);

        var actualTypeCheckErrors = astBuildingResult.Errors?.ToList().Select(x => x.ErrorCode).ToList() ?? [];
        var expectedTypeCheckErrors = options.OfType<ExpectedTypeCheckErrors>().SelectMany(x => x.Codes).ToList();

        if (actualTypeCheckErrors.Count != expectedTypeCheckErrors.Count)
        {
            var errorsDiffText = Utils.FormatTypeCheckerErrors(expectedTypeCheckErrors, actualTypeCheckErrors);
            Assert.Fail(errorsDiffText);
        }

        foreach (var fileAstNode in astBuildingResult.Files!)
        {
            if (fileAstNode.FileName == standardLibraryFileName)
            {
                continue;
            }

            Console.WriteLine(fileAstNode.String());

            var translator = new Ast2IrTranslator();
            var cgFile = translator.Translate(fileAstNode);

            Validate(cgFile);
        }
    }
}
