namespace AdventOfCode.Day6s2;

public class Day6S2
{
  public static void Run()
  {
    string[] lines = File.ReadAllLines("./Day6s2/input");
    var map = new string[lines.Length, lines[0].Length];
    int total;
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

    var (steps, _) = WalkFrom(map, new Step() { Direction = Direction.Up, Position = start });
    Console.WriteLine($"loading path completed");
    
    var totalTest = steps.Distinct().AsParallel().Select<Step, Position?>(step =>
      {
        string[,] clone = map.Clone() as string[,];
        var nextPosition = step.NextPosition();
        if (!IsValidPosition(map, nextPosition) || !IsSteppable(map, nextPosition))
        {
          return null;
        }
        clone[nextPosition.X, nextPosition.Y] = "0";

        var w = WalkFrom(clone, steps[0]);

        if (w.loop)
        {
          return nextPosition;
        }
        return null;
      })
      .Where(w => w != null)
      .Distinct().ToList();
      total = totalTest.Count();
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
    while (IsValidPosition(map, currentStep.Position) && !isLoop)
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
    if (!IsValidPosition(map, nextPosition))
    {
      return step with {Position = nextPosition};
    }
    var isValid = IsSteppable(map, nextPosition);
    var newStep = step with {Position = nextPosition};
    while (!isValid)
    {
      var newDirection = Rotate(newStep.Direction);
      newStep = step with {Direction = newDirection};
      nextPosition = newStep.NextPosition();
      isValid = IsSteppable(map, nextPosition);
    }
    return newStep;
  }

  static bool IsSteppable(string[,] map, Position position)
  {
    var field = map[position.X, position.Y];
    return "#" != field && "0" != field;
  }

  static Direction Rotate(Direction direction)
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
  
  static bool IsValidPosition(string[,] map, Position position)
  {
    return position is { X: >= 0, Y: >= 0 } && position.X < map.GetLength(0) && position.Y < map.GetLength(1);
  }

  static void PrintWalkedMap(string[,] map, List<Step> steps)
  {
    foreach (var step in steps)
    {
      map[step.Position.X, step.Position.Y] = step.Direction switch
      {
        Direction.Up => "|",
        Direction.Right => "_",
        Direction.Down => "|",
        Direction.Left => "_",
      };
    }

    for (int i = 0; i < map.GetLength(0); i++)
    {
      for (int j = 0; j < map.GetLength(1); j++)
      {
        Console.Write(map[j, i]);
      }
      Console.WriteLine();
    }
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