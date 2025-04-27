using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.types;

namespace Codegen.IR.nodes;

public record CgMethodCall(
    ICgExpression? Reciever,
    string Name,
    IReadOnlyCollection<ICgExpression> Args,
    IReadOnlyCollection<ICgType>? Generics = null) : ICgStatement, ICgExpression;
