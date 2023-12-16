namespace Day14;

class PlatformComparer : IEqualityComparer<List<string>>
{
    public bool Equals(List<string>? x, List<string>? y)
    {
        return y != null && (x?.SequenceEqual(y) ?? false);
    }

    public int GetHashCode(List<string> obj)
    {
        return obj.Aggregate(0, (current, t) => current ^ t.GetHashCode());
    }
}