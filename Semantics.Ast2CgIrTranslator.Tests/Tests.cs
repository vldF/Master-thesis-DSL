using Ast.Builder.builder;
using Codegen.Synthesizer;
using NUnit.Framework;
using TestPlatform;

namespace Semantics.Ast2CgIrTranslator.Tests;

public class Tests : BaseE2ETest<Tests>
{
    [SetUp]
    public void Setup()
    {
        UpdateTests = false;
    }

    [Test]
    public void SimpleTest()
    {
        var input = GetInput();

        var astBuilder = new AstBuilder();
        var file = astBuilder.FromString(input);
        Console.WriteLine(file.String());

        var translator = new Ast2IrTranslator();
        var cgFile = translator.Translate(file);

        Validate(cgFile);
    }

    [Test]
    public void NewTest()
    {
        var input = GetInput();

        var astBuilder = new AstBuilder();
        var file = astBuilder.FromString(input);
        Console.WriteLine(file.String());

        var translator = new Ast2IrTranslator();
        var cgFile = translator.Translate(file);

        Validate(cgFile);
    }
}
