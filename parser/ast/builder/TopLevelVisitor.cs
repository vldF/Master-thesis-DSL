using me.vldf.jsa.dsl.ast.nodes;

namespace me.vldf.jsa.dsl.parser.ast.builder;

public class TopLevelVisitor : JSADSLBaseVisitor<AstNode>
{
    public override FileAstNode VisitFile(JSADSLParser.FileContext context)
    {
        var result = context.funcDecl().Select(VisitFuncDecl).ToList();

        return new FileAstNode(result);
    }

    public override FunctionAstNode VisitFuncDecl(JSADSLParser.FuncDeclContext context)
    {
        var name = context.name.Text;
        return new FunctionAstNode(name);
    }
}
