using Day19;

var input = File.ReadAllLines("sample_input.txt").ToList();
var solution = new Solution(input);

Console.WriteLine($"Part one: {solution.PartOne()}");
Console.WriteLine($"Part two: {solution.PartTwo()}");