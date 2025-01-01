namespace AdventOfCode.Day8s2;

public class Day8S2
{
    private static Position fieldLimit;
    public static void Run()
    {
        string[] lines = File.ReadAllLines("./Day8s2/input");
        
        var xLimit = lines[0].Length-1;
        var yLimit = lines.Length-1;
        fieldLimit = new Position(xLimit, yLimit);
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
                pair.Value.Except([antenna]).SelectMany(other => CalculateAntiNode(antenna, other))))
            .Distinct()
            .ToHashSet();
        Console.WriteLine(antiNodes.Count);
    }

    private static List<Position> CalculateAntiNode(Position antenna, Position other)
    {
        List<Position> result = new();
        var xDist = other.x - antenna.x;
        var yDist = other.y - antenna.y;
        var np = new Position(antenna.x + xDist, antenna.y + yDist);

        while (WithinLimits(np))
        {
            result.Add(np);
            np = new Position(np.x + xDist, np.y + yDist);
        }
        
        return result;
    }

    private static bool WithinLimits(Position position)
    {
        return position.x >= 0 && position.x <= fieldLimit.x && position.y >= 0 && position.y <= fieldLimit.y;
    }
}

internal record Position(int x, int y);