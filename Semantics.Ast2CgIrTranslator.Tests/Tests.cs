using me.vldf.jsa.dsl.ir.builder.builder;
using me.vldf.jsa.dsl.ir.builder.checkers;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using NUnit.Framework;
using Semantics.Ast2CgIrTranslator.Tests.options;
using TestPlatform;

namespace Semantics.Ast2CgIrTranslator.Tests;

public class Tests : SingleFileCodegenTestBase
{
    private readonly InputDataOptionsParser _optionsParser = new();

    [SetUp]
    public void Setup()
    {
        UpdateTests = false;
    }

    private static IEnumerable<TestCaseData> GetAllTests()
    {
        return Directory
            .EnumerateFiles(_testDataProvider.GetInputDirPath())
            .Select(Path.GetFileNameWithoutExtension)
            .Select(fileName => new TestCaseData(fileName)
            {
                TestName = fileName
            });
    }

    [TestCaseSource(nameof(GetAllTests))]
    public void Test(string testName)
    {
        var standardLibraryPath = _testDataProvider.GetStandardLibrary();
        var standardLibraryCode = File.ReadAllText(standardLibraryPath);

        var inputPath = _testDataProvider.GetInputCodePath(testName);
        var options = _optionsParser.ParseFileOptions(inputPath);

        var inputCode = File.ReadAllText(inputPath);

        var astBuilder = new AstBuilder();
        var astBuildingResult = astBuilder.FromStrings([
            (testName, inputCode),
            (Path.GetFileNameWithoutExtension(standardLibraryPath), standardLibraryCode)
        ]);

        var expectedErrorCodes = options.OfType<ExpectedTypeCheckErrors>().SelectMany(x => x.Codes).ToList();
        if (!CheckTypeErrors(astBuildingResult, expectedErrorCodes))
        {
            return;
        }

        if (options.Any(x => x is DumpIr))
        {
            DumpIr(astBuildingResult.Files!.First());
        }

        var file = astBuildingResult.Files!.First();
        Console.WriteLine(file.String());

        var translator = new Ast2IrTranslator();
        var cgFile = translator.Translate(file);

        Validate(cgFile);
    }

    private static bool CheckTypeErrors(
        AstBuildingResult astBuildingResult,
        IReadOnlyCollection<ErrorCode> expectedErrors)
    {
        var actualTypeCheckErrors = astBuildingResult.Errors?.Select(x => x.ErrorCode).ToList() ?? [];
        var expectedTypeCheckErrors = expectedErrors.ToList();

        if (actualTypeCheckErrors.Count == expectedTypeCheckErrors.Count)
        {
            return expectedTypeCheckErrors.Count == 0;
        }

        var errorsDiffText = Utils.FormatTypeCheckerErrors(expectedTypeCheckErrors, actualTypeCheckErrors);
        Assert.Fail(errorsDiffText);

        return false;
    }

    private void DumpIr(FileAstNode file)
    {
        var resultDir = _testDataProvider.GetExpectedDirPath();

        Utils.DumpIr(file, resultDir);
    }
}
