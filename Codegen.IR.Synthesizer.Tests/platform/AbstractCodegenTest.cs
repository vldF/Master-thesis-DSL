using Codegen.IR.nodes;
using Codegen.Synthesizer;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using NUnit.Framework;

namespace Codegen.IR.Synthesizer.Tests.platform;

public abstract class AbstractCodegenTest
{
    private readonly ExpectedCodeProvider _expectedCodeProvider = new();

    protected void Validate(CgFile cgFile, string testName)
    {
        var expected = _expectedCodeProvider.GetExpectedCodeForTest(testName)?.EnsureTrailingNewLine();
        Assert.That(expected, Is.Not.Null, $"can't find expected sources for test {testName}");

        var codegenSynthesizer = new CodegenSynthesizer();
        var actual = codegenSynthesizer.Synthesize(cgFile).EnsureTrailingNewLine();

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
            }
        }

        Console.Error.WriteLine("Actual code:");
        Console.Error.WriteLine(actual);

        Assert.Fail();
    }
}
