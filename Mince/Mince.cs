using System;
using System.Collections.Generic;
using System.Text;

namespace Mince
{
    internal class Program
    {
        static int[] coins;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            coins = Array.ConvertAll(Console.ReadLine().Trim().Split(), int.Parse);
            int sum = int.Parse(Console.ReadLine().Trim());

            List<int> current = new List<int>();
            Backtrack(0, sum, current);
        }

        static void Backtrack(int startIndex, int remaining, List<int> current)
        {
            if (remaining == 0)
            {
                if (current.Count == 0)
                    Console.WriteLine("Nepoužije se žádná mince.");
                else
                    Console.WriteLine(string.Join(" ", current));
                return;
            }

            for (int i = startIndex; i < coins.Length; i++)
            {
                if (coins[i] <= remaining)
                {
                    current.Add(coins[i]);
                    Backtrack(i, remaining - coins[i], current); // i not i+1: allow reuse
                    current.RemoveAt(current.Count - 1);
                }
            }
        }
    }
}
