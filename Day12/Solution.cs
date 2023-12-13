namespace Day12;

using System.Text.RegularExpressions;
using Shared;

public class Solution
{
    private readonly IEnumerable<(string Pattern, int[] Definition)> input;
    private IEnumerable<(string Pattern, int[] Definition)> unfoldedInput;

    public Solution(IEnumerable<string> input)
    {
        this.input = input
            .Select(it => it.Split(" "))
            .Select(it => (
                Pattern: it[0],
                Definition: it[1].Split(",").Select(int.Parse).ToArray()));
    }

    public object PartOne()
    {
        var parsedInput = input.Select(it => (
            Pattern: Regex.Split(it.Pattern.Trim('.'), @"\.+"),
            it.Definition
        ));
        var sum = 0;
        foreach (var (pattern, definition) in parsedInput)
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("------------------------");
            var parsedPattern = pattern;
            var parsedDefinition = definition;
            Console.WriteLine(string.Join("|", pattern));
            Console.WriteLine(string.Join("|", definition));
            if (pattern.Length == definition.Length)
            {
                sum += pattern.Zip(definition)
                    .Aggregate(1, (acc, it) => acc * CountPossibilities(it.First, it.Second));
                continue;
            }

            //skip already solved chunks
            while (parsedPattern.First().Length == parsedDefinition.First())
            {
                parsedPattern = parsedPattern.Skip(1).ToArray();
                parsedDefinition = parsedDefinition.Skip(1).ToArray();
            }
            while (parsedPattern.Last().Length == parsedDefinition.Last())
            {
                parsedPattern = parsedPattern.SkipLast(1).ToArray();
                parsedDefinition = parsedDefinition.SkipLast(1).ToArray();
            }



            Console.WriteLine("------------------------");

            Console.WriteLine("Unsolved piece:");
            Console.WriteLine(string.Join("|", parsedPattern));
            Console.WriteLine(string.Join("|", parsedDefinition));


            sum += parsedPattern.Zip(parsedDefinition)
                .Aggregate(1, (acc, x) => acc * AllPossibilities(x.First, new[] { x.Second }, 0));
        }

        return sum;
    }

    public object PartTwo()
    {
        unfoldedInput = input.Select(it => (
            Pattern: string.Join("?", Enumerable.Repeat(it.Pattern, 5)),
            Definition: Enumerable.Repeat(it.Definition, 5).SelectMany(xs => xs).ToArray()
        ));

        return unfoldedInput.First()
            .Pattern;
    }

    private bool isValid(string pattern, int[] definition)
    {
        var patternDefinition = new List<int>();
        var current = 0;
        foreach(var c in pattern)
        {
            switch (c)
            {
                case '#':
                    current++;
                    break;
                case '.' when current > 0:
                    patternDefinition.Add(current);
                    current = 0;
                    break;
            }
        }
        if(current>0){patternDefinition.Add(current);}

        return patternDefinition.SequenceEqual(definition);
    }

    private int AllPossibilities(string s, int[] definition, int count)
    {
        var f = s.IndexOf('?');
        if (f == -1) return isValid(s, definition) ? 1 : 0;
        return new[] { "#", "." }
            .Select(it => s[..f] + it + s[(f + 1)..])
            .Sum(it => AllPossibilities(it, definition, count));
    }

    private int CountPossibilities(string pattern, int definition)
    {

        if (pattern.Contains('#'))
        {
            var startIndex = Math.Max((pattern.LastIndexOf('#') + 1) - definition, 0);
            var lastIndex = Math.Min(pattern.IndexOf('#') + definition, pattern.Length);
            var s = pattern[startIndex..lastIndex];
            return s
                .Windowed(definition)
                .Count();
        }
        return pattern.Windowed(definition).Count();
    }
}