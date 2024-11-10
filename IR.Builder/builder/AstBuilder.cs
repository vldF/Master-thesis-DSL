using Antlr4.Runtime;
using me.vldf.jsa.dsl.ir.builder.transformers;
using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.parser;

namespace me.vldf.jsa.dsl.ir.builder.builder;

public class AstBuilder
{
    public FileAstNode FromString(string str)
    {
        var inputStream = new AntlrInputStream(str);
        var lexer = new JSADSLLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new JSADSLParser(tokens);

        var lexerErrorListener = new ErrorListener<int>();
        var parserErrorListener = new ErrorListener<IToken>();
        lexer.RemoveErrorListeners();
        parser.RemoveErrorListeners();

        lexer.AddErrorListener(lexerErrorListener);
        parser.AddErrorListener(parserErrorListener);

        var rootContext = new IrContext(null);

        var fileContext = parser.file();
        var topLevelVisitor = new BaseBuilderVisitor(rootContext);
        var fileAstNode = topLevelVisitor.VisitFile(fileContext);

        if (lexerErrorListener.HadError || parserErrorListener.HadError)
        {
            throw new Exception("error while lexing occurred!");
        }

        var transOrhestrator = new TransformersOrchestrator();
        return (FileAstNode)transOrhestrator.Transform(fileAstNode);
    }
}
