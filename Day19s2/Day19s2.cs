using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day19s2;

public class Day19s2
{
    private static string[] towels;
    public static ConcurrentDictionary<string, long> cache = new();
    
    public static void Run()
    {
        var input = File.ReadAllLines("./Day19s2/input");
        towels = input[0].Split(", ");

        var toMake = input.Skip(2).ToList();
        
        var sum = toMake.AsParallel().Select(CountConstructible).Sum(amount => amount);
        
        Console.WriteLine(sum);
    }

    private static long CountConstructible(string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern)) return 1;
        
        if(cache.TryGetValue(pattern, out var isConstructible)) return isConstructible;
        
        var constructible = towels
            .Select(towel => pattern.StartsWith(towel) ? CountConstructible(pattern[towel.Length..]) : 0)
            .Sum();
        
        cache[pattern] = constructible;
        return constructible;
    }
}
