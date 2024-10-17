using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.types;

namespace Codegen.IR.nodes;

public class CgMethod : ICgStatement, ICgStatementsContainer
{
    public string Name { get; }
    public ICgType ReturnType { get; }

    private readonly Dictionary<string, ICgExpression> _args = new();
    private Dictionary<string, ICgType> _argTypes;

    public CgMethod(string name, Dictionary<string, ICgType> args, ICgType returnType)
    {
        Name = name;
        _argTypes = args;
        ReturnType = returnType;

        foreach (var (argName, _) in args)
        {
            _args[argName] = new CgVarExpression(argName);
        }
    }

    public ICgExpression GetArgument(string name)
    {
        return _args[name];
    }

    public List<ICgStatement> Statements { get; } = [];
}
