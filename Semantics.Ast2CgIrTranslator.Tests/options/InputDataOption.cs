using me.vldf.jsa.dsl.ir.builder.checkers;

namespace Semantics.Ast2CgIrTranslator.Tests.options;

public interface IInputDataOption
{
    public static IInputDataOption Parser(string name, IReadOnlyCollection<string> args)
    {
        return name switch
        {
            ExpectedTypeCheckErrors.Name => new ExpectedTypeCheckErrors(args.Select(Enum.Parse<ErrorCode>).ToList()),
            DumpIr.Name => new DumpIr(),
            _ => throw new InvalidOperationException($"unknown option {name}")
        };
    }
}

public record ExpectedTypeCheckErrors(IReadOnlyCollection<ErrorCode> Codes) : IInputDataOption
{
    public const string Name = "expected-tc-errors";
}

public record DumpIr : IInputDataOption
{
    public const string Name = "dump-ir";
}
