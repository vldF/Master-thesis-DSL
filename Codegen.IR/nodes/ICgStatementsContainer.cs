namespace Codegen.IR.nodes;

public interface ICgStatementsContainer : ICgNode
{
    public List<ICgStatement> Statements { get; }

    public void Add(ICgStatement statement)
    {
        Statements.Add(statement);
    }
}
