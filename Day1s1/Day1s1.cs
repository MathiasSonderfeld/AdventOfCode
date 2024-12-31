namespace AdventOfCode.Day1s1;

public class Day1s1
{
  public static void Run()
  {
    string inputFile = File.ReadAllText("../../../Day1s1/input");
    string[] lines = inputFile.Split("\r\n");
    List<int> left = new List<int>();
    List<int> right = new List<int>();
    foreach (var line in lines)
    {
      string[] nums = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
      int a = int.Parse(nums[0]);
      int b = int.Parse(nums[1]);
      left.Add(a);
      right.Add(b);
    }
    left.Sort();
    right.Sort();
    int total = 0;
    for (int i = 0; i < lines.Length; i++)
    {
      total += Math.Abs(left[i] - right[i]);
    }
    Console.WriteLine(total);
  }   
}