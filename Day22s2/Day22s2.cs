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
    
    public static void Run()
    {
        var lines = File.ReadAllLines("./Day22s2/input");

        var initialSecrets = lines.Select(long.Parse);
        //List<long> initialSecrets = [123];
        
        foreach (var initialSecret in initialSecrets)
        {
            var list = new List<(int, int)>();
            var result = initialSecret;
            int previous = 0;
            list.Add(((int) (result % 10), 0));
            for (var i = 0; i < 2000; i++)
            {
                result = CalculateNewSecret(result);
                var current = (int) (result % 10);
                list.Add((current, current - previous));
                previous = current;
            }
            _monkeyNumbers.Add(list);
        }

        List<int> changes = [-9, -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        
        var possibleSequences = (
            from change1 in changes 
            from change2 in changes
            from change3 in changes
            from change4 in changes
            select (List<int>)[change1, change2, change3, change4]).ToList();
        
        sequences = possibleSequences.Count;
        
        var money = possibleSequences.AsParallel().Select(CalculateMoney).Max();
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