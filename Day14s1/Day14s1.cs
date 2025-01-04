using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day14S1;

public class Day14S1
{
    private const int Width = 101;
    private const int Height = 103;
    
    public static void Run()
    {
        var input = File.ReadAllText("./Day14S1/input");
        var robots = Regex.Matches(input, @"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)")
            .Select(m => new Robot(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value))).ToList();
            
        var moves = robots.Select(robot => robot.Move(100, Width, Height)).ToList();

        Console.WriteLine(GetSafetyFactor(moves));
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