using System.Text;

namespace AdventOfCode.Day11s1;

public class Day11S1
{
    
    public static void Run()
    {
        string line = File.ReadAllText("./Day11s1/input");
        List<long> stones = line.Split(' ').Select(long.Parse).ToList();

        for (int i = 0; i < 25; i++)
        {
            stones = stones.SelectMany(stone =>
            {
                if (stone == 0) return [1];
                if(("" + stone).Length % 2 == 0) return RuleTwo(stone);
                return [stone * 2024];
            }).ToList();
        }
        Console.WriteLine(stones.Count);
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