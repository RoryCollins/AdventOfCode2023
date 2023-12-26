using Day18;

var input = File.ReadAllLines("input.txt").ToList();
// var input = File.ReadAllLines("input.txt").ToList();
var solution = new Solution(input);

Console.WriteLine($"Part one: {solution.PartOne()}");
Console.WriteLine($"Part two: {solution.PartTwo()}");