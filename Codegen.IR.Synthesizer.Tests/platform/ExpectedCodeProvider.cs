using System.Reflection;

namespace Codegen.IR.Synthesizer.Tests.platform;

public class ExpectedCodeProvider
{
    private const string BasePath = "testdata/expected/";

    public string? GetExpectedCodeForTest(string testName)
    {
        var testFilePath = BasePath + testName + ".csx";
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
        var assembly = Assembly.GetExecutingAssembly();
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
