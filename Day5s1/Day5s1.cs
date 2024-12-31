
using System.Collections;
using System.Text.RegularExpressions;
using AdventOfCode.Day4s1;

namespace AdventOfCode.Day5s1;

public class Day5s1
{

  public static void Run()
  {
    string[] lines = File.ReadAllLines("./Day5s1/input");
    
    Dictionary<int, List<int>> rules = new Dictionary<int, List<int>>();
    List<List<int>> prints = new List<List<int>>();
    int total = 0;
    
    int i = 0;
    for (; i < lines.Length; i++)
    {
      if (String.IsNullOrEmpty(lines[i]))
      {
        break;
      }
      var split = lines[i].Split('|').Select(int.Parse).ToArray();
      var toLookfor = split[1];
      var toVerify = split[0];
      if (!rules.ContainsKey(toLookfor))
      {
        rules.Add(toLookfor, new List<int>());
      }
      rules[toLookfor].Add(toVerify);
    }
    i++;
    for (; i < lines.Length; i++)
    {
      var numbers = lines[i].Split(',');
      var numblist = numbers.Select(int.Parse).ToList();
      prints.Add(numblist);
    }

    
    foreach (var print in prints)
    {
      bool isOk = true;
      foreach (var number in print)
      {
        if (!rules.ContainsKey(number))
        {
          continue;        
        }
        foreach (var rule in rules[number])
        {
          var numberPos = print.IndexOf(number);
          var rulePos = print.IndexOf(rule);
          if (numberPos < rulePos)
          {
            isOk = false;
            break;
          }
        }
      }

      if (isOk)
      {
        var m = print.Count / 2;
        var mid = print[m];
        total += mid;
      }
    }
    Console.WriteLine(total);
  }
}