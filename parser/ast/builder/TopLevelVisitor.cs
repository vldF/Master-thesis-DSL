using System.Diagnostics;
using me.vldf.jsa.dsl.ast.nodes;
using me.vldf.jsa.dsl.parser.ast.utils;

namespace me.vldf.jsa.dsl.parser.ast.builder;

public class TopLevelVisitor : JSADSLBaseVisitor<AstNode>
{
    public override FileAstNode VisitFile(JSADSLParser.FileContext context)
    {
        var result = context.topLevelDecl().Select(Visit).ToList();

        return new FileAstNode(result);
    }

    public override FunctionAstNode VisitFuncDecl(JSADSLParser.FuncDeclContext context)
    {
        var name = context.name.Text;
        var children = context.funcBody().children?.SelectNotNull(Visit).ToList()!;

        return new FunctionAstNode(name, children);
    }

    public override AstNode VisitObjectDecl(JSADSLParser.ObjectDeclContext context)
    {
        var name = context.name.Text;
        var children = context.objectBody().children?.SelectNotNull(Visit).ToList()!;

        return new ObjectAstNode(name, children);
    }
}
