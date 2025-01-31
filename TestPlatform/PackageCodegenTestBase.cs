using System.IO.Enumeration;
using System.Text;
using Codegen.IR.nodes;
using Codegen.Synthesizer;
using NUnit.Framework;

namespace TestPlatform;

public abstract class PackageCodegenTestBase
{
    protected static readonly TestDataProvider _testDataProvider = new();

    protected bool UpdateTests = false;

    protected void Validate(CgFile cgFile)
    {
        var testName = TestContext.CurrentContext.Test.Name;
        var expectedPath = _testDataProvider.GetExpectedCodePath(testName, cgFile.Name);
        var expectedCode = !File.Exists(expectedPath) ? null : File.ReadAllText(expectedPath);

        var codegenSynthesizer = new CodegenSynthesizer();
        var actualCode = codegenSynthesizer.Synthesize(cgFile).EnsureTrailingNewLine();

        if (expectedCode == null || UpdateTests)
        {
            UpdateExpectedFile(testName, Path.GetFileNameWithoutExtension(cgFile.Name), actualCode);
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

    private void UpdateExpectedFile(string testName, string fileName, string actual)
    {
        var physicalTestDataPath = _testDataProvider.GetExpectedCodePath(testName, fileName);
        var parentDir = Directory.GetParent(physicalTestDataPath);
        parentDir!.Create();

        var file = File.Open(physicalTestDataPath, FileMode.Create);
        file.Write(Encoding.UTF8.GetBytes(actual));
        file.Close();
    }
}
