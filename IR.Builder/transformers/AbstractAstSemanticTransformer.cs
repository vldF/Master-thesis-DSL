using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.references;
using me.vldf.jsa.dsl.ir.builder.utils;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public class AbstractAstSemanticTransformer : AbstractAstTransformer
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected VarExpressionAstNode InterpretorReciever;
    protected VarExpressionAstNode LocationArg;
    protected VarExpressionAstNode PythonSemantics;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public override void Init(IrContext rootContext)
    {
        base.Init(rootContext);

        InterpretorReciever = rootContext.GetFakeVariable("Interpreter");
        LocationArg = rootContext.GetFakeVariable("Location");
        PythonSemantics = rootContext.GetFakeVariable("PythonSemantics");
    }

}
