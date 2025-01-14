using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day19s1;

public class Day19s1
{
    private static string[] towels;
    
    public static void Run()
    {
        var input = File.ReadAllLines("./Day19s1/input");
        towels = input[0].Split(", ");

        var toMake = input.Skip(2).ToList();
        
        var sum = toMake.Select(IsConstructible).Count(valid => valid);
        
        Console.WriteLine(sum);
    }

    private static bool IsConstructible(string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern)) return true;
        
        return towels
            .Select(towel => pattern.StartsWith(towel) && IsConstructible(pattern[towel.Length..]))
            .Any(t => t);
    }
}
