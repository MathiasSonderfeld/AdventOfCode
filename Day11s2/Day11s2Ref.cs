using System.Text;

namespace AdventOfCode.Day11s2;

public class Day11s2Ref
{
    
    public static void Run()
    {
        string line = File.ReadAllText("./Day11s1/input");
        var stones = line.Split(' ').Select(long.Parse).ToDictionary(s => s, s => 1L);

        for (int i = 0; i < 75; i++)
        {
            stones = Blink(stones);
        }
        Console.WriteLine(stones.Select(s => s.Value).Sum());
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

    private static Dictionary<long, long> Blink(Dictionary<long, long> start)
    {
        var result = new Dictionary<long, long>();

        foreach (var keyValuePair in start)
        {
            if(keyValuePair.Key == 0) result[1] = keyValuePair.Value + result.GetValueOrDefault(1, 0);
            else if (("" + keyValuePair.Key).Length % 2 == 0)
            {
                var newKeys = RuleTwo(keyValuePair.Key);
                result[newKeys[0]] = keyValuePair.Value + result.GetValueOrDefault(newKeys[0], 0);
                result[newKeys[1]] = keyValuePair.Value + result.GetValueOrDefault(newKeys[1], 0);
            }
            else
            {
                result[keyValuePair.Key * 2024] = keyValuePair.Value + result.GetValueOrDefault(keyValuePair.Key * 2024, 0);
            }
        }
        return result;
    }
}