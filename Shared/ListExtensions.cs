namespace Shared;

public static class ListExtensions
{
    public static IEnumerable<T[]> Permute<T>(this IEnumerable<T> source)
    {
        return Permute(new List<T>(), source.ToArray());
    }

    public static IEnumerable<T[]> Windowed<T>(this IEnumerable<T> source, int size, int step = 1)
    {
        var array = source.ToArray();
        var i = 0;
        while(i + size <= array.Length)
        {
            yield return array.Skip(i + step - 1)
                .Take(size)
                .ToArray();
            i += step;
        }
    }

    private static IEnumerable<T[]> Permute<T>(IEnumerable<T> prefix, T[] remainder)
    {
        if (!remainder.Any()) return new[]{prefix.ToArray()};
        return remainder.SelectMany((s, i) => Permute(
            prefix.Append(s),
            remainder.Take(i).Concat(remainder.Skip(i + 1)).ToArray()));
    }
}