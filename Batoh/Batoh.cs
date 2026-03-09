using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Batoh
{
    internal class Program
    {
        static void Main(string[] args)
        {
		Console.OutputEncoding = Encoding.UTF8;
	
		int[] foundBestProfit = new int[];
	
		//Vypsani a vypocitani testu
		ReadAndCalcTXT("./Knapsack_testy.txt")

		//** INPUT **//
		//Profit
		string[] line = Console.ReadLine().Split();
		for (int i = 0; i < line.length(); i++) {
			profit[i] = Array.ConvertAll(line.int.Parse);
		}

		//Hmotnost
		string[] line = Console.ReadLine().Split();	
		for (int i = 0; i < line.length(); i++) {
			weight[i] = Array.ConvertAll(line.int.Parse);
		}

		int maxCapacity = int.Parse(Console.ReadLine());

		CalcKnapsack(profit, weight, maxCapacity);

        }

	private static void ReadAndCalcTXT(string path) {
		using (StreamReader sr = new StreamReader(path)) {
			int profit = int.Parse(sr.ReadLine());
		}
	}

	private int[] CalcKnapsack(int[] p, int[] w, int c, int curWeight, int curProfit) {
		if (curWeight >= c) {
			return curProfit
		} else {
			for (int i = 0; i < p.length()) {
				p[i] += curProfit;
				w[i] += curWeight;
				p.RemoveAt(i);
				w.RemoveAt(i);
				foundBestProfit = CalcKapsack(p,w,c,curWeight,curProfit)
			}
		}
	}

    }
}
