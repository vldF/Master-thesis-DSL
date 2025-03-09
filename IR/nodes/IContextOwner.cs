using me.vldf.jsa.dsl.ir.context;

namespace me.vldf.jsa.dsl.ir.nodes;

public interface IContextOwner
{
    IrContext Context { get; set; }
}
