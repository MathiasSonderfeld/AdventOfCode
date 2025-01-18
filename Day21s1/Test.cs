namespace AdventOfCode.Day21s1;

internal class Test
{
    private string[] codes;
    private (char c, int x, int y)[] numKeypad;
    private (char c, int x, int y)[] dirKeypad;
 
    public Test(string test)
    {
        codes = test.Replace("\r\n", "\n").Split('\n');
        numKeypad = (new string[] { "789", "456", "123", " 0A" })
        .SelectMany((row, y) => row.Select((c, x) => (c, x, y)))
        .Where(p => p.c != ' ')
        .ToArray();
        dirKeypad = new string[] { " ^A", "<v>" }
        .SelectMany((row, y) => row.Select((c, x) => (c, x, y)))
        .Where(p => p.c != ' ')
        .ToArray();
    }
    
    internal string Run()
    {
        var score = 0L;
        var cache = new Dictionary<(char a, char b, int robots), long>();
        foreach (var code in codes)
        {
            var lower = long.MaxValue;
            var moves = NumpadRobotMoves(code, 'A');
            foreach (var move in moves)
            {
                var pairs = move.Select((c, i) => (a: i == 0 ? 'A' : move[i - 1], b: c)).ToArray();
                var sum = pairs.Select(p => Cost(p, 2, cache)).Sum();
                if (sum < lower)
                    lower = sum;
            }
            score += lower * int.Parse(code[..^1]);
        }
        return score.ToString();
    }
    IEnumerable<string> NumpadRobotMoves(string code, char startPosition)
    {
        var end = code[0];
        var moves = Moves(startPosition, end, numKeypad);
        foreach (var move in moves)
            if (code.Length == 1)
                return moves.Select(m => m + 'A');
            else
                return moves.SelectMany(m => NumpadRobotMoves(code[1..], end).Select(r => m + 'A' + r));
        return Enumerable.Empty<string>();
    }
 
 
    private long Cost((char startLabel, char targetLabel) route, int robots, Dictionary<(char a, char b, int robots), long> cache)
    {
        if (cache.TryGetValue((a: route.startLabel, b: route.targetLabel, robots), out var inCache))
            return inCache;
        var lower = long.MaxValue;
        var moves = Moves(route.startLabel, route.targetLabel, dirKeypad);
        foreach (var move in moves)
        {
            var pairs = move.Append('A').Select((c, i) => (a: i == 0 ? 'A' : move[i - 1], b: c)).ToArray();
            var sum = pairs.Select(p => robots == 1 ? 1 : Cost(p, robots - 1, cache)).Sum();
            if (sum < lower)
                lower = sum;
        }
        cache.Add((a: route.startLabel, b: route.targetLabel, robots), lower);
        return lower;
    }
 
    IEnumerable<string> Moves(char start, char end, (char c, int x, int y)[] keypad)
    {
        var res = new List<string>();
        var startp = keypad.First(p => p.c == start);
        var endp = keypad.First(p => p.c == end);
        var stack = new Stack<(int x, int y, List<char> path)>();
        stack.Push((startp.x, startp.y, new List<char>()));
        while (stack.Count > 0)
        {
            var (x, y, path) = stack.Pop();
            if (x == endp.x && y == endp.y)
            {
                yield return new string(path.ToArray());
                continue;
            }
            if (!keypad.Any(p => p.x == x && p.y == y))
                continue;
            if (x < endp.x)
                stack.Push((x + 1, y, path.Append('>').ToList()));
            if (x > endp.x)
                stack.Push((x - 1, y, path.Append('<').ToList()));
            if (y < endp.y)
                stack.Push((x, y + 1, path.Append('v').ToList()));
            if (y > endp.y)
                stack.Push((x, y - 1, path.Append('^').ToList()));
        }
    }
}