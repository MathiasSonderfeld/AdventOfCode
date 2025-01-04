using System.Collections.Concurrent;
using System.Text;

namespace AdventOfCode.Day12S1;

public class Day12S1
{
    private static char[,] map;

    private static List<List<Position>> areas = [];
    private static List<Position> plots = [];
    
    public static void Run()
    {
        var lines = File.ReadAllLines("./Day12s1/input");

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
        
        var sum = areas.Select(positions => positions.Select(GetFenceCount).Sum() * positions.Count).Sum();
        
        Console.WriteLine(sum);
    }

    private static void FindAreas(Position start)
    {
        var letter = map[start.X, start.Y];
        
        areas.Last().Add(start);
        plots.Remove(start);

        var neighbours = GetNeigbours(start);
        foreach (var neighbour in neighbours)
        {
            if(areas.Last().Contains(neighbour) || map[neighbour.X, neighbour.Y] != letter) continue;
            FindAreas(neighbour);
        }
    }

    private static List<Position> GetNeigbours(Position pos)
    {
        var neighbours = new List<Position>();
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
            
            if(IsValid(neigbbour)) neighbours.Add(neigbbour);
        }
        return neighbours;
    }

    private static bool IsValid(Position pos)
    {
        return pos.X >= 0 && pos.X < map.GetLength(0) && pos.Y >= 0 && pos.Y < map.GetLength(1);
    }

    private static int GetFenceCount(Position pos)
    {
        var sum = 0;
        if (pos.X == 0) sum++;
        if (pos.X == map.GetLength(0) - 1) sum++;
        if (pos.Y == 0) sum++;
        if (pos.Y == map.GetLength(1) - 1) sum++;
        
        var neighbours = GetNeigbours(pos);
        
        sum += neighbours.Sum(neighbour => map[neighbour.X, neighbour.Y] != map[pos.X, pos.Y] ? 1 : 0);
        return sum;
    }
}

public record Position(int X, int Y);
public enum Direction { Up, Down, Left, Right }