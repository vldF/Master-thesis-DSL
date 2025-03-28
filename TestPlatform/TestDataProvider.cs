using NUnit.Framework;

namespace TestPlatform;

public class TestDataProvider
{
    private readonly string _testdataRelPath = TestDirectoriesUtils.GetCurrentProjectTestdataPath();

    public string GetExpectedCodePath(string testName)
    {
        var fileName = testName + ".jsa";

        return Path.Combine(GetExpectedDirPath(), fileName);
    }

    public string GetExpectedCodePath(string testName, string fileName)
    {
        var dirPath = Path.Combine(GetExpectedDirPath(), testName);
        fileName += ".jsa";

        return Path.Combine(dirPath, fileName);
    }

    public string GetInputCodePath(string testName)
    {
        var fileName = testName + ".jsadsl";
        return Path.Combine(GetInputDirPath(), fileName);
    }

    public string GetInputCodePath(string testName, string fileName)
    {
        var dirPath = Path.Combine(GetExpectedDirPath(), testName);
        fileName += ".jsadsl";

        return Path.Combine(dirPath, fileName);
    }

    public string GetStandardLibrary()
    {
        return Path.Combine(GetDirectoryInSolutionRoot("StandardLibrary"), "Standard.jsadsl");
    }

    public string GetExpectedDirPath()
    {
        var testdataPath = GetDirectoryInProjectRoot(_testdataRelPath);
        return Path.Combine(testdataPath, "expected");
    }

    public string GetInputDirPath()
    {
        var testdataPath = GetDirectoryInProjectRoot(_testdataRelPath);
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

    private static string GetDirectoryInSolutionRoot(string relDirPath)
    {
        var solDir = Directory.GetParent(GetProjectRootDirectory())!.Parent!;
        return Path.Combine(solDir.FullName, relDirPath);
    }
}
