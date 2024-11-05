using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;
using me.vldf.jsa.dsl.ast.nodes.declarations;
using Semantics.Ast2CgIrTranslator.Semantics;

namespace Semantics.Ast2CgIrTranslator;

public class TranslatorContext
{
    public CgFile File;
    public ICgExpression CurrentBuilder;
    public CgMethod HandlerMethod;

    public SemanticTypes SemanticTypes = new();
}
