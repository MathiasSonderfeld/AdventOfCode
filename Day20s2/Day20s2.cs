using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day20s2;

public class Day20s2
{
    public const int CheatSeconds = 20;
    
    private static char[,] map;
    private static Position start;
    private static Position end;
    private static List<Position> processedSteps = [];
    
    public static void Run()
    {
        var lines = File.ReadAllLines("./Day20s2/input");
        
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

        var position = start;
        processedSteps.Add(position);
        
        while (map[position.X, position.Y] != 'E') {
            foreach (var direction in Enum.GetValues<Direction>())
            {
                var neighbourInDirection = direction.GetNeighbourInDirection(position);
                var letter = map[neighbourInDirection.X, neighbourInDirection.Y];
                if (letter is not '.' and not 'E' || processedSteps.Contains(neighbourInDirection)) continue;
                
                processedSteps.Add(neighbourInDirection);
                position = neighbourInDirection;
            }
        }

        var possibleShortCuts = new List<(Position, Position, int)>();
        for (var i = 0; i < processedSteps.Count; i++)
        {
            for (var j = i + 52; j < processedSteps.Count; j++)
            {
                if (processedSteps[i].IsShortCutAble(processedSteps[j]))
                {
                    possibleShortCuts.Add((processedSteps[i], processedSteps[j], j - i - 2));
                }
            }
        }
        
        Console.WriteLine(possibleShortCuts.Count);
        var t = possibleShortCuts.GroupBy(x => x.Item3).ToDictionary(x => x.Key, x => x.Count());
        Console.WriteLine(string.Join("\n", t.Select(t => t.Key + ": " + t.Value)));
        
    }
    
    private static void Visualize(Direction direction)
    {
        var maxX = map.GetLength(0);
        var maxY = map.GetLength(1);
        
        Console.WriteLine(direction);
        for (var yIndex = 0; yIndex <= maxY; yIndex++)
        {
            for (var xIndex = 0; xIndex <= maxX; xIndex++)
            {
                Console.Write(map[xIndex, yIndex]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}

public record Position(int X, int Y)
{
    public bool IsShortCutAble(Position position)
    {
        var deltaX = Math.Abs(X - position.X);
        var deltaY = Math.Abs(Y - position.Y);
        return deltaX + deltaY <= Day20s2.CheatSeconds;
    }
};
public record State(Position Position, int CurrentCosts);
public enum Direction { North, South, West, East };

public static class DirectionExtensions
{
    public static Position GetNeighbourInDirection(this Direction dir, Position pos)
    {
        return dir switch
        {
            Direction.North => pos with { Y = pos.Y - 1 },
            Direction.South => pos with { Y = pos.Y + 1 },
            Direction.West => pos with { X = pos.X - 1 },
            Direction.East => pos with { X = pos.X + 1 },
            _ => throw new ArgumentException($"Invalid direction at position {pos}")
        };
    }
}
