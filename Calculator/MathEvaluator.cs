using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator {
	
	class MathEvaluator {
		public int numBase = 10;
		List<object> parsed;
		int i = 0;
		public Fraction calculate(String expression) {
		
			if (expression.ElementAt(0) == '-') expression = "0" + expression;
			expression = expression.Replace(" ", "")
									.Replace("--", "+")
									.Replace("+-", "-")
									.Replace("*-", "*(-1)*")
									.Replace("/-", "*(-1)/");

			parsed = new List<object>();
			StringBuilder prev = new StringBuilder();

			for (int i = 0; i < expression.Length; i++) {
				if (expression[i] == '(') {
					int start = i;
					
					int x = 0;
					bool exit = false;
					for (i++; i < expression.Length & (!exit); i++) {
						if (expression[i] == '(') {
							x++;
						}
						else if (expression[i] == ')') {
							if (x == 0) {
								String last = "";
								if ((i + 1) < expression.Length) last = expression.Substring(i + 1);
								parsed.Add(new MathEvaluator().calculate(expression.Substring(start + 1, i - start - 1)));
								
								expression = expression.Substring(0, start) + last;
								i = start - 2;
								exit = true;
							}
							x--;

						}

					}
				}



				else if (expression[i] == '.' || Fraction.CharToDigit(expression[i]) != -1 ) {
					prev.Append(expression[i]);
				}
				
				else {
					if (!prev.ToString().Equals("")) {
						parsed.Add(new Fraction(prev.ToString(), numBase));
						prev = new StringBuilder();
					}
					parsed.Add(expression[i]);
				}

			}

			if (!prev.ToString().Equals("")) {
				parsed.Add(new Fraction(prev.ToString(), numBase));
			}
			for (i = 0; i < parsed.Count; i++) {
				object current = parsed[i];
				if (current.Equals('^')) process((Fraction a, Fraction b) => new Fraction(Math.Pow(a.AsDouble(), b.AsDouble())));
			}
			for (i = 0; i < parsed.Count; i++) {
				Object current = parsed[i];
				if (current.Equals('*')) process((Fraction a, Fraction b) => a * b);
				else if (current.Equals('/')) process((Fraction a, Fraction b) => a / b);
			}
			for (i = 0; i < parsed.Count(); i++) {
				Object current = parsed.ElementAt(i);
				if (current.Equals('+')) process((Fraction a, Fraction b) => a + b);
				else if (current.Equals('-')) process((Fraction a, Fraction b) => a - b);
			}

			return (Fraction)parsed[0];
		}

		private delegate Fraction Operation(Fraction a, Fraction b);

		private void process(Operation operation) {
			Fraction num1 = (Fraction)parsed[i - 1];
			Fraction num2 = (Fraction)parsed[i + 1];
			parsed[i - 1] = operation(num1, num2);
			parsed.RemoveAt(i + 1);
			parsed.RemoveAt(i);
			i -= 2;
		}
		

	}
	
}
