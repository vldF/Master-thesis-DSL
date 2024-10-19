namespace Codegen.IR.nodes;


public class CgStatementsContainer(List<ICgStatement>? statements = null) : ICgStatementsContainer
{
    public List<ICgStatement> Statements { get; } = statements ?? [];
}
