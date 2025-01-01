using System.Text;

namespace AdventOfCode.Day11s2;

public class Day11S2
{
    
    public static void Run()
    {
        string line = File.ReadAllText("./Day11s2/input");
        List<long> stones = line.Split(' ').Select(long.Parse).ToList();

        var total = stones.AsParallel().Select(stone => CountStones(stone, 0)).Sum();
        Console.WriteLine(total);
    }

    private static long CountStones(long value, int depth)
    {
        if(depth == 50) return 1;

        if (value == 0) return CountStones(1, depth + 1);

        if (("" + value).Length % 2 == 0)
        {
             var l = RuleTwo(value);
             return CountStones(l[0], depth + 1) + CountStones(l[1], depth + 1);
        }
        return CountStones(value * 2024, depth + 1);
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