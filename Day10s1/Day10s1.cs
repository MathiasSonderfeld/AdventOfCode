using System.Text;

namespace AdventOfCode.Day10s1;

public class Day10S1
{
    private static int[,] map;
    
    public static void Run()
    {
        string[] lines = File.ReadAllLines("./Day10s1/input");
        map = new int[lines.Length, lines[0].Length];
        List<Position> trailheads = [];
        
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                char current = lines[i][j];
                if (current == '0')
                {
                    trailheads.Add(new Position(j, i));
                }
                map[j, i] = int.Parse("" + current);
            }
        }
        var ths = trailheads.Select(t => (t, FindTrail(t))).ToList();
        var total = ths.Select(a => a.Item2.Count).Sum();
        Console.WriteLine(total);
    }

    private static List<Position> FindTrail(Position current)
    {
        var currentLevel = map[current.X, current.Y];
        if (currentLevel == 9) return [current];
        
        var nextLevel = currentLevel + 1;
        var ret = Enum.GetValues<Direction>().SelectMany(direction =>
        {
            var ng = GetNeighbour(current, direction);
            if (!isValidPosition(ng) || map[ng.X, ng.Y] != nextLevel)
                return [];
            return FindTrail(ng);
            
        }).Distinct().ToList();
        return ret;
    }

    private static Position GetNeighbour(Position current, Direction direction)
    {
        return direction switch
        {
            Direction.Up => current with { Y = current.Y - 1 },
            Direction.Down => current with { Y = current.Y + 1 },
            Direction.Left => current with { X = current.X - 1 },
            Direction.Right => current with { X = current.X + 1 },
            _ => throw new NotImplementedException(),
        };
    }

    private static bool isValidPosition(Position current)
    {
        return current.X >= 0 && current.X < map.GetLength(0) && current.Y >= 0 && current.Y < map.GetLength(1);
    }
}
record Position(int X, int Y);
enum Direction { Up, Down, Left, Right }