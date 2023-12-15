namespace Day15;

using System.Text.RegularExpressions;

internal record Lens(string Label, int Focus);
public class Solution
{
    private readonly IEnumerable<string> input;
    private Dictionary<int, List<Lens>> lensBoxes = new();

    public Solution(string input)
    {
        this.input = input.Split(',');
        foreach (var i in Enumerable.Range(0, 256))
        {
            lensBoxes.Add(i, new List<Lens>());
        }
    }

    public object PartOne()
    {
        return input.Sum(Encode);
    }

    public object PartTwo()
    {
        foreach (var command in input)
        {
            DoIt(command);
        }


        return Enumerable.Range(0, 256)
            .Select(it => new { ls = lensBoxes[it], i = it }).Sum(it => (it.i + 1) * it.ls
            .Select((l, i) => new { l, i })
            .Sum(indexedLens => (indexedLens.i + 1) * indexedLens.l.Focus));
    }

    private void DoIt(string command)
    {
        var f = Regex.Match(command, @"(?<label>\w*)(?<cmd>-|=\d+)");
        var label = f.Groups["label"].Value;
        var box = Encode(label);

        var l = lensBoxes[box].FindIndex(it => it.Label == label);
        if (f.Groups["cmd"].Value == "-")
        {
            if (l >= 0) lensBoxes[box].RemoveAt(l);
            return;
        }

        var focus = int.Parse(f.Groups["cmd"].Value[1..]);
        var lens = new Lens(label, focus);

        if (l >= 0) lensBoxes[box][l] = lens;
        else lensBoxes[box].Add(lens);
    }

    private static int Encode(string s)
    {
        return s.Aggregate(0, (i, c) => ((i + c) * 17) % 256);
    }
}