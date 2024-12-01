using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace TestPlatform;

public static class DiffUtils
{
    public static void PrintDiffForFiles(string expectedPath, string actualPath)
    {
        var expectedCode = File.ReadAllText(expectedPath);
        var actualCode = File.ReadAllText(actualPath);

        PrintDiffForCode(expectedCode, actualCode);

        // integration with Rider, it allows to compare files with the build-in diff tool
        var host = Environment.GetEnvironmentVariable("RESHARPER_HOST");
        if (host == "Rider")
        {
            Console.WriteLine($"Compare(Rider): \"file:///{actualPath}\",\"file:///{expectedPath}\"");
        }
    }

    private static void PrintDiffForCode(string expectedCode, string actualCode)
    {
        var diffBuilder = new InlineDiffBuilder();
        var diff = diffBuilder.BuildDiffModel(expectedCode, actualCode);

        foreach (var diffPice in diff.Lines)
        {
            switch (diffPice.Type)
            {
                case ChangeType.Deleted:
                    Console.Error.WriteLine("-{0}", diffPice.Text);
                    break;
                case ChangeType.Inserted:
                    Console.Error.WriteLine("+{0}", diffPice.Text);
                    break;
                case ChangeType.Modified:
                    Console.Error.WriteLine("~{0}", diffPice.Text);
                    break;
                case ChangeType.Unchanged:
                    Console.Error.WriteLine(" {0}", diffPice.Text);
                    break;
            }
        }
    }
}
