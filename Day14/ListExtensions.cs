namespace Day14;

using System.Text;

internal static class ListExtensions
{
    internal static IEnumerable<string> Cycle(this IEnumerable<string> platform)
    {
        return platform
            .TiltNorth()
            .TiltWest()
            .TiltSouth()
            .TiltEast();
    }
    internal static IEnumerable<string> TiltNorth(this IEnumerable<string> platform)
    {
        var p = platform.ToList();
        var columns = new List<string>();
        for (int col = 0; col < p[0].Length; col++)
        {
            var column = new StringBuilder();
            foreach (var row in p)
            {
                column.Append(row[col]);
            }

            columns.Add(ProcessLine(column.ToString()));
        }

        for (int i = 0; i < p[0].Length; i++)
        {
            yield return string.Join("", columns.Select(s => s[i]));
        }
    }

    internal static IEnumerable<string> TiltEast(this IEnumerable<string> platform)
    {
        return platform
            .Select(it => string.Join("", it.ToCharArray().Reverse()))
            .Select(ProcessLine)
            .Select(it => string.Join("", it.ToCharArray().Reverse()));
    }

    internal static IEnumerable<string> TiltWest(this IEnumerable<string> platform)
    {
        return platform
            .Select(ProcessLine);
    }

    internal static IEnumerable<string> TiltSouth(this IEnumerable<String> platform)
    {
        var p = platform.ToList();
        p.Reverse();
        var columns = new List<string>();
        for (int col = 0; col < p[0].Length; col++)
        {
            var column = new StringBuilder();

            foreach (var row in p)
            {
                column.Append(row[col]);
            }

            columns.Add(ProcessLine(column.ToString()));
        }

        for (int i = p[0].Length - 1; i >= 0; i--)
        {
            yield return string.Join("", columns.Select(s => s[i]));
        }
    }

    private static string ProcessLine(string foo)
    {
        return string.Join("#", foo.Split("#")
            .Select(ProcessLinePart));
    }

    private static string ProcessLinePart(string s)
    {
        var rockCount = s.Count(c => c == 'O');
        var rocks = Enumerable.Repeat('O', rockCount);
        var spaces = Enumerable.Repeat('.', s.Length - rockCount);
        return string.Join("", rocks.Concat(spaces));
    }
}