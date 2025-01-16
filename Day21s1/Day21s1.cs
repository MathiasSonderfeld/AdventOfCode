using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day21s1;

public class Day21s1
{
    
    public static void Run()
    {
        var lines = File.ReadAllLines("./Day21s1/input");
        var robot = new RobotCalculator(new NumericKeypad(10));
        var secondRobot = new RobotCalculator(new DirectionalKeypad(10));
        var thirdRobot = new RobotCalculator(new DirectionalKeypad(10));


        var total = lines.Select(line =>
        {
            var start = line.Select(c => c == 'A' ? 10 : int.Parse("" + c)).ToList();
            var r1 = robot.Press([start]);
            var r2 = secondRobot.Press(r1);
            var r3 = thirdRobot.Press(r2);
            Console.WriteLine($"r3: {r3.Count}");
            var complexity =  int.Parse(line.Substring(0, 3)) * r3.Min(l => l.Count);
            return complexity;
        }).Sum();

        Console.WriteLine(total);

        // var res = robot.Press([[7,3]]);
        // Console.WriteLine(String.Join("\n", res.Select(l => string.Join(", ", l.Select(c => c.AsInput())))));
    }
}

class RobotCalculator(Keypad keypad)
{
    private Keypad _keypad = keypad;
    
    public List<List<int>> Press(List<List<int>> values)
    {
        var allOptions = values.SelectMany(way =>
            {
                var res = way
                    .Select(key => _keypad.MoveTo(key))
                    .Aggregate((a, b) =>
                        a.SelectMany(inner => b.Select(innerB => inner.Concat(innerB).ToList())).ToList())
                    .ToList();
                _keypad.Reset();
                return res;
            })
            .ToList();

        return allOptions;
    }
}

internal abstract class Keypad(int initial)
{
    private int _current = initial;
    private readonly int _initial = initial;

    private Dictionary<(int dx, int dy, Direction xDirection, Direction yDirection, Position currentPosition), List<List<int>>> Cache = new();
    
    protected abstract Dictionary<int, Position> Map();
    
    public void Reset()
    {
        _current = _initial;
    }
    
    public List<List<int>> MoveTo(int newValue)
    {
        var currentPosition = Map()[_current];
        var targetPosition = Map()[newValue];
        _current = newValue;
        
        var (dx, dy) = currentPosition.DistanceXY(targetPosition);
        var xdir = dx < 0 ? Direction.Right : Direction.Left;
        var ydir = dy > 0 ? Direction.Up : Direction.Down;
        
        return MoveRecursive(dx, dy, xdir, ydir, currentPosition);
    }



    private List<List<int>> MoveRecursive(int dx, int dy, Direction xDirection, Direction yDirection, Position currentPosition)
    {
        if (dx == 0 && dy == 0)
        {
            return [[10]];
        }
        if (Cache.TryGetValue((dx, dy, xDirection, yDirection, currentPosition), out var cache))
        {
            return cache;
        }

        var xMove = dx != 0 ? MoveDirection(xDirection, new Step(dx, currentPosition)) : (false, null);
        var yMove = dy != 0 ? MoveDirection(yDirection, new Step(dy, currentPosition)) : (false, null);
        
        var xr = xMove.canMove ? MoveRecursive(xMove.step!.Delta, dy, xDirection, yDirection, xMove.step.Position)
            .Select(l => new List<int>{(int) xDirection}.Concat(l).ToList()).ToList() : [];
        
        var yr = yMove.canMove ? MoveRecursive(dx, yMove.step!.Delta, xDirection, yDirection, yMove.step.Position)
            .Select(l => new List<int>{(int) yDirection}.Concat(l).ToList()).ToList() : [];

        var total = xr.Concat(yr).ToList();
        Cache[(dx, dy, xDirection, yDirection, currentPosition)] = total;
        return total;
    }

    private (bool canMove, Step step) MoveDirection(Direction direction, Step step)
    {
        Position? newPosition;
        if (step.Delta == 0 || (newPosition = Move(step.Position, direction)) == null) return (false, step);
        var newDelta = step.Delta + direction.Modifier();
        return (true, new Step(newDelta, newPosition));
    }

    private Position? Move(Position position, Direction direction)
    {
        var np = direction.GetNeighbourInDirection(position);
        return Map().ContainsValue(np) ? np : null;
    }
}

class NumericKeypad(int initial) : Keypad(initial)
{
    private static readonly Dictionary<int, Position> map = new()
    {
        { 7, new Position(0, 0) },
        { 8, new Position(1, 0) },
        { 9, new Position(2, 0) },
        { 4, new Position(0, 1) },
        { 5, new Position(1, 1) },
        { 6, new Position(2, 1) },
        { 1, new Position(0, 2) },
        { 2, new Position(1, 2) },
        { 3, new Position(2, 2) },
        { 0, new Position(1, 3) },
        { 10, new Position(2, 3) }
    };

    protected override Dictionary<int, Position> Map()
    {
        return map;
    }
}

class DirectionalKeypad(int initial) : Keypad(initial)
{
    private static readonly Dictionary<int, Position> map = new()
    {
        { (int) Direction.Left, new Position(0, 1) },
        { (int) Direction.Right, new Position(2, 1) },
        { (int) Direction.Up, new Position(1, 0) },
        { (int) Direction.Down, new Position(1, 1) },
        { 10, new Position(2, 0) }
    };

    protected override Dictionary<int, Position> Map()
    {
        return map;
    }
}

public record Step(int Delta, Position Position);
public enum Direction { Up, Down, Left, Right };
public static class IntExtensions
{
    public static char AsInput(this int input)
    {
        return input switch
        {
            (int) Direction.Up => '^',
            (int) Direction.Down => 'v',
            (int) Direction.Left => '<',
            (int) Direction.Right => '>',
            10 => 'A',
            _ => throw new ArgumentException($"Invalid input {input}")
        };
    }
}
public static class DirectionExtensions
{
    public static Position GetNeighbourInDirection(this Direction dir, Position pos)
    {
        return dir switch
        {
            Direction.Up => pos with { Y = pos.Y - 1 },
            Direction.Down => pos with { Y = pos.Y + 1 },
            Direction.Left => pos with { X = pos.X - 1 },
            Direction.Right => pos with { X = pos.X + 1 },
            _ => throw new ArgumentException($"Invalid direction at position {pos}")
        };
    }
    
    public static int Modifier(this Direction dir)
    {
        return dir switch
        {
            Direction.Up => -1,
            Direction.Down => 1,
            Direction.Left => -1,
            Direction.Right => 1,
            _ => throw new ArgumentException($"Invalid direction {dir}")
        };
    }
}
public record Position(int X, int Y)
{
    public (int deltaX, int deltaY) DistanceXY(Position position)
    {
        var deltaX = X - position.X;
        var deltaY = Y - position.Y;
        return (deltaX, deltaY);
    }
};
