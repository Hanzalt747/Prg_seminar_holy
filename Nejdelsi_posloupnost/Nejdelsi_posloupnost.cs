using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = new UTF8Encoding(false);

        string obsah = File.ReadAllText("vstupy.txt", new UTF8Encoding(false))
            .Replace("\r\n", "\n").TrimStart('\uFEFF').TrimEnd('\n');

        // sekvence jsou oddeleny prazdnym radkem
        string[] bloky = obsah.Split(new string[] { "\n\n" }, StringSplitOptions.None);

        for (int b = 0; b < bloky.Length; b++)
        {
            if (b > 0)
                Console.WriteLine();

            string blok = bloky[b].Trim();

            if (blok == "")
            {
                Console.WriteLine("prazdna posloupnost");
                continue;
            }

            string[] tokeny = blok.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int n = tokeny.Length;
            int[] a = new int[n];
            for (int i = 0; i < n; i++)
                a[i] = int.Parse(tokeny[i]);

            // L[i] = delka nejdelsi rostouci podposloupnosti koncici na indexu i
            int[] L = new int[n];
            int[] prev = new int[n];

            for (int i = 0; i < n; i++)
            {
                L[i] = 1;
                prev[i] = -1;
                for (int j = 0; j < i; j++)
                {
                    if (a[j] < a[i] && L[j] + 1 > L[i])
                    {
                        L[i] = L[j] + 1;
                        prev[i] = j;
                    }
                }
            }

            // najdeme konec nejdelsi podposloupnosti
            int best = 0;
            for (int i = 1; i < n; i++)
                if (L[i] > L[best])
                    best = i;

            // rekonstrukce zpetne pres prev[]
            var vysledek = new List<int>();
            for (int idx = best; idx != -1; idx = prev[idx])
                vysledek.Add(a[idx]);

            vysledek.Reverse();
            Console.WriteLine(string.Join(" ", vysledek));
        }
    }
}
