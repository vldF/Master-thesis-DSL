using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;

namespace Semantics.Ast2CgIrTranslator.Semantics;

public class Semantics
{
    public CgVarExpression Types = new("PythonTypes");
    public CgVarExpression SemanticsApi = new("SemanticsApi");
    public CgVarExpression InterpreterApi = new("Interpreter");
    public CgVarExpression ProcessorApi = new("ProcessorApi");
    public CgVarExpression TaintOrigin = new("TaintOrigin");
    public CgVarExpression ModuleDescriptorVar = new("ModuleDescriptor");
    public CgVarExpression Log = new("Log");

    private CgVarExpression _location = new("Location");
    private CgVarExpression _functionCall = new("functionCall");

    public ICgExpression CreateClass(string name)
    {
        return Types.CallMethod("CreateClass", [AsExpression(name)]);
    }

    public ICgExpression CreateInstance(
        CgVarExpression classDescriptor,
        IEnumerable<ICgExpression> args)
    {
        List<ICgExpression> callArgs =
        [
            _location.Property("Empty"),
            classDescriptor,
            new CgListLiteralExpression(args.ToList(), "ImmutableArray<SymbolicExpression>")
        ];

        return Types.CallMethod("CreateInstance", callArgs);
    }

    public ICgExpression CreateNonTypedInstance(
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

    public ICgExpression GetSelf()
    {
        return new CgVarExpression("self");
    }
}
