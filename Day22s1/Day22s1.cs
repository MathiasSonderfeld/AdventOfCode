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

        var initialSecrets = lines.Select(long.Parse);
        
        var results = initialSecrets.Select(secret =>
        {
            var result = secret;
            for (long i = 0; i < 2000; i++)
            {
                result = CalculateNewSecret(result);
            }
            return result;
        });
        Console.WriteLine(string.Join(',', results));
        Console.WriteLine(results.Sum());
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
        var result = secretNumber * 64;
        return MixAndPrune(result, secretNumber);
    }
    
    private static long SecondStep(long secretNumber)
    {
        var result = secretNumber /32;
        return MixAndPrune(result, secretNumber);
    }
    
    private static long ThirdStep(long secretNumber)
    {
        var result = secretNumber * 2048;
        return MixAndPrune(result, secretNumber);
    }

    private static long MixAndPrune(long result, long secretNumber)
    {
        result = secretNumber ^ result;
        return result % 16777216;
    }
}