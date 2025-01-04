using System.Collections.Concurrent;
using System.Text;

namespace AdventOfCode.Day12S2;

public class Day12S2
{
    private static char[,] map;

    private static List<List<Position>> areas = [];
    private static List<Position> plots = [];
    
    public static void Run()
    {
        var lines = File.ReadAllLines("./Day12S2/input");

        map = new char[lines.Length, lines[0].Length];
        
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                map[x, y] = lines[y][x];
                plots.Add(new Position(x, y));
            }
        }

        var plot = new Position(0, 0);
        while (plot != null)
        {
            areas.Add([]);
            FindAreas(plot);
            
            plot = plots.FirstOrDefault();
        }
        
        var sum = areas.Select(positions => GetSideCount(positions) * positions.Count).Sum();
        
        Console.WriteLine(sum);
    }

    private static void FindAreas(Position start)
    {
        var letter = map[start.X, start.Y];
        
        areas.Last().Add(start);
        plots.Remove(start);

        var neighbours = GetNeigbours(start).Select(pos => pos.position).ToList();
        foreach (var neighbour in neighbours)
        {
            if(areas.Last().Contains(neighbour) || map[neighbour.X, neighbour.Y] != letter) continue;
            FindAreas(neighbour);
        }
    }

    private static List<(Direction direction, Position position)> GetNeigbours(Position pos)
    {
        var neighbours = new List<(Direction, Position)>();
        foreach (var direction in Enum.GetValues<Direction>())
        {
            var neigbbour = direction switch
            {
                Direction.Up => pos with { Y = pos.Y - 1 },
                Direction.Down => pos with { Y = pos.Y + 1 },
                Direction.Left => pos with { X = pos.X - 1 },
                Direction.Right => pos with { X = pos.X + 1 },
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
            
            if(IsValid(neigbbour)) neighbours.Add((direction, neigbbour));
        }
        return neighbours;
    }

    private static bool IsValid(Position pos)
    {
        return pos.X >= 0 && pos.X < map.GetLength(0) && pos.Y >= 0 && pos.Y < map.GetLength(1);
    }

    private static int GetSideCount(List<Position> plot)
    {
        var fences = plot.Select(pos => (pos, GetFenceDirections(pos))).ToDictionary(fence => fence.Item1, fence => fence.Item2);
        
        var minX = plot.Min(p => p.X);
        var minY = plot.Min(p => p.Y);
        var maxX = plot.Max(p => p.X);
        var maxY = plot.Max(p => p.Y);

        var borderDetected = (false, false);
        var sum = 0;

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                var fence = fences.GetValueOrDefault(new Position(x, y)) ?? [];

                var borderUp = fence.Contains(Direction.Up);
                var borderDown = fence.Contains(Direction.Down);
                
                if (!borderDetected.Item1 && borderUp) sum++;
                if (!borderDetected.Item2 && borderDown) sum++;
                
                borderDetected = (borderUp, borderDown);
            }
        }
        
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                var fence = fences.GetValueOrDefault(new Position(x, y)) ?? [];

                var borderLeft = fence.Contains(Direction.Left);
                var borderRight = fence.Contains(Direction.Right);
                
                if (!borderDetected.Item1 && borderLeft) sum++;
                if (!borderDetected.Item2 && borderRight) sum++;
                
                borderDetected = (borderLeft, borderRight);
            }
        }
        
        return sum;
    }

    private static List<Direction> GetFenceDirections(Position pos)
    {
        var list = new List<Direction>();
        if (pos.X == 0) list.Add(Direction.Left);
        if (pos.X == map.GetLength(0) - 1) list.Add(Direction.Right);
        if (pos.Y == 0) list.Add(Direction.Up);
        if (pos.Y == map.GetLength(1) - 1) list.Add(Direction.Down);
        
        var neighbours = GetNeigbours(pos);
        
        foreach (var neighbour in neighbours)
        {
            if (map[neighbour.position.X, neighbour.position.Y] != map[pos.X, pos.Y])
            {
                list.Add(neighbour.direction);
            }
        }
        return list;
    }
}

public record Position(int X, int Y);
public enum Direction { Up, Down, Left, Right }