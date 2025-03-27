using System.Text;
using me.vldf.jsa.dsl.ir.builder.builder;
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

        var actualTypeCheckErrors = astBuildingResult.Errors?.ToList().Select(x => x.ErrorCode).ToList() ?? [];
        var expectedTypeCheckErrors = options.OfType<ExpectedTypeCheckErrors>().SelectMany(x => x.Codes).ToList();

        if (actualTypeCheckErrors.Count != expectedTypeCheckErrors.Count)
        {
            var missingErrors = expectedTypeCheckErrors.ToList();
            foreach (var actualTypeCheckError in actualTypeCheckErrors)
            {
                missingErrors.Remove(actualTypeCheckError);
            }

            var extraErrors = actualTypeCheckErrors.ToList();
            foreach (var expectedTypeCheckError in expectedTypeCheckErrors)
            {
                extraErrors.Remove(expectedTypeCheckError);
            }

            var error = new StringBuilder();
            error.AppendLine("missing errors:");
            foreach (var missingError in missingErrors)
            {
                error.AppendLine($" {missingError}");
            }

            error.AppendLine();
            error.AppendLine("extra errors:");
            foreach (var extraError in extraErrors)
            {
                error.AppendLine($" {extraError}");
            }

            Assert.Fail(error.ToString());
        }

        if (expectedTypeCheckErrors.Count != 0)
        {
            return;
        }

        var file = astBuildingResult.Files!.First();
        Console.WriteLine(file.String());

        var translator = new Ast2IrTranslator();
        var cgFile = translator.Translate(file);

        Validate(cgFile);
    }
}
