using System.Text;
using Codegen.IR.nodes;
using Codegen.Synthesizer;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using NUnit.Framework;

namespace TestPlatform;

public abstract class BaseCodegenTest<T>
{
    protected readonly TestDataProvider<T> _testDataProvider = new();

    protected bool UpdateTests = false;

    protected void Validate(CgFile cgFile)
    {
        var testName = TestContext.CurrentContext.Test.Name;
        var expected = _testDataProvider.GetExpectedCodeForTest(testName)?.EnsureTrailingNewLine();

        var codegenSynthesizer = new CodegenSynthesizer();
        var actual = codegenSynthesizer.Synthesize(cgFile).EnsureTrailingNewLine();

        if (expected == null || UpdateTests)
        {
            UpdateExpectedFile(testName, actual);
            return;
        }

        if (expected == actual)
        {
            return;
        }

        Console.Error.WriteLine("Test failed!");
        var diffBuilder = new InlineDiffBuilder();
        var diff = diffBuilder.BuildDiffModel(expected, actual);

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

        Console.Error.WriteLine("Actual code:");
        Console.Error.WriteLine(actual);

        Assert.Fail();
    }

    private static void UpdateExpectedFile(string testName, string actual)
    {
        var physicalTestDataPath = "../../../testdata/expected/" + testName + ".jsadsl";
        var file = File.Open(physicalTestDataPath, FileMode.Create);
        file.Write(Encoding.UTF8.GetBytes(actual));
        file.Close();
    }
}
