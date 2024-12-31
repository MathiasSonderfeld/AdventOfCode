
using AdventOfCode.Day4s1;

namespace AdventOfCode.Day4s1;

public class Day4s1
{
  private static string[][] arr;
  private static string xmas = "XMAS";
  
  public static void Run()
  {
    string[] lines = File.ReadAllLines("./Day4s1/input");
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
    
    for (int currentY = 0; currentY < lineLen; currentY++)
    {
      for (int currentX = 0; currentX < lines.Length; currentX++)
      {
        var current = arr[currentY][currentX];
        if (current == "X")
        {
          foreach (var dir in Enum.GetValues(typeof(Direction)).Cast<Direction>())
          {
            var r = testDirection(currentX, currentY, dir, 0);
            if (r)
            {
              Console.WriteLine("found xmas at " + currentX + ", " + currentY + ", " + dir);
              totalXmas++;
            }
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
  NORTH,
  SOUTH,
  EAST,
  WEST,
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
      Direction.NORTH => (currentX, currentY - 1),
      Direction.SOUTH => (currentX, currentY + 1),
      Direction.EAST => (currentX + 1, currentY),
      Direction.WEST => (currentX - 1, currentY),
      Direction.NORTHEAST => (currentX + 1, currentY - 1),
      Direction.NORTHWEST => (currentX - 1, currentY - 1),
      Direction.SOUTHEAST => (currentX + 1, currentY + 1),
      Direction.SOUTHWEST => (currentX - 1, currentY + 1),
      _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
    };
  }
}