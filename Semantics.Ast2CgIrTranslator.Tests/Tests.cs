using me.vldf.jsa.dsl.ir.builder.builder;
using NUnit.Framework;
using TestPlatform;

namespace Semantics.Ast2CgIrTranslator.Tests;

public class Tests : SingleFileCodegenTestBase
{
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
        var inputCode = File.ReadAllText(inputPath);

        var astBuilder = new AstBuilder();
        var astBuildingResult = astBuilder.FromStrings([
            (testName, inputCode),
            (Path.GetFileNameWithoutExtension(standardLibraryPath), standardLibraryCode)
        ]);

        var errors = astBuildingResult.Errors;
        if (errors != null && errors.Count != 0)
        {
            var formattedErrors = errors.Aggregate("", (s, error) => s + "\n\n" + error.Format());
            Assert.Fail(formattedErrors);
            return;
        }

        var file = astBuildingResult.Files!.First();
        Console.WriteLine(file.String());

        var translator = new Ast2IrTranslator();
        var cgFile = translator.Translate(file);

        Validate(cgFile);
    }
}
