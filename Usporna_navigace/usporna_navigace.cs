/*
    Author: Jan Holy
    Date: 28. 9. 2025
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace gale_shapley.cs
{
    /// <summary>
    /// Reprezentace hrany grafu.
    /// </summary>
    public class Edge
    {
        public int From;   // odkud
        public int To;     // kam
        public int Weight; // váha
	public int Flag;

        public Edge(int from, int to, int weight, int flag)
        {
            this.From = from;
            this.To = to;
            this.Weight = weight;
	    this.Flag = flag;
        }
    }

    /// <summary>
    /// Zabalený načtený vstup.
    /// </summary>
    public class GraphInput
    {
        public int NodeCount;       // n
        public int EdgeCount;       // m
        public List<Edge> Edges;    // seznam hran
        public int StartNode;       // s
        public int GoalNode;        // t
    }

    /// <summary>
    /// Načte graf ve formátu:
    /// n m
    /// m řádků: u v w
    /// s t
    /// Ignoruje prázdné řádky a komentáře začínající '#' nebo '//'.
    /// </summary>
    public static class InputReader
    {
        public static GraphInput ReadGraphWithStartGoal()
        {
            return ReadGraphWithStartGoal(Console.In);
        }

        public static GraphInput ReadGraphWithStartGoal(string filePath)
        {
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                return ReadGraphWithStartGoal(streamReader);
            }
        }

        public static GraphInput ReadGraphWithStartGoal(TextReader textReader)
        {
            FastScanner scanner = new FastScanner(textReader);

            int nodeCount = scanner.NextInt();
            int edgeCount = scanner.NextInt();

            List<Edge> edges = new List<Edge>(edgeCount);
            for (int i = 0; i < edgeCount; i++)
            {
                int from = scanner.NextInt();
                int to = scanner.NextInt();
                int weight = scanner.NextInt();
		int flag = scanner.NextInt();
                edges.Add(new Edge(from, to, weight, flag));
            }

            int startNode = scanner.NextInt();
            int goalNode = scanner.NextInt();

            GraphInput input = new GraphInput();
            input.NodeCount = nodeCount;
            input.EdgeCount = edgeCount;
            input.Edges = edges;
            input.StartNode = startNode;
            input.GoalNode = goalNode;

            return input;
        }
    }

    /// <summary>
    /// Jednoduchý rychlý skener pro konzoli/soubor.
    /// </summary>
    public class FastScanner
    {
        private readonly TextReader reader;
        private string currentLine;
        private int currentIndex;

        public FastScanner(TextReader textReader)
        {
            this.reader = textReader;
            this.currentLine = null;
            this.currentIndex = 0;
        }

        public string Next()
        {
            while (true)
            {
                if (this.currentLine == null || this.currentIndex >= this.currentLine.Length)
                {
                    string line = this.reader.ReadLine();
                    if (line == null)
                    {
                        throw new EndOfStreamException("Neočekávaný konec vstupu.");
                    }

                    if (IsBlankOrComment(line))
                    {
                        // přeskoč prázdné/komentované řádky
                        continue;
                    }

                    this.currentLine = line;
                    this.currentIndex = 0;
                }

                // přeskoč mezery
                while (this.currentIndex < this.currentLine.Length &&
                       char.IsWhiteSpace(this.currentLine[this.currentIndex]))
                {
                    this.currentIndex++;
                }

                if (this.currentIndex >= this.currentLine.Length)
                {
                    this.currentLine = null;
                    continue;
                }

                int tokenStart = this.currentIndex;
                while (this.currentIndex < this.currentLine.Length &&
                       !char.IsWhiteSpace(this.currentLine[this.currentIndex]))
                {
                    this.currentIndex++;
                }

                return this.currentLine.Substring(tokenStart, this.currentIndex - tokenStart);
            }
        }

        public int NextInt()
        {
            return int.Parse(Next(), CultureInfo.InvariantCulture);
        }

        public long NextLong()
        {
            return long.Parse(Next(), CultureInfo.InvariantCulture);
        }

        public double NextDouble()
        {
            return double.Parse(Next(), CultureInfo.InvariantCulture);
        }

        private static bool IsBlankOrComment(string s)
        {
            int i = 0;
            while (i < s.Length && char.IsWhiteSpace(s[i])) i++;
            if (i >= s.Length) return true;

            char c = s[i];
            if (c == '#') return true;
            if (c == '/' && i + 1 < s.Length && s[i + 1] == '/') return true;

            return false;
        }
    }

    // Vstupní bod programu MUSÍ být uvnitř třídy.
    public class Program
    {
        public static void Main(string[] args)
        {
            GraphInput input = InputReader.ReadGraphWithStartGoal("in_2.txt");
            Console.WriteLine(string.Format(
                "M = {0}, S = {1}, Start = {2}, Goal = {3}",
                input.NodeCount, input.EdgeCount, input.StartNode, input.GoalNode));

        }
    }
}

