
using AdventOfCode.Day4s1;

namespace AdventOfCode.Day4s2;

public class Day4s2
{
  private static string[][] arr;
  private static string xmas = "XMAS";
  
  public static void Run()
  {
    string[] lines = File.ReadAllLines("../../../Day4s2/input");
    var lineLen = lines[0].Length;
    arr = new string[lines.Length][];
    int totalXmas = 0;
    for (int i = 0; i < lines.Length; i++)
    {
      arr[i] = new string[lineLen];
      for (int j = 0; j < lineLen; j++)
      {
        arr[i][j] = "" + lines[i][j];    
      }
    }
    
    for (int currentY = 1; currentY < lineLen-1; currentY++)
    {
      for (int currentX = 1; currentX < lines.Length-1; currentX++)
      {
        var current = arr[currentY][currentX];
        if (current == "A")
        {
          var nw = Direction.NORTHWEST.NewDirection(currentX, currentY);
          var ne = Direction.NORTHEAST.NewDirection(currentX, currentY);
          var sw = Direction.SOUTHWEST.NewDirection(currentX, currentY);
          var se = Direction.SOUTHEAST.NewDirection(currentX, currentY);
          var nwc =  arr[nw.y][nw.x];
          var nec = arr[ne.y][ne.x];
          var swc = arr[sw.y][sw.x];
          var sec = arr[se.y][se.x];

          var p = nwc == "M" && sec == "S" || nwc == "S" && sec == "M";
          var q = nec == "M" && swc == "S" || nec == "S" && swc == "M";

          if (p && q)
          {
            totalXmas++;
          }
        }
      }
    }
    Console.WriteLine(totalXmas);
  }

  private static bool testDirection(int currentX, int currentY, Direction direction, int depth)
  {
    if (depth >= xmas.Length)
    {
      return true;
    }
    if(currentX < 0 || currentX >= arr[0].Length || currentY < 0 || currentY >= arr[depth].Length)
    {
      return false;
    }
    var current = arr[currentY][currentX];
    if (current == "" + xmas[depth])
    {
      var (x,y) = direction.NewDirection(currentX, currentY);
      return testDirection(x, y, direction, depth + 1);
    }
    return false;
  }
}

public enum Direction
{
  NORTHEAST,
  NORTHWEST,
  SOUTHEAST,
  SOUTHWEST
}

public static class DirectionExtensions
{
  public static (int x, int y) NewDirection(this Direction dir, int currentX, int currentY)
  {
    return dir switch
    {
      Direction.NORTHEAST => (currentX + 1, currentY - 1),
      Direction.NORTHWEST => (currentX - 1, currentY - 1),
      Direction.SOUTHEAST => (currentX + 1, currentY + 1),
      Direction.SOUTHWEST => (currentX - 1, currentY + 1),
      _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
    };
  }
}