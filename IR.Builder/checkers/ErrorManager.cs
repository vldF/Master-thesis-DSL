namespace me.vldf.jsa.dsl.ir.builder.checkers;

public class ErrorManager
{
    private readonly List<Error> _errorDescriptors = [];

    public void Report(Error error)
    {
        _errorDescriptors.Add(error);
    }

    public IReadOnlyCollection<Error> GetErrors() => _errorDescriptors;
}
