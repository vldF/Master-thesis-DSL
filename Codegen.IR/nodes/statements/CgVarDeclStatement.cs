using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.types;

namespace Codegen.IR.nodes.statements;

public record CgVarDeclStatement(string Name, ICgType? Type, ICgExpression? Init = null) : ICgStatement
{
    public ICgExpression AsValue()
    {
        return new CgVarExpression(Name);
    }
}
