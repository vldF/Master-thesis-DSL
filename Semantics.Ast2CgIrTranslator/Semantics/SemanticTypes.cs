using Codegen.IR.Builder;
using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;

namespace Semantics.Ast2CgIrTranslator.Semantics;

public class SemanticTypes
{
    public CgVarExpression Types = new("PythonTypes");
    public CgVarExpression SemanticsApi = new("SemanticsApi");

    public ICgExpression CreateClass(string name)
    {
        return Types.CallMethod("CreateClass", [AsExpression(name)]);
    }

    public CgMethodCall Return(ICgExpression? expression = null)
    {
        var res = expression ?? SemanticsApi.Property("None");

        return new CgVarExpression("CallHandlerResult").CallMethod("Processed", [res]);
    }
}
