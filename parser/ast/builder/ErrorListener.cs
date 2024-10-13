using Antlr4.Runtime;

namespace me.vldf.jsa.dsl.parser.ast.builder;

public class ErrorListener<TS> : ConsoleErrorListener<TS>
{
    public bool HadError;

    public override void SyntaxError(TextWriter output, IRecognizer recognizer, TS offendingSymbol, int line,
        int col, string msg, RecognitionException e)
    {
        HadError = true;
        base.SyntaxError(output, recognizer, offendingSymbol, line, col, msg, e);
    }
}
