namespace AdventOfCode.Day16s1;

using System;
using System.Collections.Generic;

public class ReindeerMazeSolver
{
    // Directions: 0 = East, 1 = North, 2 = West, 3 = South
    static readonly int[,] Directions = new int[,] { { 0, 1 }, { -1, 0 }, { 0, -1 }, { 1, 0 } };  // (dy, dx) for [East, North, West, South]

    public static int SolveMaze(string[] maze)
    {
        int rows = maze.Length;
        int cols = maze[0].Length;

        (int r, int c) start = (-1, -1);
        (int r, int c) end = (-1, -1);

        // Find start 'S' and end 'E' positions
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (maze[r][c] == 'S') start = (r, c);
                if (maze[r][c] == 'E') end = (r, c);
            }
        }

        // Priority Queue for BFS: (cost, row, col, direction)
        // Min-heap based on cost
        var pq = new PriorityQueue<(int, int, int, int), int>();
        pq.Enqueue((0, start.r, start.c, 0), 0);  // Starting at S, facing East (0)

        // Visited array for each position and direction (visited[row, col, direction])
        var visited = new int[rows, cols, 4];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                for (int d = 0; d < 4; d++)
                    visited[i, j, d] = int.MaxValue;

        visited[start.r, start.c, 0] = 0;  // Starting position with 0 cost

        // Perform BFS
        while (pq.Count > 0)
        {
            var (cost, r, c, dir) = pq.Dequeue();

            // If we reach the end, return the cost
            if ((r, c) == end)
                return cost;

            // Explore 3 possible actions:
            // 1. Move forward
            // 2. Rotate clockwise (90 degrees)
            // 3. Rotate counterclockwise (90 degrees)
            for (int i = 0; i < 3; i++)
            {
                int newCost = cost;
                int newR = r, newC = c, newDir = dir;

                if (i == 0)
                {
                    // Move forward
                    newR += Directions[dir, 0];
                    newC += Directions[dir, 1];

                    // If the new position is inside bounds and not a wall
                    if (newR >= 0 && newR < rows && newC >= 0 && newC < cols && maze[newR][newC] != '#')
                    {
                        newCost = cost + 1;  // Moving forward costs 1 point
                        newDir = dir;
                    }
                    else
                        continue; // Invalid move
                }
                else if (i == 1)
                {
                    // Rotate clockwise (90 degrees)
                    newDir = (dir + 1) % 4;
                    newCost = cost + 1000;  // Rotation costs 1000 points
                }
                else
                {
                    // Rotate counterclockwise (90 degrees)
                    newDir = (dir + 3) % 4;
                    newCost = cost + 1000;  // Rotation costs 1000 points
                }

                // Only process the state if it results in a lower cost
                if (newCost < visited[newR, newC, newDir])
                {
                    visited[newR, newC, newDir] = newCost;
                    pq.Enqueue((newCost, newR, newC, newDir), newCost);
                }
            }
        }

        return -1;  // Return -1 if no path exists (although the problem guarantees there is one)
    }

    public static void Main()
    {
        string[] maze = File.ReadAllLines("./Day15s2/input");

        int result = SolveMaze(maze);
        Console.WriteLine("Minimum score: " + result);
    }
}