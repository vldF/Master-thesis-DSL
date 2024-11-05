using Codegen.IR.Builder;
using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;

namespace Semantics.Ast2CgIrTranslator.Semantics;

public class SemanticTypes
{
    public CgVarExpression Types = new("PythonTypes");
    public CgVarExpression SemanticsApi = new("SemanticsApi");

    private CgVarExpression _location = new("Location");

    public ICgExpression CreateClass(string name)
    {
        return Types.CallMethod("CreateClass", [AsExpression(name)]);
    }

    public ICgExpression CreateInstance(
        string name,
        ICgExpression descriptor,
        IEnumerable<ICgExpression> args)
    {
        var customName = $"<{name}>";
        List<ICgExpression> callArgs = [_location, descriptor, AsExpression(customName)];
        callArgs.AddRange(args);

        return SemanticsApi.CallMethod("CreateObjectDescriptor", callArgs);
    }

    public CgMethodCall Return(ICgExpression? expression = null)
    {
        var res = expression ?? SemanticsApi.Property("None");

        return new CgVarExpression("CallHandlerResult").CallMethod("Processed", [res]);
    }
}
