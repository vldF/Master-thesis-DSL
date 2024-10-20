using NUnit.Framework;

namespace TestPlatform;

public class BaseE2ETest<T> : BaseCodegenTest<T>
{
    protected string GetInput()
    {
        var testName = TestContext.CurrentContext.Test.Name;
        var inputCode = _testDataProvider.GetInputCodeForTest(testName);

        if (inputCode == null)
        {
            Assert.Fail($"can't find input for test {testName}");
        }

        return inputCode;
    }
}
