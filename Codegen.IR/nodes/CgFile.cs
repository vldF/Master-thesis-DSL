namespace Codegen.IR.nodes;

public record CgFile(string Name) : ICgNode, ICgStatementsContainer
{
    public List<ICgStatement> Statements { get; } = [];
}
