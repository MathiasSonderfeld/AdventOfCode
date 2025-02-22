﻿using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day18s2;

public class Day18s2
{
    private const int Bytes = 1024;
    private const int MapSize = 71;
    
    private static HashSet<Position> impassable = [];
    private static readonly Position Start = new(0, 0);
    private static readonly Position End = new(MapSize - 1, MapSize - 1);
    
    private static HashSet<Position> visited = [];
    
    public static void Run()
    {
        var input = File.ReadAllLines("./Day18s2/input");

        for (int i = 0; i < Bytes; i++)
        {
            var positions = input[i].Split(',').Select(int.Parse).ToArray();
            impassable.Add(new Position(positions[0], positions[1]));
        }
        
        for (int i = Bytes; i < input.Length; i++)
        {
            var positions = input[i].Split(',').Select(int.Parse).ToArray();
            impassable.Add(new Position(positions[0], positions[1]));
            if (!IsSolvable())
            {
                Console.WriteLine($"No solution at {positions[0]},{positions[1]}");
                break;
            }
        }
    }

    private static bool IsSolvable()
    {
        visited.Clear();
        
        PriorityQueue<State, int> queue = new();
        queue.Enqueue(new State(Start, 0), 0);

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();
            if (state.Position.Equals(End))
            {
                return true;
            }

            foreach (Direction direction in Enum.GetValues<Direction>())
            {
                var newPos = direction.GetNeighbourInDirection(state.Position);
                if (IsValid(newPos))
                {
                    visited.Add(newPos);
                    var newMoves = state.Moves + 1;
                    queue.Enqueue(new State(newPos, newMoves), newMoves);
                }
            }
        }
        return false;
    }

    private static bool IsValid(Position position)
    {
        return !impassable.Contains(position) && !visited.Contains(position) && position is { X: >= 0 and < MapSize, Y: >= 0 and < MapSize };
    }
}

public record State(Position Position, int Moves);
public record Position(int X, int Y);
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
}
