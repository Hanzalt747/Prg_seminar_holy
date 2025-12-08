using System;
using System.Collections.Generic;
using System.IO;

namespace Test
{
    class Program
    {
        struct Coord
        {
            public int X;
            public int Y;

            public Coord(int x, int y)
            {
                X = x;
                Y = y;
            }

            public bool InBounds()
            {
                return X >= 0 && Y >= 0 && X < 8 && Y < 8;
            }
        }

        static void Main(string[] args)
        {
            for (int i = 1; i <= 4; i++)
            {
		// UNIXovy file system
                using (StreamReader sr = new StreamReader($"./vstupni_soubory/{i}.txt"))
                {
                    int pocetPrekazek = int.Parse(sr.ReadLine());
                    HashSet<Coord> prekazky = new HashSet<Coord>();

                    for (int p = 0; p < pocetPrekazek; p++)
                    {
                        string radekPrekazky = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(radekPrekazky)) { p--; continue; }
                        prekazky.Add(NactiSouradnici(radekPrekazky));
                    }

                    Coord start = NactiSouradnici(sr.ReadLine());
                    Coord cil = NactiSouradnici(sr.ReadLine());

                    int cesta = NajdiCestu(start, cil, prekazky);

                    if (cesta == 0)
                    {
                        Console.WriteLine("Cesta neexistuje.");
                    }
                    else
                    {
                        Console.WriteLine($"Pocet tahu: {cesta - 1}");
                    }
                }
            }
        }

        // Vstupni radek ma format "x y"
        static Coord NactiSouradnici(string radek)
        {
            string[] casti = radek.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int x = int.Parse(casti[0]);
            int y = int.Parse(casti[1]);
            return new Coord(x, y);
        }

        // Nejkratsi cesta kone
        static int NajdiCestu(Coord start, Coord cil, HashSet<Coord> prekazky)
        {
            if (start.X == cil.X && start.Y == cil.Y) return 1;

	    // Vsechny mozne kroky/skoky kone
            int[,] skoky = new int[,]
            {
                { 2, 1 }, { 1, 2 }, { -1, 2 }, { -2, 1 },
                { -2, -1 }, { -1, -2 }, { 1, -2 }, { 2, -1 }
            };
	    // Abych se neopakoval
            int[,] vzdalenost = new int[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    vzdalenost[i, j] = -1;

	    // Fronta pro zkouseni vsech tahu
            Queue<Coord> fronta = new Queue<Coord>();

            vzdalenost[start.X, start.Y] = 0;
            fronta.Enqueue(start);

            while (fronta.Count > 0)
            {

                Coord poz = fronta.Dequeue();

                for (int i = 0; i < skoky.GetLength(0); i++)
                {
                    Coord dalsi = new Coord(poz.X + skoky[i, 0], poz.Y + skoky[i, 1]);
                    if (!dalsi.InBounds()) continue;
                    if (vzdalenost[dalsi.X, dalsi.Y] != -1) continue;
                    if (prekazky.Contains(dalsi)) continue;

                    vzdalenost[dalsi.X, dalsi.Y] = vzdalenost[poz.X, poz.Y] + 1;
                    if (dalsi.X == cil.X && dalsi.Y == cil.Y)
                    {
                        // +1 protoze vracime pocet vrcholu na ceste (start i cil)
                        return vzdalenost[dalsi.X, dalsi.Y] + 1;
                    }
                    fronta.Enqueue(dalsi);
                }
            }
	    return 0;
        }
    }
}
