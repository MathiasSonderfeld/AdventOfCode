using System.Text;

namespace AdventOfCode.Day9s2;

public class Day9S2
{
    public static void Run()
    {
        string line = File.ReadAllText("./Day9s2/input");

        var storage = line.Select(c => int.Parse("" + c))
            .Select<int, StorageBlock>((n, i) =>
            {
                if (i % 2 == 0)
                {
                    return new FilePart(i/2, n);
                }
                return new FreeSpace(n);
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
            var freeSpaceIndex = storage.FindIndex(c => c.GetType() == typeof(FreeSpace) && c.Size >= current.Size);
            if (freeSpaceIndex >= i || freeSpaceIndex < 0)
            {
                continue;
            }
            
            var fs = storage[freeSpaceIndex];
            var sizeDiff = fs.Size - current.Size;
            if (sizeDiff == 0)
            {
                storage[freeSpaceIndex]= current;
                storage[i] = fs;
            }
            else
            {
                storage[freeSpaceIndex]= current;
                storage.Insert(freeSpaceIndex+1, new FreeSpace(sizeDiff));
                storage[i+1] = fs with {Size = current.Size};
            }
        }
        
        long chsum = storage.SelectMany(sb => Enumerable.Repeat(sb.GetType() == typeof(FreeSpace) ? 0 : ((FilePart)sb).Id, sb.Size))
            .Select<int, long>((c, i) => c * i)
            .Sum();
        Console.WriteLine(String.Join("", storage));
        Console.WriteLine(chsum);
    }
}

internal record StorageBlock(int Size);

internal record FreeSpace(int Size) : StorageBlock(Size)
{
    public override string ToString()
    {
        return new string('.', Size);
    }
}
internal record FilePart(int Id, int Size): StorageBlock(Size)
{
    public override string ToString()
    {
        return string.Concat(Enumerable.Repeat("" + Id, Size));
    }
}