using System.Text;
using Codegen.IR.nodes;
using Codegen.Synthesizer;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using NUnit.Framework;

namespace Codegen.IR.Synthesizer.Tests.platform;

public abstract class AbstractCodegenTest
{
    private readonly ExpectedCodeProvider _expectedCodeProvider = new();

    protected bool UpdateTests = false;

    protected void Validate(CgFile cgFile)
    {
        var testName = TestContext.CurrentContext.Test.Name;
        var expected = _expectedCodeProvider.GetExpectedCodeForTest(testName)?.EnsureTrailingNewLine();
        Assert.That(expected, Is.Not.Null, $"can't find expected sources for test {testName}");

        var codegenSynthesizer = new CodegenSynthesizer();
        var actual = codegenSynthesizer.Synthesize(cgFile).EnsureTrailingNewLine();

        if (expected == actual)
        {
            return;
        }

        if (UpdateTests)
        {
            var physicalTestDataPath = "../../../testdata/expected/" + testName + ".csx";
            var file = File.Open(physicalTestDataPath, FileMode.Create);
            file.Write(Encoding.UTF8.GetBytes(actual));
            file.Close();

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
            }
        }

        Console.Error.WriteLine("Actual code:");
        Console.Error.WriteLine(actual);

        Assert.Fail();
    }
}
