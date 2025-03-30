using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;

namespace Semantics.Ast2CgIrTranslator;

public class TranslatorContext
{
    public CgFile File;
    public ICgExpression CurrentClassDescriptor;
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

    private readonly Dictionary<string, int> _freshVarIndexes = new();

    public string GetFreshVarName(string name)
    {
        var newIdx = _freshVarIndexes.GetValueOrDefault(name, 0) + 1;
        _freshVarIndexes[name] = newIdx;

        return name + newIdx;
    }
}
