using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.references;
using me.vldf.jsa.dsl.ir.builder.utils;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public class AbstractAstSemanticTransformer : AbstractAstTransformer
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected VarExpressionAstNode Interpretor;
    protected VarExpressionAstNode LocationArg;
    protected VarExpressionAstNode PythonSemantics;
    protected VarExpressionAstNode Engine;

    protected TypeReference IntTypeRef;

    protected IrContext CurrentContext;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static Dictionary<IrContext, Dictionary<string, int>> _freshVarsContext = new ();

    public override void Init(IrContext rootContext)
    {
        base.Init(rootContext);

        CurrentContext = rootContext;

        var intType = SimpleAstType.Int;
        CurrentContext.SaveNewType(intType);
        IntTypeRef = new TypeReference("int", CurrentContext);

        Interpretor = rootContext.GetFakeVariable("Interpreter");
        LocationArg = rootContext.GetFakeVariable("Location");
        PythonSemantics = rootContext.GetFakeVariable("PythonSemantics");
        Engine = rootContext.GetFakeVariable("Engine");
    }

    protected string GetFreshVar(string name)
    {
        if (!_freshVarsContext.TryGetValue(CurrentContext, out var context))
        {
            context = new Dictionary<string, int>();
            _freshVarsContext[CurrentContext] = context;
        }

        if (!context.TryGetValue(name, out var counter))
        {
            counter = 0;
        }

        context[name] = counter + 1;

        return name + counter;
    }

}
