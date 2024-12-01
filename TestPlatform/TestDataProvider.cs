using System.Reflection;
using NUnit.Framework;

namespace TestPlatform;

public class TestDataProvider(string testdataRelPath)
{
    public string GetExpectedCodePath(string testName)
    {
        var fileNape = testName + ".jsa";

        return Path.Combine(GetExpectedDirPath(), fileNape);
    }

    public string GetInputCodePath(string testName)
    {
        var fileName = testName + ".jsadsl";
        return Path.Combine(GetInputDirPath(), fileName);
    }
    private string GetExpectedDirPath()
    {
        var testdataPath = GetDirectoryInProjectRoot(testdataRelPath);
        return Path.Combine(testdataPath, "expected");
    }

    private string GetInputDirPath()
    {
        var testdataPath = GetDirectoryInProjectRoot(testdataRelPath);
        return Path.Combine(testdataPath, "input");
    }

    private static string GetDirectoryInProjectRoot(string relDirPath)
    {
        return Path.Combine(GetProjectRootDirectory(), relDirPath);
    }
    private static string GetProjectRootDirectory()
    {
        return Directory.GetParent(TestContext.CurrentContext.TestDirectory)!.Parent!.FullName;
    }
}
