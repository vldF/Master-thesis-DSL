namespace me.vldf.jsa.dsl.ir.nodes;

public interface IAstNode
{
    public string String();

    public bool IsSyntetic
    {
        get => false;
        set => throw new NotImplementedException();
    }
}
