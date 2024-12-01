namespace TestPlatform;

public class TestDirectoriesUtils
{
    public static string GetCurrentProjectTestdataPath()
    {
        var projectDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
        var result = Path.Combine(projectDir, "testdata");

        Directory.CreateDirectory(result);

        return result;
    }

    public static string GetCurrentProjectTestTempPath()
    {
        var projectDir = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
        var result = Path.Combine(projectDir, "testtemp");

        Directory.CreateDirectory(result);

        return result;
    }
}
