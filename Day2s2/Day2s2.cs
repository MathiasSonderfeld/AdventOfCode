namespace AdventOfCode.Day1s1;

public class Day2s1
{
  public static void Run()
  {
    string inputFile = File.ReadAllText("../../../Day2s1/input");
    string[] lines = inputFile.Split("\r\n");
    int total = 0;

    foreach (var line in lines)
    {
      string[] nums = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
      var l = nums.ToList().ConvertAll(int.Parse);
      var increasing = IsConsistent(l, true);
      var decreasing = IsConsistent(l, false);
      var isInMargin = IsInMargin(l);
      if (isInMargin && (increasing || decreasing))
      {
        total++;
      }
    }
    Console.WriteLine(total);
  }

  private static bool IsConsistent(List<int> numbers, bool increasing)
  {
    var isConsistent = true;
    for (var i = 0; isConsistent && i < numbers.Count-1; i++)
    {
      if (increasing)
      {
        isConsistent = numbers[i] < numbers[i + 1];
      }
      else
      {
        isConsistent = numbers[i] > numbers[i + 1];
      }
    }
    return isConsistent;
  }

  private static bool IsInMargin(List<int> numbers)
  {
    var inMargin = true;
    for (var i = 0; inMargin && i < numbers.Count - 1; i++)
    {
      var diff = Math.Abs(numbers[i] - numbers[i + 1]);
      inMargin = diff is > 0 and < 4;
    }
    return inMargin;
  }
}