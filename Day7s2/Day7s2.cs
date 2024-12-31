namespace AdventOfCode.Day7s2;

public class Day7S2
{
    public static void Run()
    {
        string[] lines = File.ReadAllLines("./Day7s2/input");
        
        var sum = lines.Select(line =>
        {
            var split = line.Split(": ");
            var result = long.Parse(split[0]);
            var numbers = split[1].Split(" ").Select(long.Parse).ToList();
            
            if (IsReachable(result, numbers[0], numbers.Skip(1).ToList()))
            {
                Console.WriteLine($"{result} is reachable with: {line}");
                return result;
            }
            return 0;
        }).Sum();
        
        Console.WriteLine(sum);
    }

    private static bool IsReachable(long result, long startValue, List<long> numbers)
    {
        if(startValue == result && numbers.Count == 0) return true;
        if(startValue > result) return false;
        if(numbers.Count == 0) return false;
        
        var plusValue = startValue + numbers[0];
        var multValue = startValue * numbers[0];
        var concatValue = long.Parse(startValue + "" + numbers[0]);
        
        var remainingNumbers = numbers.Skip(1).ToList();
        return IsReachable(result, plusValue, remainingNumbers) || IsReachable(result, multValue, remainingNumbers) || IsReachable(result, concatValue, remainingNumbers);
    }
}