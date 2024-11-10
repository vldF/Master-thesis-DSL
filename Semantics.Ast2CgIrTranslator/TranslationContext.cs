using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;
using Semantics.Ast2CgIrTranslator.Semantics;

namespace Semantics.Ast2CgIrTranslator;

public class TranslatorContext
{
    public CgFile File;
    public ICgExpression CurrentBuilder;
    public CgMethod HandlerMethod;

    public Semantics.Semantics Semantics = new();

    public Dictionary<string, ICgExpression> ClassDescriptorVariables = new();
}
