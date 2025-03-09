using me.vldf.jsa.dsl.ir.builder.utils;
using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public class AbstractAstSemanticTransformer : AbstractAstTransformer
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected VarExpressionAstNode Interpretor;
    protected VarExpressionAstNode LocationArg;
    protected VarExpressionAstNode SemanticsApi;
    protected VarExpressionAstNode Engine;

    protected TypeReference IntTypeRef;

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static Dictionary<string, int> _freshVarsContext = new ();

    public override void Init(IrContext rootContext)
    {
        base.Init(rootContext);

        IntTypeRef = new TypeReference("int", rootContext);

        Interpretor = rootContext.GetFakeVariable("Interpreter");
        LocationArg = rootContext.GetFakeVariable("location");
        SemanticsApi = rootContext.GetFakeVariable("SemanticsApi");
        Engine = rootContext.GetFakeVariable("Engine");
    }

    protected string GetFreshVar(string name)
    {
        var value = _freshVarsContext.GetValueOrDefault(name, 0);
        _freshVarsContext[name] = value + 1;

        return name + value;
    }

}
