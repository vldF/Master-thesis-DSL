using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.types;

namespace Codegen.IR.nodes;

public class CgMethod : ICgStatement, ICgStatementsContainer
{
    public string Name { get; }
    public ICgType ReturnType { get; }

    private readonly Dictionary<string, ICgExpression> _args = new();
    public Dictionary<string, ICgType> ArgTypes;
    public ICollection<CgAnnotation> Annotations { get; }

    public CgMethod(string name, Dictionary<string, ICgType> args, ICgType returnType, ICollection<CgAnnotation> annotations)
    {
        Name = name;
        ArgTypes = args;
        ReturnType = returnType;
        Annotations = annotations;

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
