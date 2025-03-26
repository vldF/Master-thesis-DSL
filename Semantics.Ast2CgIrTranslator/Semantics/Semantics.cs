using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;

namespace Semantics.Ast2CgIrTranslator.Semantics;

public class Semantics
{
    public CgVarExpression Types = new("PythonTypes");
    public CgVarExpression SemanticsApi = new("SemanticsApi");
    public CgVarExpression InterpreterApi = new("Interpreter");
    public CgVarExpression TaintOrigin = new("TaintOrigin");

    private CgVarExpression _location = new("Location");
    private CgVarExpression _functionCall = new("functionCall");

    public ICgExpression CreateClass(string name)
    {
        return Types.CallMethod("CreateClass", [AsExpression(name)]);
    }

    public ICgExpression CreateInstance(
        string name,
        IEnumerable<ICgExpression> args)
    {
        var customName = $"<{name}>";
        List<ICgExpression> callArgs = [AsExpression(customName)];
        callArgs.AddRange(args);

        return SemanticsApi.CallMethod("CreateObjectDescriptor", callArgs);
    }

    public CgMethodCall Return(ICgExpression? expression = null)
    {
        var res = expression ?? SemanticsApi.Property("None");

        return new CgVarExpression("CallHandlerResult").CallMethod("Processed", [res]);
    }

    public ICgExpression GetArgument(int idx)
    {
        return _functionCall.Property("Arguments").Index(idx);
    }

    public ICgExpression GetSelf()
    {
        return new CgVarExpression("self");
    }
}
