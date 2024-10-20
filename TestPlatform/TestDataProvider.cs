using System.Reflection;

namespace TestPlatform;

public class TestDataProvider<T>
{
    private const string BasePath = "testdata/";
    private const string ExpectedDirPath = BasePath + "expected/";
    private const string InputDirPath = BasePath + "input/";

    public string? GetExpectedCodeForTest(string testName)
    {
        var testFilePath = ExpectedDirPath + testName + ".jsa";
        var testFileAsStream = GetResourceStream(testFilePath);
        if (testFileAsStream == null)
        {
            return null;
        }

        var reader = new StreamReader(testFileAsStream);
        return reader.ReadToEnd();
    }

    public string? GetInputCodeForTest(string testName)
    {
        var testFilePath = InputDirPath + testName + ".jsadsl";
        var testFileAsStream = GetResourceStream(testFilePath);
        if (testFileAsStream == null)
        {
            return null;
        }

        var reader = new StreamReader(testFileAsStream);
        return reader.ReadToEnd();
    }

    private static Stream? GetResourceStream(string resName)
    {
        var assembly = Assembly.GetAssembly(typeof(T));
        var allResources = assembly.GetManifestResourceNames();
        var convertedResourceName = resName.Replace("/", ".");

        var resourceName = allResources.FirstOrDefault(res => res.EndsWith(convertedResourceName));
        if (resourceName == null)
        {
            return null;
        }

        return assembly.GetManifestResourceStream(resourceName);
    }
}
