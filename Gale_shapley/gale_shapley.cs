/*
	Author: Jan Holy
	Date: 28. 9. 2025
*/
using System;
using System.Collections.Generic;

namespace gale_shapley.cs
{
    internal class Program
    {
        static void Main(string[] args)
        {
		int n;
		int arraySize;
		int[,] mainArray;
		Console.WriteLine("Napis preference (nejdriv zeny, pak muzi): ");
		n = int.Parse(Console.ReadLine());
		arraySize = n*n;
		mainArray = new int[arraySize, n+n-1];
		for (int i = 0; i < arraySize; i++) {
			string line = Console.ReadLine();
			for (int j = 0; j < n+n-1; j++) {
				char cell = line[j];
				if (!Char.IsWhiteSpace(cell)) {
					mainArray[i,j] = cell -'0';
				}
			}
		}

		for (int i = 0; i < arraySize; i++) {
            		for (int j = 0; j < n+n-1; j++) {
            			Console.Write(mainArray[i, j]);
            		}
            		Console.WriteLine();
        	}
	}

    }
}
