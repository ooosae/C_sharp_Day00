using System;
using System.IO;

static string[] LoadNamesFromFile(string filePath)
{
    try
    {
        return File.ReadAllLines(filePath);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Something went wrong. Check your input and retry.");
        return new string[0];
    }
}

static string FindClosestName(string userInput, string[] names)
{
    int minDistance = int.MaxValue;
    string closestName = null;

    foreach (string name in names)
    {
        int distance = CalculateLevenshteinDistance(userInput, name);

        if (distance == 0)
            return name;

        if (distance < minDistance)
        {
            minDistance = distance;
            closestName = name;
        }
    }
    
    return minDistance <= 1 ? closestName : null;
}

static void ProvideAdditionalCorrections(string userInput,string[] names)
{
    foreach (string name in names)
    {
        int distance = CalculateLevenshteinDistance(userInput, name);
        if (distance <= 2)
        {
            Console.WriteLine($"Did you mean \"{name}\"? Y/N");
            string response = Console.ReadLine()?.Trim();

            if (response != null && response.ToUpper() == "Y")
            {
                Console.WriteLine($"Hello, {name}!");
                return;
            }
        }
    }

    Console.WriteLine("Your name was not found.");
}

static int CalculateLevenshteinDistance(string s, string t)
{
    int[,] dp = new int[s.Length + 1, t.Length + 1];

    for (int i = 0; i <= s.Length; i++)
    {
        for (int j = 0; j <= t.Length; j++)
        {
            if (i == 0)
            {
                dp[i, j] = j;
            }
            else if (j == 0)
            {
                dp[i, j] = i;
            }
            else
            {
                dp[i, j] = Min(dp[i - 1, j - 1] + (s[i - 1] == t[j - 1] ? 0 : 1),
                               dp[i, j - 1] + 1,
                               dp[i - 1, j] + 1);
            }
        }
    }

    return dp[s.Length, t.Length];
}

static int Min(int a, int b, int c)
{
    return Math.Min(Math.Min(a, b), c);
}

string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../us_names.txt");
string[] names = LoadNamesFromFile(filePath);

Console.WriteLine("Enter name:");
string userInput = Console.ReadLine()?.Trim();

if (string.IsNullOrEmpty(userInput))
{
    Console.WriteLine("Your name was not found.");
    return;
}

string correctedName = FindClosestName(userInput, names);

if (correctedName != null)
{
    Console.WriteLine($"Did you mean \"{correctedName}\"? Y/N");
    string response = Console.ReadLine()?.Trim();

    if (response != null && response.ToUpper() == "Y")
        Console.WriteLine($"Hello, {correctedName}!");
    else
        ProvideAdditionalCorrections(userInput, names);
}
else
{
    Console.WriteLine("Your name was not found.");
}
