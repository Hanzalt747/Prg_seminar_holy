/*
	Author: Jan Holy
	Date: 20. 10. 2025
*/
using System;
using System.Collections.Generic;

namespace MathLibrary
{
	internal class Program {
		static void Main(string[] args) {
			Interval domF1 = new Interval(double.NegativeInfinity, double.PositiveInfinity, false, false); // (-∞, ∞)
        		Interval ranF1 = new Interval(-5, 5, true, true);       // [-5, 5]
			MathFunction Linear = new LinearFunction("Linearni rovnice", "f(x) = 2·x + 5", domF1, ranF1);
		
		}
	}

	public struct Interval {
		public double LowerBound { get; }
		public double UpperBound { get; }
		public bool IsLowerClosed { get; }
		public bool IsUpperClosed { get; }

		public Interval(double lowerBound, double upperBound, bool isLowerClosed, bool isUpperClosed) {
        		LowerBound = lowerBound;
        		UpperBound = upperBound;
        		IsLowerClosed = isLowerClosed;
        		IsUpperClosed = isUpperClosed;
    		}

    		public override string ToString() {
        		string leftBracket = IsLowerClosed ? "[" : "(";
        		string rightBracket = IsUpperClosed ? "]" : ")";

        		string lowerText = double.IsNegativeInfinity(LowerBound) ? "-∞" : LowerBound.ToString();
        		string upperText = double.IsPositiveInfinity(UpperBound) ? "∞" : UpperBound.ToString();

        		return $"{leftBracket}{lowerText}, {upperText}{rightBracket}";
    		}
		public bool Contains(double x)
    		{
        		bool lowerOk = IsLowerClosed ? x >= LowerBound : x > LowerBound;
        		bool upperOk = IsUpperClosed ? x <= UpperBound : x < UpperBound;
        		return lowerOk && upperOk;
    		}
	}

	public abstract class MathFunction {
		protected string Name;
		protected string Description;
		protected Interval Domain;
		protected Interval Range;

		public MathFunction(string name, string description, Interval domain, Interval range ) {
			Name = name;
			Description = description;
			Domain = domain;
			Range = range;
		}
		public abstract string ToString();
	}

	//// DEFINOVANI FUNKCI
	public class LinearFunction : MathFunction {
		public MathFunction(string name, string description, Interval domain, Interval range ) : base(name,description,domain,domain,range) {}

		public override string ToString() {
			return $"{Name}, {Description}, {Domain}, {Range}";
		}
	}
}

