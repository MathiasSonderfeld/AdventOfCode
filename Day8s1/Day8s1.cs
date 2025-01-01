namespace AdventOfCode.Day8s1;

public class Day8S1
{
    public static void Run()
    {
        string[] lines = File.ReadAllLines("./Day8s1/input");
        
        int xLimit = lines[0].Length-1;
        int yLimit = lines.Length-1;
        Dictionary<char, List<Position>> antennas = new();
        
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                char c = lines[y][x];
                if (c == '.') {continue;}
                var list = antennas.GetValueOrDefault(c, []);
                list.Add(new Position(x, y));
                antennas[c] = list;
            }
        }
        
        var antiNodes = antennas.SelectMany(pair =>
            pair.Value.SelectMany(antenna =>
                pair.Value.Except([antenna]).Select(other => CalculateAntiNode(antenna, other))))
            .Distinct()
            .Where(n => WithinLimits(n, xLimit, yLimit))
            .ToHashSet();
        Console.WriteLine(antiNodes.Count);
    }

    private static Position CalculateAntiNode(Position antenna, Position other)
    {
        var nx = other.x * 2 - antenna.x; 
        var ny = other.y * 2 - antenna.y;
        return new Position(nx, ny);
    }

    private static bool WithinLimits(Position position, int xLimit, int yLimit)
    {
        return position.x >= 0 && position.x <= xLimit && position.y >= 0 && position.y <= yLimit;
    }
}

internal record Position(int x, int y);