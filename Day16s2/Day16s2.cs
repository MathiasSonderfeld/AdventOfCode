using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day16s2;

public class Day16s2
{
    private static char[,] map;
    private static Position start;
    private static Position end;
    private static HashSet<PositionAndDirection> processedSteps = []; 
    
    public static void Run()
    {
        var lines = File.ReadAllLines("./Day16s1/input");
        
        map = new char[lines[0].Length, lines.Length];
        
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[0].Length; x++)
            {
                map[x, y] = lines[y][x];
                switch (lines[y][x])
                {
                    case 'S':
                        start = new Position(x, y);
                        break;
                    case 'E':
                        end = new Position(x, y);
                        break;
                }
            }
        }
        
        PriorityQueue<State, int> queue = new();
        queue.Enqueue(new State(new PositionAndDirection(start, Direction.Right), 0), 0);

        var totalCost = int.MaxValue;
        while (queue.Count > 0)
        {
            var state = queue.Dequeue();
            if (state.Current.Position.Equals(end))
            {
                totalCost = state.CurrentCosts;
                break;
            }
            processedSteps.Add(state.Current);
            Step(state.Current.Direction.GetCounterclockwiseTurn(), state, 1001, queue);
            Step(state.Current.Direction.GetClockwiseTurn(), state, 1001, queue);
            Step(state.Current.Direction, state, 1, queue);           
            if (queue.Count == 0)
            {
                Console.WriteLine("No steps");
            }
        }
        
        Console.WriteLine(totalCost);
    }

    private static void Step(Direction dir, State state, int additionalCosts, PriorityQueue<State, int> queue)
    {
        var neighbourInDirection = dir.GetNeighbourInDirection(state.Current.Position);
        var newPositionAndDirection = new PositionAndDirection(neighbourInDirection, dir);
        var leftChar = map[neighbourInDirection.X, neighbourInDirection.Y];
        if (leftChar is not '.' and not 'E' || processedSteps.Contains(newPositionAndDirection)) return;
        var newCost = state.CurrentCosts + additionalCosts;
        queue.Enqueue(new State(newPositionAndDirection, newCost), newCost);
    }

    private static void Visualize(Direction direction)
    {
        var maxX = map.GetLength(0);
        var maxY = map.GetLength(1);
        
        Console.WriteLine(direction);
        for (int yIndex = 0; yIndex <= maxY; yIndex++)
        {
            for (int xIndex = 0; xIndex <= maxX; xIndex++)
            {
                Console.Write(map[xIndex, yIndex]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
public record Position(int X, int Y);
public record PositionAndDirection(Position Position, Direction Direction);
public record State(PositionAndDirection Current, int CurrentCosts);
public enum Direction { NORTH, Down, Left, Right };

public static class DirectionExtensions
{
    public static Position GetNeighbourInDirection(this Direction dir, Position pos)
    {
        return dir switch
        {
            Direction.NORTH => pos with { Y = pos.Y - 1 },
            Direction.Down => pos with { Y = pos.Y + 1 },
            Direction.Left => pos with { X = pos.X - 1 },
            Direction.Right => pos with { X = pos.X + 1 },
            _ => throw new ArgumentException($"Invalid direction at position {pos}")
        };
    }

    public static Direction GetCounterclockwiseTurn(this Direction dir)
    {
        return dir switch
        {
            Direction.NORTH => Direction.Left,
            Direction.Down => Direction.Right,
            Direction.Left => Direction.Down,
            Direction.Right => Direction.NORTH,
            _ => throw new ArgumentException($"Invalid direction at position {dir}")
        };
    }

    public static Direction GetClockwiseTurn(this Direction dir)
    {
        return dir switch
        {
            Direction.NORTH => Direction.Right,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.NORTH,
            Direction.Right => Direction.Down,
            _ => throw new ArgumentException($"Invalid direction at position {dir}")
        };
    }
}
