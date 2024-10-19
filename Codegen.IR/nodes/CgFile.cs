namespace Codegen.IR.nodes;

public record CgFile(string Name) : ICgStatementsContainer, ICgStatement
{
    public List<ICgStatement> Statements { get; } = [];
}
