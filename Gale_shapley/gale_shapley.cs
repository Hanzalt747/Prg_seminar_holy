/*
	Author: Jan Holy
	Date: 12. 10. 2025
*/
using System;
using System.Collections.Generic;

class StableMarriage
{
    public static void Main()
    {
        int n = int.Parse(Console.ReadLine());

        // Preference žen (womenPreferences[w][i] = i-tý muž v seznamu ženy w)
        int[][] womenPreferences = new int[n][];
        for (int i = 0; i < n; i++)
        {
            string[] line = Console.ReadLine().Split();
            womenPreferences[i] = Array.ConvertAll(line, int.Parse);
        }

        // Preference mužů (muž -> (žena -> priorita))
        int[][] menPreferences = new int[n][];
        for (int i = 0; i < n; i++)
        {
            string[] line = Console.ReadLine().Split();
            menPreferences[i] = Array.ConvertAll(line, int.Parse);
        }

        // Mapa mužských preferencí pro rychlý přístup (menRank[m][w] = priorita ženy w pro muže m)
        int[][] menRank = new int[n][];
        for (int m = 0; m < n; m++)
        {
            menRank[m] = new int[n + 1]; // 1-based
            for (int rank = 0; rank < n; rank++)
            {
                int woman = menPreferences[m][rank];
                menRank[m][woman] = rank;
            }
        }

        int[] womanNextProposal = new int[n]; // Index dalšího muže, kterého žena osloví
        int[] manPartner = new int[n];        // manPartner[m] = index ženy + 1
        Array.Fill(manPartner, -1);           // -1 značí, že muž je volný

        bool[] engaged = new bool[n];         // Je žena zadaná?

        Queue<int> freeWomen = new Queue<int>();
        for (int i = 0; i < n; i++) freeWomen.Enqueue(i); // všechny ženy jsou na začátku volné

        while (freeWomen.Count > 0)
        {
            int w = freeWomen.Dequeue();
            int nextManIndex = womanNextProposal[w]++;
            int m = womenPreferences[w][nextManIndex] - 1; // preference jsou 1-based

            if (manPartner[m] == -1)
            {
                manPartner[m] = w;
                engaged[w] = true;
            }
            else
            {
                int currentWoman = manPartner[m];
                if (menRank[m][w + 1] < menRank[m][currentWoman + 1])
                {
                    manPartner[m] = w;
                    engaged[w] = true;
                    engaged[currentWoman] = false;
                    freeWomen.Enqueue(currentWoman);
                }
                else
                {
                    freeWomen.Enqueue(w);
                }
            }
        }

        // Výstup: pro každou ženu, zjistíme ke kterému muži byla přiřazena
        int[] womanToMan = new int[n]; // index ženy -> muž (1-based)
        for (int m = 0; m < n; m++)
        {
            int w = manPartner[m];
            womanToMan[w] = m + 1;
        }

        foreach (int man in womanToMan)
        {
            Console.WriteLine(man);
        }
    }
}

