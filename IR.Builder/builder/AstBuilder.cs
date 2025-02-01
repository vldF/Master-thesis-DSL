using Antlr4.Runtime;
using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.builder.transformers;
using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.parser;

namespace me.vldf.jsa.dsl.ir.builder.builder;

public class AstBuilder
{
    public IReadOnlyCollection<FileAstNode> FromStrings(IReadOnlyCollection<(string name, string code)> codes)
    {
        var rootContext = new IrContext(null);
        rootContext.SaveNewType(SimpleAstType.Any);
        rootContext.SaveNewType(SimpleAstType.Int);
        rootContext.SaveNewType(SimpleAstType.StringT);

        var transOrhestrator = new TransformersOrchestrator(rootContext);

        var parseResult = codes
            .Select(c => Parse(c, rootContext))
            .ToList();

        var allContexts = parseResult
            .Select(c => c.fileIrContext)
            .ToList();

        foreach (var fileIrContext in allContexts)
        {
            fileIrContext.InitializeImports(allContexts);
        }

        return parseResult
            .Select(f => f.fileAstNode)
            .Select(f => (FileAstNode)transOrhestrator.Transform(f))
            .ToList();
    }

    private (FileAstNode fileAstNode, IrContext fileIrContext) Parse((string name, string code) file, IrContext rootContext)
    {
        var fileIrContext = new IrContext(rootContext);
        var topLevelVisitor = new BaseBuilderVisitor(fileIrContext);

        var inputStream = new AntlrInputStream(file.code);
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
        var fileAstNode = topLevelVisitor.VisitFile(fileContext);
        fileAstNode.FileName = file.name;

        fileIrContext.Package = fileAstNode.Package;

        if (lexerErrorListener.HadError || parserErrorListener.HadError)
        {
            throw new Exception("error while lexing occurred!");
        }

        return (fileAstNode, fileIrContext);
    }
}
