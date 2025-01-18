using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day22s2;

public class Day22s2
{
    private static List<List<(int, int)>> _monkeyNumbers = [];
    private static int sequencesChecked = 0;
    private static int sequences = 0;
    private static Dictionary<Sequence, int> _bananasDict = new ();
    private static HashSet<Sequence> _alreadySold = new();
    
    public static void Run()
    {
        var lines = File.ReadAllLines("./Day22s2/input");

        var initialSecrets = lines.Select(long.Parse);
        
        foreach (var initialSecret in initialSecrets)
        {
            var result = initialSecret;
            var sequence = new Sequence();
            var previous = 0;
            
            for (var i = 0; i < 2000; i++)
            {
                result = CalculateNewSecret(result);
                var current = (int) (result % 10);
                sequence = sequence.SetCurrent(current - previous);
                previous = current;
                if (i < 4 || _alreadySold.Contains(sequence)) continue;
                if (!_bananasDict.TryAdd(sequence, current))
                {
                    _bananasDict[sequence] += current;
                    _alreadySold.Add(sequence);
                }
            }
            _alreadySold.Clear();
        }

        var money = _bananasDict.Values.Max();
        Console.WriteLine(money);
    }

    private static long CalculateMoney(List<int> sequence)
    {
        var money = _monkeyNumbers.Select((_, index) => SellHidingSpotToMonkey(sequence, index)).Sum();
        sequencesChecked++;
        if (sequencesChecked % 200 == 0)
        {
            Console.WriteLine($"Sequences checked: {sequencesChecked} / {sequences}");
        }
        return money;
    }

    private static int SellHidingSpotToMonkey(List<int> monkeySequence, int monkeyIndex)
    {
        var secretSequence = _monkeyNumbers[monkeyIndex].Select(x => x.Item2).ToList();
        var bananaSequence = _monkeyNumbers[monkeyIndex].Select(x => x.Item1).ToList();
        
        for (var i = 1; i < secretSequence.Count - 4; i++)
        {
            if (secretSequence[i] == monkeySequence[0]
                && secretSequence[i + 1] == monkeySequence[1]
                && secretSequence[i + 2] == monkeySequence[2]
                && secretSequence[i + 3] == monkeySequence[3])
            {
                return bananaSequence[i + 3];
            }
        }
            
        return 0;
    }

    private static long CalculateNewSecret(long secretNumber)
    {
        secretNumber = FirstStep(secretNumber);
        secretNumber = SecondStep(secretNumber);
        secretNumber = ThirdStep(secretNumber);
        
        return secretNumber;
    }

    private static long FirstStep(long secretNumber)
    {
        //var result = secretNumber * 64;
        var result = secretNumber << 6;
        return MixAndPrune(result, secretNumber);
    }
    
    private static long SecondStep(long secretNumber)
    {
        // var result = secretNumber /32;
        var result = secretNumber >> 5;
        return MixAndPrune(result, secretNumber);
    }
    
    private static long ThirdStep(long secretNumber)
    {
        //var result = secretNumber * 2048;
        var result = secretNumber << 11;
        return MixAndPrune(result, secretNumber);
    }

    private static long MixAndPrune(long result, long secretNumber)
    {
        result = secretNumber ^ result;
        //return result % 16777216;
        return result & 16777215;
    }
}

internal record Sequence(int First = 0, int Second = 0, int Third = 0, int Fourth = 0)
{
    public Sequence SetCurrent(int value)
    {
        return new Sequence(Second, Third, Fourth, value);
    }
}