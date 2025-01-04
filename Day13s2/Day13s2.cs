using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day13S2;

public class Day13S2
{
    public static void Run()
    {
        var input = File.ReadAllText("./Day13S1/input");
        input = input.Replace("\r", "");
        
        var tokens = Regex.Matches(input, @"Button A: X\+(\d+), Y\+(\d+)\nButton B: X\+(\d+), Y\+(\d+)\nPrize: X=(\d+), Y=(\d+)")
            .Select(x => GetTokens(long.Parse(x.Groups[1].Value), long.Parse(x.Groups[2].Value), long.Parse(x.Groups[3].Value), long.Parse(x.Groups[4].Value), long.Parse(x.Groups[5].Value) + 10000000000000, long.Parse(x.Groups[6].Value) + 10000000000000))
            .Sum();
        
        Console.WriteLine(tokens);
    }

    private static long GetTokens(long xA, long yA, long xB, long yB, long x, long y)
    {
        long a = (x * yB - y * xB) / (xA * yB - yA * xB);
        long b = (y - a * yA) / yB;

        if (a * xA + b * xB != x || a * yA + b * yB != y)
        {
            Console.WriteLine("Maschine nicht lösbar!");
            return 0;
        }

        return 3 * a + b;
    }
}