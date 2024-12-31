using System.Text.RegularExpressions;

namespace AdventOfCode.Day3s2;

public class Day3s2
{
  public static void Run()
  {
    string pattern = "mul\\(\\d{1,3},\\d{1,3}\\)";
    int total = 0;
    string inputFile = File.ReadAllText("../../../Day3s2/input");

    foreach (string str in inputFile.Split("do()"))
    {
      var dontPosition = str.IndexOf("don't()", StringComparison.CurrentCultureIgnoreCase);
      string substr;
      if (dontPosition > 0)
      {
        substr = str.Substring(0, dontPosition);
      }
      else
      {
        substr = str;
      }
      
      var matches = Regex.Matches(substr, pattern);
      for (var i = 0; i < matches.Count; i++)
      {
        var match = Regex.Match(matches[i].Value, @"\d{1,3}");
        var a = match.Value;
        var b = match.NextMatch().Value;
        var ai = int.Parse(a);
        var bi = int.Parse(b);
        total += ai * bi;
      }
    }
    Console.WriteLine(total);
  }   
}