using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day19s1;

public class Day19s1
{
    private static string[] towels;
    public static ConcurrentDictionary<string, bool> cache = new();
    
    public static void Run()
    {
        var input = File.ReadAllLines("./Day19s1/input");
        towels = input[0].Split(", ");

        var toMake = input.Skip(2).ToList();
        
        var sum = toMake.AsParallel().Select(IsConstructible).Count(valid => valid);
        
        Console.WriteLine(sum);
    }

    private static bool IsConstructible(string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern)) return true;
        
        if(cache.TryGetValue(pattern, out var isConstructible)) return isConstructible;
        
        var constructible = towels
            .Select(towel => pattern.StartsWith(towel) && IsConstructible(pattern[towel.Length..]))
            .Any(t => t);
        
        cache[pattern] = constructible;
        return constructible;
    }
}
