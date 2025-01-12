using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day17s1;

public partial class Day17s1
{
    private static int aRegister;
    private static int bRegister;
    private static int cRegister;
    private static int instructionPointer;

    private static Dictionary<int, Action<int>> instructions = new()
    {
        { 0, adv },
        { 1, bxl },
        { 2, bst },
        { 3, jnz },
        { 4, bxc },
        { 5, output },
        { 6, bdv },
        { 7, cdv }
    };
    
    
    
    public static void Run()
    {
        var input = File.ReadAllText("./Day17s1/input");
        input = input.Replace("\r", "");
        var x = MyRegex().Match(input);
        aRegister = int.Parse(x.Groups[1].Value);
        bRegister = int.Parse(x.Groups[2].Value);
        cRegister = int.Parse(x.Groups[3].Value);
        
        var program = x.Groups[4].Value.Split(",").Select(int.Parse).ToArray();

        while (instructionPointer < program.Length)
        {
             instructions[program[instructionPointer]](program[instructionPointer + 1]);
             instructionPointer += 2;
        }
        
        
    }

    private static int ResolveCombo(int input)
    {
        return input switch
        {
            (>= 0 and <= 3) => input,
            4 => aRegister,
            5 => bRegister,
            6 => cRegister,
            _ => throw new InvalidDataException("invalid combo operator " + input)
        };
    }

    private static void adv(int operand)
    {
        aRegister = (int) (aRegister / Math.Pow(2, ResolveCombo(operand)));
    }

    private static void bxl(int operand)
    {
        bRegister ^= operand;
    }

    private static void bst(int operand)
    {
        bRegister = ResolveCombo(operand) % 8;
    }

    private static void jnz(int operand)
    {
        if (aRegister == 0) return;
        instructionPointer = operand - 2;
    }

    private static void bxc(int operand)
    {
        bRegister ^= cRegister;
    }

    private static void output(int operand)
    {
        Console.Write(ResolveCombo(operand) % 8 + ",");
    }

    private static void bdv(int operand)
    {
        bRegister = (int) (aRegister / Math.Pow(2, ResolveCombo(operand)));
    }

    private static void cdv(int operand)
    {
        cRegister = (int) (aRegister / Math.Pow(2, ResolveCombo(operand)));
    }

    [GeneratedRegex(@"Register A: (\d+)\nRegister B: (\d+)\nRegister C: (\d+)\n\nProgram: (.*)$")]
    private static partial Regex MyRegex();
}