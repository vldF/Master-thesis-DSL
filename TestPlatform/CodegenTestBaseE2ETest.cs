using NUnit.Framework;

namespace TestPlatform;

public class CodegenTestBaseE2ETest() : CodegenTestBase(TestDirectoriesUtils.GetCurrentProjectTestdataPath())
{
    protected string GetInput()
    {
        var testName = TestContext.CurrentContext.Test.Name;
        var inputPath = _testDataProvider.GetInputCodePath(testName);

        return File.ReadAllText(inputPath);
    }
}
