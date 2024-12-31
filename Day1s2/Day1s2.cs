namespace AdventOfCode.Day1s1;

public class Day1s2
{
  public static void Run()
  {
    string inputFile = File.ReadAllText("../../../Day1s2/input");
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
    
    int similarityCode = 0;
    for (int i = 0; i < lines.Length; i++)
    {
      similarityCode += left[i] * right.FindAll(x => x == left[i]).Count();
    }
    Console.WriteLine(similarityCode);
  }   
}