namespace Codegen.IR.nodes.expressions;

public record CgVarExpression(string Name, bool isOutVar = false) : ICgExpression
{
    public CgVarExpression AsNoOutVar()
    {
        return this with
        {
            isOutVar = false
        };
    }
}
