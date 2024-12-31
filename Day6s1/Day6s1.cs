
using System.Collections;
using System.Text.RegularExpressions;
using AdventOfCode.Day4s1;

namespace AdventOfCode.Day5s2;

public class Day5s2
{
  static Dictionary<int, List<int>> rules = new();


  public static void Run()
  {
    string[] lines = File.ReadAllLines("./Day5s2/input");
    
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
    List<List<int>> brokenRecordsList = new List<List<int>>();
    
    foreach (var print in prints)
    {
      var (isOk, index) = isValid(print);
      if (!isOk)
      {
        brokenRecordsList.Add(print);
      }
    }
    

    foreach (var brokenRecord in brokenRecordsList)
    {
      var fixedRecord = CorrectUpdate(brokenRecord);
      
      var m = fixedRecord.Count / 2;
      var mid = fixedRecord[m];
      total += mid;
    }
    
    Console.WriteLine(total);
  }
  
  static List<int> CorrectUpdate(List<int> updateLine) {
    
    for (int checkIndex = 0; checkIndex < updateLine.Count; checkIndex++) {
      
      if (!rules.ContainsKey(updateLine[checkIndex])) continue;
      
      var notAllowedAfterNumbers = rules[updateLine[checkIndex]];
      
      for (int errorSearchIndex = checkIndex + 1; errorSearchIndex < updateLine.Count; errorSearchIndex++) {
        
        if (notAllowedAfterNumbers.Contains(updateLine[errorSearchIndex])) {
          
          var errorFound = updateLine[errorSearchIndex];
          for (int swapIndex = errorSearchIndex; swapIndex > checkIndex; swapIndex--) {
            updateLine[swapIndex] = updateLine[swapIndex - 1];
          }
          updateLine[checkIndex] = errorFound;
        }
      }
    }

    if(isValid(updateLine.ToList()).valid) return updateLine;

    return CorrectUpdate(updateLine);
  }

  private static (bool valid, int index) isValid(List<int> numbers)
  {
    bool isOk = true;
    int index = -1;
    for (int i = 0; i < numbers.Count; i++)
    {
      var number = numbers[i];
      if (!rules.ContainsKey(number))
      {
        continue;        
      }
      foreach (var rule in rules[number])
      {
        var numberPos = numbers.IndexOf(number);
        var rulePos = numbers.IndexOf(rule);
        if (numberPos < rulePos)
        {
          isOk = false;
          index = i;
          break;
        }
      }
    }
    
    return (isOk, index);
  }
}