using System.Text;

namespace AdventOfCode.Day9s1;

public class Day9S1
{
    public static void Run()
    {
        string line = File.ReadAllText("./Day9s1/input");

        var storage = line.Select(c => int.Parse("" + c))
            .SelectMany<int, StorageBlock>((n, i) =>
            {
                if (i % 2 == 0)
                {
                    return Enumerable.Repeat(new FilePart(i/2), n);
                }
                return Enumerable.Repeat(new FreeSpace(), n);
            })
            .ToList();
            
        Console.WriteLine(String.Join("", storage));

        for (int i = storage.Count - 1; i >= 0; i--)
        {
            var current = storage[i];
            if (current.GetType() == typeof(FreeSpace))
            {
                continue;
            }
            var freeSpaceIndex = storage.FindIndex(c => c.GetType() == typeof(FreeSpace));
            if (freeSpaceIndex >= i || freeSpaceIndex < 0)
            {
                break;
            }
            var fs = storage[freeSpaceIndex];
            storage[freeSpaceIndex]= current;
            storage[i] = fs;
        }
        
        long chsum = storage.Select<StorageBlock, long>((c, i) =>
        {
            if (c.GetType() == typeof(FreeSpace))
            {
                return 0;
            }

            return ((FilePart)c).id * i;
        })
            .Sum();
        Console.WriteLine(String.Join("", storage));
        Console.WriteLine(chsum);
    }
}

internal record StorageBlock;

internal record FreeSpace : StorageBlock
{
    public override String ToString()
    {
        return ".";
    }
}
internal record FilePart(int id): StorageBlock
{
    public override String ToString()
    {
        return "" + id;
    }
}