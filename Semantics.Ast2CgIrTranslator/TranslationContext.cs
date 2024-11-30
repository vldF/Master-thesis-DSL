using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;
using Semantics.Ast2CgIrTranslator.Semantics;

namespace Semantics.Ast2CgIrTranslator;

public class TranslatorContext
{
    public CgFile File;
    public ICgExpression CurrentBuilder;
    public CgMethod HandlerMethod;
    public ICgStatementsContainer? CurrentContainer { get; private set; }

    public Semantics.Semantics Semantics = new();

    public Dictionary<string, ICgExpression> ClassDescriptorVariables = new();

    private Stack<ICgStatementsContainer> _containersStack = new ();
    public void PushContainer(ICgStatementsContainer container)
    {
        _containersStack.Push(container);
        CurrentContainer = container;
    }

    public void PopContainer()
    {
        _containersStack.Pop();
        _containersStack.TryPeek(out var result);
        CurrentContainer = result;
    }
}
