using Codegen.IR.nodes;
using Codegen.IR.nodes.types;

namespace Codegen.IR.Builder;

public static class CodegenIrBuilder
{
    public static CgFile CreateFile(string name)
    {
        return new CgFile(name);
    }

    public static CgMethod CreateMethod(this CgFile file, string name, Dictionary<string, ICgType> args, ICgType returnType)
    {
        var method = new CgMethod(name, args, returnType);
        file.Statements.Add(method);

        return method;
    }
}
