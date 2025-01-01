using System.Collections.Concurrent;
using System.Text;

namespace AdventOfCode.Day11s2;

public class Day11S2
{
    private static ConcurrentDictionary<string, long> cache = new();
    public static void Run()
    {
        string line = File.ReadAllText("./Day11s2/input");
        List<long> stones = line.Split(' ').Select(long.Parse).ToList();

        var total = stones.AsParallel().Select(stone => CountStones(stone, 0)).Sum();
        Console.WriteLine(total);
    }

    private static long CountStones(long stone, long depth)
    {
        long result;
        if (depth == 75)
            result = 1;
        else if (cache.TryGetValue($"{depth}:{stone}", out result))
            return result;
        else if (stone == 0)
            result = CountStones(1, depth + 1);
        else if (("" + stone).Length % 2 == 0)
        {
            var parts = RuleTwo(stone);
            result = CountStones(parts[0], depth + 1);
            result += CountStones(parts[1], depth + 1);
        }
        else
        {
            result = CountStones(stone * 2024, depth + 1);
        }

        cache[$"{depth}:{stone}"] = result;

        return result;
    }

    private static List<long> RuleTwo(long x)
    {
        var s = "" + x;
        var left = s.Substring(0, s.Length / 2);
        var right = s.Substring(s.Length / 2);
        var li = long.Parse(left);
        var ri = long.Parse(right);
        return [li, ri];
    }
}