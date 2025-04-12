using me.vldf.jsa.dsl.ir.builder.checkers.python;
using me.vldf.jsa.dsl.ir.nodes.declarations;

namespace me.vldf.jsa.dsl.ir.builder.checkers;

public class CheckersOrchestrator
{
    public IReadOnlyCollection<Error> Check(FileAstNode file)
    {
        var errorManager = new ErrorManager();
        List<AbstractCheckerBase> checkers =
        [
            new TypeChecker(errorManager),
            new InitMethodChecker(errorManager),
        ];

        foreach (var checker in checkers)
        {
            checker.Check(file);
        }

        return errorManager.GetErrors();
    }
}
