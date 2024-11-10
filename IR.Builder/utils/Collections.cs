namespace me.vldf.jsa.dsl.ir.builder.utils;

public static class Collections
{
    public static IEnumerable<TR> SelectNotNull<T, TR>(this IEnumerable<T?> sequence, Func<T?, TR?> func)
    {
        return sequence.Select(func).Where(x => x != null)!;
    }
}
