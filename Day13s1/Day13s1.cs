using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day13S1;

public class Day13S1
{
    public static void Run()
    {
        var input = File.ReadAllText("./Day13S1/input");
        input = input.Replace("\r", "");
        
        var tokens = Regex.Matches(input, @"Button A: X\+(\d+), Y\+(\d+)\nButton B: X\+(\d+), Y\+(\d+)\nPrize: X=(\d+), Y=(\d+)")
            .Select(x => GetTokens(int.Parse(x.Groups[1].Value), int.Parse(x.Groups[2].Value), int.Parse(x.Groups[3].Value), int.Parse(x.Groups[4].Value), int.Parse(x.Groups[5].Value), int.Parse(x.Groups[6].Value)))
            .Sum();
        
        Console.WriteLine(tokens);
    }

    private static int GetTokens(int xA, int yA, int xB, int yB, int x, int y)
    {
        int a = (x * yB - y * xB) / (xA * yB - yA * xB);
        int b = (y - a * yA) / yB;

        if (a * xA + b * xB != x || a * yA + b * yB != y)
        {
            Console.WriteLine("Maschine nicht lösbar!");
            return 0;
        }

        return 3 * a + b;
    }
}