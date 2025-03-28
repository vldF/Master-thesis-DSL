namespace me.vldf.jsa.dsl.ir.nodes.statements;

public class ImportAstNode(
    string fileName) : IStatementAstNode
{
    public string FileName { get; } = fileName;

    public string String()
    {
        return $"import \"{FileName}\"";
    }

    public IAstNode? Parent { get; set; }
}
