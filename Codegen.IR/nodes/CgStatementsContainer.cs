namespace Codegen.IR.nodes;


public class CgStatementsContainer : ICgStatementsContainer
{
    public List<ICgStatement> Statements { get; }

    public CgStatementsContainer(IReadOnlyCollection<ICgStatement>? statements = null)
    {
    }
}
