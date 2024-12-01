using System.Text;
using Codegen.IR.nodes;
using Codegen.Synthesizer;
using NUnit.Framework;

namespace TestPlatform;

public abstract class CodegenTestBase
{
    protected static readonly TestDataProvider _testDataProvider = new();

    protected bool UpdateTests = false;

    protected void Validate(CgFile cgFile)
    {
        var testName = TestContext.CurrentContext.Test.Name;
        var expectedPath = _testDataProvider.GetExpectedCodePath(testName);
        var expectedCode = !File.Exists(expectedPath) ? null : File.ReadAllText(expectedPath);

        var codegenSynthesizer = new CodegenSynthesizer();
        var actualCode = codegenSynthesizer.Synthesize(cgFile).EnsureTrailingNewLine();

        if (expectedCode == null || UpdateTests)
        {
            UpdateExpectedFile(testName, actualCode);
            return;
        }

        if (expectedCode == actualCode)
        {
            return;
        }

        Console.Error.WriteLine("Test failed!");
        var tempPath = TestDirectoriesUtils.GetCurrentProjectTestTempPath();
        var actualPath = Path.Combine(tempPath, testName + ".jsa");
        File.WriteAllText(actualPath, actualCode);

        DiffUtils.PrintDiffForFiles(expectedPath, actualPath);

        Assert.Fail();
    }

    private void UpdateExpectedFile(string testName, string actual)
    {
        var physicalTestDataPath = _testDataProvider.GetExpectedCodePath(testName);
        var file = File.Open(physicalTestDataPath, FileMode.Create);
        file.Write(Encoding.UTF8.GetBytes(actual));
        file.Close();
    }
}
