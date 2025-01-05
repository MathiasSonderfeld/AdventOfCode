using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day15s2;

public class Day15s2
{
    private static Dictionary<Position, Tile> tiles = new();
    private static Robot robot;
    
    public static void Run()
    {
        var lines = File.ReadAllLines("./Day15s2/input");

        var y = 0;
        for (; y < lines.Length; y++)
        {
            if (string.IsNullOrWhiteSpace(lines[y])) break;
            for (var x = 0; x < lines[y].Length; x++)
            {
                List<Position> positions = [new(2*x, y), new(x*2+1, y)];
                var ch = lines[y][x];
                if(ch == '.') continue;
                Tile tile = ch switch
                {
                    '#' => new Wall(positions),
                    'O' => new Box(positions),
                    '@' => new Robot(positions[0]),
                    _ => throw new ArgumentException($"Invalid tile at x {x} and y {y} with value {ch}")
                };
                tiles.Add(positions[0], tile);
                
                if (tile is Robot r)
                {
                    robot = r;
                }
                else
                {
                    tiles.Add(positions[1], tile);
                }
            }
        }
        Visualize(Direction.Right);
        for (; y < lines.Length; y++)
        {
            foreach (var direction in lines[y].Select(c => c.AsDirection()))
            {
                if(CanMove(robot, direction))
                    Move(robot, direction);
            }
        }
        
         var total = tiles.Select(t => t.Value).Where(t => t is Box).Distinct().Select(b => b.Positions[0].Y * 100 + b.Positions[0].X).Sum();
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
                    Box => tile.Positions.First().Equals(pos) ? '[' : ']',
                    _ => throw new ArgumentException($"Invalid tile at position {pos}")
                };
                Console.Write(c);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private static bool CanMove(Tile tile, Direction direction)
    {
        var neighbourPositions = tile.Positions.Select(t => direction.GetNeighbourInDirection(t)).Except(tile.Positions).ToList();
        var movable = neighbourPositions.Select(neighbourPosition =>
        {
            var neighbourTile = tiles.GetValueOrDefault(neighbourPosition);
            return neighbourTile switch
            {
                null => true,
                Box => CanMove(neighbourTile, direction),
                Robot => throw new InvalidDataException("Invalid robot position"),
                Wall or _ => false
            };
        }).Aggregate((a,b) => a && b);
        
        return movable;
    }

    private static void Move(Tile tile, Direction direction)
    {
        var neighbourPositions = tile.Positions.Select(t => direction.GetNeighbourInDirection(t)).Except(tile.Positions).ToList();
        neighbourPositions.ForEach(neighbourPosition =>
        {
            var neighbourTile = tiles.GetValueOrDefault(neighbourPosition);
            if(neighbourTile is Box) Move(neighbourTile, direction);
        });
        
        tile.Positions.ForEach(p => tiles.Remove(p));
        var newPositions = tile.Positions.Select(p => direction.GetNeighbourInDirection(p)).ToList();
        var newTile = tile with{Positions = newPositions};
        newPositions.ForEach(np => tiles.Add(np, newTile));
        if (newTile is Robot r)
        {
            robot = r;
        }
    }
    
    
    
}
public record Position(int X, int Y);
public record Tile(List<Position> Positions);
public record Robot(Position Position) : Tile([Position]);
public record Wall(List<Position> Positions) : Tile(Positions);
public record Box(List<Position> Positions) : Tile(Positions);
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
