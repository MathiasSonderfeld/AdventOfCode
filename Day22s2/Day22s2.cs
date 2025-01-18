using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day22s1;

public class Day22s1
{
    public static void Run()
    {
        var lines = File.ReadAllLines("./Day22s1/input");

        // var initialSecrets = lines.Select(long.Parse);
        List<long> initialSecrets = [123];
        var results = new List<List<(long, long)>>();
        foreach (var initialSecret in initialSecrets)
        {
            var list = new List<(long, long)>();
            var result = initialSecret;
            list.Add((result, result % 10));
            for (var i = 0; i < 2000; i++)
            {
                result = CalculateNewSecret(result);
                list.Add((result, result % 10));
            }
            results.Add(list);
        }
        //Console.WriteLine(string.Join(',', results));
        Console.WriteLine(results.Select(res => res.Last()).Sum());
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