using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day14S2;

public class Day14S2
{
    private const int Width = 101;
    private const int Height = 103;
    
    private static string christmansCheck = new string('#', 29);
    
    public static void Run()
    {
        var input = File.ReadAllText("./Day14S2/input");
        var robots = Regex.Matches(input, @"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)")
            .Select(m => new Robot(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value))).ToList();

        var iter = 0;
        string vis = "";
        do
        {
            iter++;
            var moves = robots.Select(robot => robot.Move(iter, Width, Height)).ToList();
            vis = GetVisualiszation(moves);
        } while (!CheckPotentialTree(vis));
            
        Console.WriteLine("Move " + iter);
        Console.WriteLine();
        Console.WriteLine(vis);
        Console.WriteLine();
        Console.WriteLine();
    }

    private static bool CheckPotentialTree(string map)
    {
        return map.Contains(christmansCheck);
    }

    private static string GetVisualiszation(List<(int x, int y)> pos, char roboter = '#', char whitspace = ' ')
    {
        string map = "";

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                map += pos.Any(p => p.x == x && p.y == y) ? roboter : whitspace;
            }
            map += "\n";
        }
        
        return map;
    }

    private static int GetSafetyFactor(List<(int x, int y)> robots)
    {
        var safetyFactor = 1;
        var halfWidth = Width / 2;
        var halfHeight = Height / 2;
        
        safetyFactor *= robots.Count(robot => robot.x < halfWidth && robot.y < halfHeight); // Q1
        safetyFactor *= robots.Count(robot => robot.x > halfWidth && robot.y < halfHeight); // Q2 
        safetyFactor *= robots.Count(robot => robot.x < halfWidth && robot.y > halfHeight); // Q3
        safetyFactor *= robots.Count(robot => robot.x > halfWidth && robot.y > halfHeight); // Q4
        
        return safetyFactor;
    }
}

public record Robot(int X, int Y, int VelX, int VelY)
{
    public (int x, int y) Move(int iterations, int width, int height)
    {
        var x = (X + (iterations * VelX)) % width;
        if(x < 0) x += width;
        var y = (Y + (iterations * VelY)) % height;
        if(y < 0) y += height;
        
        return (x, y);
    }
}