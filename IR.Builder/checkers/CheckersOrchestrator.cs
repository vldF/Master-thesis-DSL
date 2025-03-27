using me.vldf.jsa.dsl.ir.nodes.declarations;

namespace me.vldf.jsa.dsl.ir.builder.checkers;

public class CheckersOrchestrator
{
    private readonly ErrorManager _errorManager = new();

    private readonly List<AbstractCheckerBase> _checkers;

    public CheckersOrchestrator()
    {
        _checkers = [
            new TypeChecker(_errorManager),
        ];
    }

    public IReadOnlyCollection<Error> Check(FileAstNode file)
    {
        foreach (var checker in _checkers)
        {
            checker.Check(file);
        }

        return _errorManager.GetErrors();
    }
}
