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
            var r1 = robot.press(start);
            var r2 = secondRobot.press(r1);
            var r3 = thirdRobot.press(r2);
            Console.WriteLine($"r3: {r3.Count}");
            var complexity =  int.Parse(line.Substring(0, 3)) * r3.Count;
            return complexity;
        }).Sum();
        
        Console.WriteLine(total);
    }
}

class RobotCalculator(Keypad keypad)
{
    private Keypad _keypad = keypad;
    
    public List<int> press(List<int> values)
    {
        return values.SelectMany(value => _keypad.MoveTo(value)).ToList();
    }
}

abstract class Keypad(int initial)
{
    private int _current = initial;

    protected abstract Dictionary<int, Position> Map();
    protected abstract int GapY();

    public List<int> MoveTo(int newValue)
    {
        var currentPosition = Map()[_current];
        var targetPosition = Map()[newValue];
        var moveXFirst = currentPosition.Y != GapY();
        var (dx, dy) = currentPosition.DistanceXY(targetPosition);
        var xdir = dx < 0 ? Direction.Right : Direction.Left;
        var xMod = dx < 0 ? 1 : -1;
        var ydir = dy > 0 ? Direction.Up : Direction.Down;
        var yMod = dy < 0 ? 1 : -1;
        
        List<Direction> directions = new();
        while (!currentPosition.Equals(targetPosition))
        {
            if (moveXFirst)
            {
                var moved = MoveDirection(ref dx, xdir, ref currentPosition, directions) || MoveDirection(ref dy, ydir, ref currentPosition, directions);
                if (!moved)
                {
                    throw new Exception("FUCK");
                }
            }
            else
            {
                var moved = MoveDirection(ref dy, ydir, ref currentPosition, directions) || MoveDirection(ref dx, xdir, ref currentPosition, directions);
                if (!moved)
                {
                    throw new Exception("FUCK");
                }
            }
        }
        _current = newValue;
        var l =  directions.Select(d => (int)d).ToList();
        //Press current Button
        l.Add(10);
        return l;
    }

    private bool MoveDirection(ref int delta, Direction direction, ref Position currentPosition, List<Direction> directions)
    {
        Position? newPosition;
        if (delta == 0 || (newPosition = Move(currentPosition, direction)) == null) return false;
        currentPosition = newPosition;
        delta += direction.Modifier();
        directions.Add(direction);
        return true;
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

    protected override int GapY()
    {
        return 3;
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

    protected override int GapY()
    {
        return 0;
    }
}

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
