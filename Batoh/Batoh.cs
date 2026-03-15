using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Batoh
{
    internal class Program
    {
        static int   bestProfit;
        static bool[] bestChoice;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            RunFileTests("./Knapsack_testy.txt");

            Console.WriteLine("--- Vlastni vstup ---");
            int[] profit   = ReadInts();
            int[] weight   = ReadInts();
            int   capacity = int.Parse(Console.ReadLine());

            Solve(profit, weight, capacity, out int maxP, out List<int> items);
            Console.WriteLine(maxP);
            Console.WriteLine(string.Join(" ", items));
        }

        static void Solve(int[] profit, int[] weight, int capacity,
                          out int maxProfit, out List<int> items)
        {
            int n     = profit.Length;
            bestProfit = 0;
            bestChoice = new bool[n];

            Backtrack(profit, weight, capacity, new bool[n], 0, 0, 0);

            maxProfit = bestProfit;
            items = new List<int>();
            for (int i = 0; i < n; i++)
                if (bestChoice[i])
                    items.Add(i + 1); // 1-based item numbers
        }

        // Try including or excluding item at index 'i', then recurse.
        static void Backtrack(int[] profit, int[] weight, int capacity,
                              bool[] choice, int i, int curWeight, int curProfit)
        {
            // All items considered — check if this is the best result so far
            if (i == profit.Length)
            {
                if (curProfit > bestProfit)
                {
                    bestProfit = curProfit;
                    Array.Copy(choice, bestChoice, choice.Length);
                }
                return;
            }

            // Option 1: skip item i
            Backtrack(profit, weight, capacity, choice, i + 1, curWeight, curProfit);

            // Option 2: take item i (only if it fits)
            if (curWeight + weight[i] <= capacity)
            {
                choice[i] = true;
                Backtrack(profit, weight, capacity, choice, i + 1,
                          curWeight + weight[i], curProfit + profit[i]);
                choice[i] = false;
            }
        }

        static void RunFileTests(string path)
        {
            string[] lines = File.ReadAllLines(path);

            // Skip the header block at the top of the file
            int i = 0;
            while (i < lines.Length && !char.IsDigit(lines[i].TrimStart().FirstOrDefault()))
                i++;

            int testNum = 1;
            while (i < lines.Length)
            {
                while (i < lines.Length && lines[i].Trim() == "") i++;
                if (i >= lines.Length) break;

                int[] profit   = ParseInts(lines[i++]);
                int[] weight   = ParseInts(lines[i++]);
                int   capacity = int.Parse(lines[i++]);
                int   expProfit = int.Parse(lines[i++].Replace("->", "").Trim());
                List<int> expItems = ParseInts(lines[i++].Replace("->", "")).ToList();

                Solve(profit, weight, capacity, out int maxP, out List<int> items);

                Console.WriteLine("=== Test " + testNum++ + " ===");
                Console.WriteLine("Vypocitano: " + maxP + "  [" + string.Join(" ", items) + "]");
                Console.WriteLine("Ocekavano:  " + expProfit + "  [" + string.Join(" ", expItems) + "]");
                Console.WriteLine(maxP == expProfit ? "OK" : "CHYBA");
                Console.WriteLine();
            }
        }

        static int[] ReadInts() =>
            Console.ReadLine().Trim()
                .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray();

        static int[] ParseInts(string line) =>
            line.Trim()
                .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray();
    }
}
