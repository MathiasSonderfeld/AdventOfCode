using System.Text.RegularExpressions;

namespace AdventOfCode.Day3s1;

public class Day3s1
{
  public static void Run()
  {
    string inputFile = File.ReadAllText("../../../Day3s1/input");
    string pattern = "mul\\(\\d{1,3},\\d{1,3}\\)";
    var matches = Regex.Matches(inputFile, pattern);
    int total = 0;
    for (var i = 0; i < matches.Count; i++)
    {
      var match = Regex.Match(matches[i].Value, @"\d{1,3}");
      var a = match.Value;
      var b = match.NextMatch().Value;
      var ai = int.Parse(a);
      var bi = int.Parse(b);
      total += ai * bi;
    }
    Console.WriteLine(total);
  }   
}