using System;
using System.Collections.Generic;
using System.Globalization;

namespace MathFunctions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<MathFunction> functions = new List<MathFunction>()
            {
                new LinearFunction(2, 3),
                new AbsoluteLinearFunction(-1.5, 2),
                new RationalLinearFunction(1, -2, 3, 4),
                new QuadraticFunction(1, -2, 1) // (x-1)^2
            };

            double x = 2;
            foreach (var f in functions)
            {
                // Každá funkce vypíše základní údaje a případné doplňující poznámky
                f.PrintInfo();
                Console.WriteLine($"f({x}) = {f.Calculate(x)}");

                var df = f as IDifferentiable;
                if (df != null)
                    Console.WriteLine(df.OutputDerivative());

                var inv = f as IInvertible;
                if (inv != null)
                    Console.WriteLine(inv.OutputInversion());

                Console.WriteLine(new string('-', 60));
            }

            Console.WriteLine("Hotovo.");
        }
    }

    // Jednoduchá reprezentace intervalu
    struct Interval
    {
        public double LowerBoundValue { get; private set; }
        public double UpperBoundValue { get; private set; }
        public char LowerBoundBracket { get; private set; }
        public char UpperBoundBracket { get; private set; }

        public Interval(char lbb, double lbv, double ubv, char ubb)
        {
            LowerBoundBracket = lbb;
            LowerBoundValue = lbv;
            UpperBoundValue = ubv;
            UpperBoundBracket = ubb;
        }

        public override string ToString()
        {
            // Elegantní zápis intervalu včetně otevřenosti/zavřenosti hranic
            string lo = double.IsNegativeInfinity(LowerBoundValue) ? "-inf" : LowerBoundValue.ToString();
            string hi = double.IsPositiveInfinity(UpperBoundValue) ? "+inf" : UpperBoundValue.ToString();
            return string.Format("{0}{1},{2}{3}", LowerBoundBracket, lo, hi, UpperBoundBracket);
        }
    }

    // Rozhraní/mezioblicej :)
    interface IDifferentiable
    {
        string OutputDerivative();
    }

    interface IInvertible
    {
        string OutputInversion();
    }

    // Abstraktní základ
    abstract class MathFunction
    {
        public string Name { get; private set; }
        public string Description { get; protected set; }
        public Interval Domain { get; protected set; }
        public Interval Range { get; protected set; }

        protected MathFunction(string name, string description)
        {
            Name = name;
            Description = description;

            // výchozí obory: (-inf, +inf)
            Domain = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')');
            Range  = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')');
        }

        public abstract double Calculate(double x);

        public virtual void PrintInfo()
        {
            Console.WriteLine(Name + ": " + Description);
            Console.WriteLine("D(f) = " + Domain);
            Console.WriteLine("H(f) = " + Range);
        }
    }

    // 1) Lineární funkce
    class LinearFunction : MathFunction, IInvertible, IDifferentiable
    {
        private readonly double a, b;

        public LinearFunction(double a, double b)
            : base("Lineární funkce", string.Format("f(x) = {0}x + {1}", a, b))
        {
            this.a = a;
            this.b = b;
            Domain = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')');
            Range  = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')');
        }

        public override double Calculate(double x) { return a * x + b; }

        public string OutputDerivative() { return "f'(x) = " + a; }

        public string OutputInversion()
        {
            if (a == 0) return "Inverze: neexistuje (a = 0 ⇒ funkce je konstantní).";
            // f^{-1}(x) = (x - b)/a
            return string.Format("f^(-1)(x) = (x - {0}) / {1} ;  D(f^(-1)) = H(f) = {2}", b, a, Range.ToString());
        }
    }

    // 2) Lineární s absolutní hodnotou
    class AbsoluteLinearFunction : MathFunction, IDifferentiable
    {
        private readonly double a, b;

        public AbsoluteLinearFunction(double a, double b)
            : base("Lineární s absolutní hodnotou", string.Format("f(x) = |{0}x + {1}|", a, b))
        {
            this.a = a;
            this.b = b;

            Domain = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')');

            if (a == 0)
            {
                double c = Math.Abs(b);
                Range = new Interval('[', c, c, ']'); // jednoprvkový interval
            }
            else
            {
                Range = new Interval('[', 0, double.PositiveInfinity, ')');
            }
        }

        public override double Calculate(double x) { return Math.Abs(a * x + b); }

        public override void PrintInfo()
        {
            base.PrintInfo();
            if (a != 0)
            {
                double x0 = -b / a;
                Console.WriteLine("Poznámka: bod zlomu v x = " + x0 + " (nederivovatelný).");
            }
        }

        public string OutputDerivative()
        {
            if (a == 0) return "f'(x) = 0 (konstantní funkce, všude derivovatelná).";
            double x0 = -b / a;
            string primaryCmp = a > 0 ? ">" : "<";
            string secondaryCmp = a > 0 ? "<" : ">";
            return string.Format("f'(x) = {{ {0} pro x {1} {2};  {3} pro x {4} {2} }}; v x = {2} neexistuje.",
                a, primaryCmp, x0, -a, secondaryCmp);
        }
    }

    // 3) Lineární lomená f(x) = (ax + b) / (cx + d)
    class RationalLinearFunction : MathFunction, IInvertible, IDifferentiable
    {
        private readonly double a, b, c, d;

        public RationalLinearFunction(double a, double b, double c, double d)
            : base("Lineární lomená funkce", string.Format("f(x) = ({0}x + {1}) / ({2}x + {3})", a, b, c, d))
        {
            this.a = a; this.b = b; this.c = c; this.d = d;
            Domain = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')');
            Range  = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')');
        }

        public override double Calculate(double x)
        {
            double denom = c * x + d;
            if (denom == 0)
                throw new DivideByZeroException("f není definována v x = " + (-d / c) + ".");
            return (a * x + b) / denom;
        }

        public override void PrintInfo()
        {
            Console.WriteLine(Name + ": " + Description);

            double det = a * d - b * c;

            if (c != 0)
            {
                double xV = -d / c;
                double yH = a / c;

                Console.WriteLine("D(f) = (-inf,+inf) \\ { " + xV + " }");

                if (det != 0)
                    Console.WriteLine("H(f) = (-inf,+inf) \\ { " + yH + " }");
                else
                    Console.WriteLine("H(f) = { " + yH + " } (konstantní na oboru definice).");

                Console.WriteLine("Vertikální asymptota: x = " + xV + " (v tomto bodě není f definována).");
                if (det != 0)
                    Console.WriteLine("Horizontální asymptota: y = " + yH + ".");
            }
            else
            {
                Console.WriteLine("D(f) = " + Domain);
                Console.WriteLine("H(f) = " + Range);
                Console.WriteLine("Poznámka: c = 0 ⇒ funkce je ve skutečnosti lineární.");
            }
        }

        public string OutputDerivative()
        {
            // f'(x) = (ad - bc) / (cx + d)^2
            double det = a * d - b * c;
            return string.Format("f'(x) = ({0}*{1} - {2}*{3}) / ({4}x + {5})^2 = {6} / ({4}x + {5})^2",
                a, d, b, c, c, d, det);
        }

        public string OutputInversion()
        {
            // Inverze existuje pokud ad - bc ≠ 0. Potom f^{-1}(x) = (d x - b) / (-c x + a)
            double det = a * d - b * c;
            if (det == 0)
                return "Inverze: neexistuje (ad - bc = 0 ⇒ zobrazení není prosté).";

            string inv = $"f^(-1)(x) = ({d}x - {b}) / ({-c}x + {a})";
            string note;
            if (c != 0)
            {
                double excluded = a / c;
                note = " ; D(f^(-1)) = R \\ { " + excluded + " } (aby jmenovatel " + (-c) + "x + " + a + " ≠ 0).";
            }
            else
            {
                note = " ; D(f^(-1)) = R (protože jmenovatel je konstantní nenulový).";
            }
            return inv + note;
        }
    }

    // 4) Kvadratická funkce
    class QuadraticFunction : MathFunction, IDifferentiable
    {
        private readonly double a, b, c;

        public QuadraticFunction(double a, double b, double c)
            : base("Kvadratická funkce", string.Format("f(x) = {0}x^2 + {1}x + {2}", a, b, c))
        {
            this.a = a; this.b = b; this.c = c;

            Domain = new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')');
            Range  = ComputeRange();
        }

        private Interval ComputeRange()
        {
            if (a == 0)
                return new Interval('(', double.NegativeInfinity, double.PositiveInfinity, ')');

            // Vrchol paraboly určuje, zda je minimum či maximum na celé R
            double x0 = -b / (2 * a);
            double f0 = a * x0 * x0 + b * x0 + c;
            if (a > 0)
                return new Interval('[', f0, double.PositiveInfinity, ')');
            else
                return new Interval('(', double.NegativeInfinity, f0, ']');
        }

        public override double Calculate(double x) { return a * x * x + b * x + c; }

        public override void PrintInfo()
        {
            base.PrintInfo();
            if (a == 0)
            {
                Console.WriteLine("Poznámka: a = 0 ⇒ funkce je ve skutečnosti lineární.");
            }
            else
            {
                double x0 = -b / (2 * a);
                double y0 = Calculate(x0);
                Console.WriteLine(string.Format("Vrchol: V[{0}, {1}] ; osa souměrnosti: x = {0}", x0, y0));
                double D = b * b - 4 * a * c;
                string info = (D < 0) ? "(reálné kořeny neexistují)" :
                              (D == 0) ? "(dvojnásobný kořen)" : "(dva reálné kořeny)";
                Console.WriteLine("Diskriminant: D = " + D + " " + info);
            }
        }

        public string OutputDerivative()
        {
            return string.Format("f'(x) = {0}x + {1}", 2 * a, b);
        }
    }
}
