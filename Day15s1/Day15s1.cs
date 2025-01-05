using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day15S1;

public class Day15S1
{
    private static Dictionary<Position, Tile> tiles = new();
    private static Robot robot;
    
    public static void Run()
    {
        var lines = File.ReadAllLines("./Day15s1/input");

        int y = 0;
        for (; y < lines.Length; y++)
        {
            if (string.IsNullOrWhiteSpace(lines[y])) break;
            for (int x = 0; x < lines[y].Length; x++)
            {
                var pos = new Position(x, y);
                var ch = lines[y][x];
                if(ch == '.') continue;
                Tile tile = ch switch
                {
                    '#' => new Wall(pos),
                    'O' => new Box(pos),
                    '@' => new Robot(pos),
                    _ => throw new ArgumentException($"Invalid tile at position {pos} with value {ch}")
                };
                tiles.Add(pos, tile);
                if (tile is Robot r)
                {
                    robot = r;
                }
            }
        }
        Visualize(Direction.Right);
        for (; y < lines.Length; y++)
        {
            foreach (var step in lines[y].Select(c => c.AsDirection()))
            {
                Move(robot, step);
            }
        }
        
        var total = tiles.Select(t => t.Value).Where(t => t is Box).Select(b => b.Position.Y * 100 + b.Position.X).Sum();
        Console.WriteLine(total);
    }

    private static void Visualize(Direction direction)
    {
        var maxX = tiles.Keys.Max(x => x.X);
        var maxY = tiles.Keys.Max(x => x.Y);
        
        Console.WriteLine(direction);
        for (int yIndex = 0; yIndex <= maxY; yIndex++)
        {
            for (int xIndex = 0; xIndex <= maxX; xIndex++)
            {
                var pos = new Position(xIndex, yIndex);
                var tile = tiles.GetValueOrDefault(pos);
                char c = tile switch
                {
                    null => '.',
                    Wall => '#',
                    Robot => '@',
                    Box => 'O',
                    _ => throw new ArgumentException($"Invalid tile at position {pos}")
                };
                Console.Write(c);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private static bool Move(Tile tile, Direction direction)
    {
        var neighbourPosition = direction.GetNeighbourInDirection(tile.Position);
        var neighbourTile = tiles.GetValueOrDefault(neighbourPosition);
        var movable = neighbourTile switch
        {
            null => true,
            Box => Move(neighbourTile, direction),
            Robot => throw new InvalidDataException("Invalid robot position"),
            Wall or _ => false
        };

        if (!movable) return movable;
        
        tiles.Remove(tile.Position);
        tiles.Add(neighbourPosition, tile with { Position = neighbourPosition });
        if (tile is Robot r)
        {
            robot = r with { Position = neighbourPosition };
        }
        return movable;
    }
    
    
    
}
public record Position(int X, int Y);
public record Tile(Position Position);
public record Robot(Position Position) : Tile(Position);
public record Wall(Position Position) : Tile(Position);
public record Box(Position Position) : Tile(Position);
public enum Direction { Up, Down, Left, Right };

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

    public static Direction AsDirection(this char c)
    {
        return c switch
        {
            '^' => Direction.Up,
            'v' => Direction.Down,
            '<' => Direction.Left,
            '>' => Direction.Right,
            _ => throw new ArgumentException($"Invalid direction at position {c}")
        };
    }
}
