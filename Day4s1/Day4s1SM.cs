using System.Text.RegularExpressions;

namespace AdventOfCode.Day4s1;


public static class XmasSolver
{
    public static void Solve()
    {

        var inputFile = File.ReadAllLines("./myInput.txt");

        var rows = inputFile;
        var rowsReversed = rows.Select(line => string.Concat(line.Reverse())).ToArray();

        var columns = Enumerable.Repeat("", rows[0].Length).ToArray();
        for (int i = 0; i < columns.Length; i++)
        {
            foreach (var rowText in rows)
            {
                columns[i] += rowText[i];
            }
        }
        var columnsReversed = columns.Select(line => string.Concat(line.Reverse())).ToArray();

        var diagonalsWSNE = new List<string>();

        int startRow = 0;
        int startCol = 0;
        int row = startRow;
        int col = startCol;

        string diagonal = "";
        while (col < columns.Length)
        {
            diagonal += rows[row][col];
            row--;
            col++;

            if (row < 0 || col >= columns.Length)
            {
                if (startRow < rows.Length - 1)
                {
                    startRow++;
                }
                else
                {
                    startCol++;
                }
                row = startRow;
                col = startCol;
                diagonalsWSNE.Add(diagonal);
                diagonal = "";
            }
        }
        var diagonalsWSNEReversed = diagonalsWSNE.Select(line => string.Concat(line.Reverse())).ToArray();

        var diagonalsWNSE = new List<string>();

        startRow = 0;
        startCol = columns.Length - 1;
        row = startRow;
        col = startCol;

        diagonal = "";
        while (row < rows.Length)
        {
            diagonal += rows[row][col];
            row++;
            col++;

            if (row > rows.Length - 1 || col > columns.Length - 1)
            {
                if (startCol > 0)
                {
                    startCol--;
                }
                else
                {
                    startRow++;
                }
                row = startRow;
                col = startCol;
                diagonalsWNSE.Add(diagonal);
                diagonal = "";
            }
        }
        var diagonalsWNSEReversed = diagonalsWNSE.Select(line => string.Concat(line.Reverse())).ToArray();

        var regex = @"XMAS";
        var sumOcc = 0;
        sumOcc += CountOccurences(rows, regex);
        sumOcc += CountOccurences(rowsReversed, regex);
        sumOcc += CountOccurences(columns, regex);
        sumOcc += CountOccurences(columnsReversed, regex);
        sumOcc += CountOccurences(diagonalsWNSE, regex);
        sumOcc += CountOccurences(diagonalsWNSEReversed, regex);
        sumOcc += CountOccurences(diagonalsWSNE, regex);
        sumOcc += CountOccurences(diagonalsWSNEReversed, regex);

        Console.WriteLine($"XMAS Occurences: {sumOcc}");
    }
    static int CountOccurences(IEnumerable<string> textArr, string pattern)
    {
        int sum = 0;
        foreach (var text in textArr)
        {
            var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
            sum += matches.Count;
        }

        Console.WriteLine(sum);

        return sum;
    }
}