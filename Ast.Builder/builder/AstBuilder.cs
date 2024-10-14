using Antlr4.Runtime;
using me.vldf.jsa.dsl.ast.nodes;
using me.vldf.jsa.dsl.parser;

namespace Ast.Builder.builder;

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

        var fileContext = parser.file();
        var topLevelVisitor = new TopLevelVisitor();
        var fileAstNode = topLevelVisitor.VisitFile(fileContext);

        if (lexerErrorListener.HadError || parserErrorListener.HadError)
        {
            throw new Exception("error while lexing occurred!");
        }

        return fileAstNode;
    }
}
