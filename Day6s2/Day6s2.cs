
using System.Collections;
using System.Text.RegularExpressions;
using AdventOfCode.Day4s1;

namespace AdventOfCode.Day6s2;

public class Day6s2
{
  public static void Run()
  {
    string[] lines = File.ReadAllLines("./Day6s2/input");
    var map = new string[lines.Length, lines[0].Length];
    int total = 0;
    Position start = new Position();
    for (int i = 0; i < lines.Length; i++)
    {
      for (int j = 0; j < lines[0].Length; j++)
      {
        map[j,i] = "" + lines[i][j];
        if (lines[i][j] == '^')
        {
          start.X = j;
          start.Y = i;
        }
      }
    }
    Console.WriteLine($"loading map completed");
    // add code here

    var (steps,  loop) = WalkFrom(map, new Step() { Direction = Direction.Up, Position = start });
    Console.WriteLine($"loading path completed");
    
    
    total = steps.Distinct().AsParallel().Select<Step, Position?>(step =>
      {
        string[,] clone = map.Clone() as string[,];
        var nextPosition = step.NextPosition();
        if (!isValidPosition(map, nextPosition))
        {
          return null;
        }
        clone[nextPosition.X, nextPosition.Y] = "#";

        var w = WalkFrom(clone, step);
        if (w.loop) return nextPosition;
        return null;
      })
      .Where(w => w != null)
      .Distinct()
      .Count();
    Console.WriteLine($"identifying path completed");

    Console.WriteLine($"identifying loops completed");

    // print results
    // Console.WriteLine($"Total steps: {steps.Count}");
    // Console.WriteLine($"Loop: {loop}");
    Console.WriteLine($"total: {total}");
    // Console.WriteLine($"Total fields: {steps.Select(x => x.Position).Distinct().Count()}");
  }

  static (List<Step> steps, bool loop) WalkFrom(string[,] map, Step here)
  {
    List<Step> steps = new List<Step>();
    var currentStep = here;
    bool isLoop = false;
    while (isValidPosition(map, currentStep.Position) && !isLoop)
    {
      steps.Add(currentStep);
      currentStep = GetNextPosition(map, currentStep);
      isLoop = steps.Contains(currentStep);
    }

    return (steps, isLoop);
  }
  
  static Step GetNextPosition(string[,] map, Step step)
  {
    
    var nextPosition = step.NextPosition();
    if (!isValidPosition(map, nextPosition))
    {
      return step with {Position = nextPosition};
    }
    var isValid = isSteppable(map, nextPosition);
    var newStep = step with {Position = nextPosition};
    while (!isValid)
    {
      var newDirection = rotate(newStep.Direction);
      newStep = step with {Direction = newDirection};
      nextPosition = newStep.NextPosition();
      isValid = isSteppable(map, nextPosition);
    }
    return newStep;
  }

  static bool isSteppable(string[,] map, Position position)
  {
    var field = map[position.X, position.Y];
    return "#" != field;
  }

  static Direction rotate(Direction direction)
  {
    return direction switch
    {
      Direction.Up => Direction.Right,
      Direction.Right => Direction.Down,
      Direction.Down => Direction.Left,
      Direction.Left => Direction.Up,
      _ =>  throw new NotImplementedException()
    };
  }
  
  static bool isValidPosition(string[,] map, Position position)
  {
    return position is { X: >= 0, Y: >= 0 } && position.X < map.GetLength(0) && position.Y < map.GetLength(1);
  }
}

record Position
{
  public int X { get; set; }
  public int Y { get; set; }
}

record Step
{
  public Position Position { get; set; }
  public Direction Direction { get; set; }

  
  public Position NextPosition()
  {
    return Direction switch
    {
      Direction.Up => new Position { X = Position.X, Y = Position.Y - 1 },
      Direction.Down => new Position { X = Position.X, Y = Position.Y + 1 },
      Direction.Left => new Position { X = Position.X - 1, Y = Position.Y },
      Direction.Right => new Position { X = Position.X + 1, Y = Position.Y },
      _ => throw new NotImplementedException(),
    };
  }
}

enum Direction { Up, Down, Left, Right }