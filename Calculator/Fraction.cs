using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Calculator {
	class Fraction {
		public BigInteger numerator = 0;
		public BigInteger denomonator = 1;
		readonly static string FRACTION_SLASH = "⁄";
		readonly static string[] SUPERSCRIPT = new string[] { "⁰, ¹, ², ³, ⁴, ⁵, ⁶, ⁷, ⁸, ⁹" };
		readonly static string[] SUBSCRIPT = new string[] { "₀, ₁, ₂, ₃, ₄, ₅, ₆, ₇, ₈, ₉" };
		
		public Fraction(BigInteger num, BigInteger den) {
			numerator = num;
			denomonator = den;
		}


		public Fraction(double x, int y = 0x1000) {
			numerator = (long)(x * y);
			denomonator = y;
			Simplify();
		}

		public Fraction(string str, int numBase) {
			if ((numBase > 16) || (numBase < 2)) throw new Exception("Base out of range");
			char[] noFullStops = str.Replace(".", "").ToCharArray();
			for (int i = 0; i < noFullStops.Length; i++) {
				int charValue = CharToDigit(noFullStops[i]);
				if (charValue >= numBase || charValue == -1) throw new Exception("Digit Out Of Range");
				numerator += (int) Math.Pow(numBase, noFullStops.Length - i - 1) * charValue;
			}
			if (str.Contains(".")) {
				denomonator = (int)Math.Pow(numBase, (double)(str.Count() - str.IndexOf('.') - 1));
			}
			Console.WriteLine(numerator);
		}

		public void Simplify() {
			bool changed;

			do {
				changed = false;
				               // The biggest factor is half of the number
				for (int i = 2; i <= denomonator && !changed; i++) { 
					if ((denomonator % i) == 0 && (numerator % i) == 0) {
						changed = true;
						denomonator /= i;
						numerator /= i;
					}
				}
			} while (changed);
		}

		public double AsDouble() {
			return (double)numerator / (double) denomonator;

		}

		public string AsFraction() {
			Simplify();
			return numerator + "/" + denomonator;
		}

		public string AsDecimalString(){
			return Decimal.Divide((decimal)numerator, (decimal)denomonator).ToString();
		}



		//radix point V
		//      011001.101

		
	
		public static string MakeFraction(long a, long b) {
			return a + "/" + b;
		}

		public static int CharToDigit(char x) {
			if ((x >= '0') && (x <= '9')) {
				return x - '0';
			}
			else if ((x >= 'a') && (x <= 'f')) {
				return (x - 'a') + 10;
			}
			else if ((x >= 'A') && (x <= 'F')) {
				return (x - 'A') + 10;
			} else {
				return -1;
			}
		}

		public static char DigitToChar(int x) {
			if ((x >= 0) && (x <= 9)) {
				return (char) ('0' + x);
			}
			else if ((x >= 10) && (x <= 15)) {
				return (char)('A' + x - 10);
			} else {
				throw new Exception("Digit out of range");
			}

		}

		public static bool ValidDigit (char x, int numBase) {
			int digit = CharToDigit(x);
			return digit < numBase;
		}

		
		
		public static Fraction operator +(Fraction a) => a;
		public static Fraction operator -(Fraction a) => new Fraction(-a.numerator, a.denomonator);

		public static Fraction operator +(Fraction a, Fraction b)
			=> new Fraction(a.numerator * b.denomonator + b.numerator * a.denomonator, a.denomonator * b.denomonator);

		public static Fraction operator -(Fraction a, Fraction b)
			=> a + (-b);

		public static Fraction operator *(Fraction a, Fraction b)
			=> new Fraction(a.numerator * b.numerator, a.denomonator * b.denomonator);

		public static Fraction operator /(Fraction a, Fraction b) {
			if (b.numerator == 0) {
				throw new DivideByZeroException();
			}
			return new Fraction(a.numerator * b.denomonator, a.denomonator * b.numerator);
		}

	}
}
