using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whenever_in_C_Sharp
{
	class Command
	{
		private static readonly string print = "print";
		public string? deferString { get; set; }
		public string? againString { get; set; }
		public string? actionString { get; set; }
		public int numToDo = 1;

		public void execute(int lineNumber)
		{
			try
			{
				if (this.evaluateBool(deferString)) return;
				this.action();
				if (!this.evaluateBool(againString)) WheneverCode.code.changeNumToDo(lineNumber, -1);
			}
			catch (SyntaxErrorException e)
			{
				Console.WriteLine("Syntax error at line " + lineNumber.ToString());
				Console.WriteLine(e.Message);
				Environment.Exit(1);
			}
		}

		private bool evaluateBool(string s)
		{
			if (s == null || s.Equals("false")) return false;
			if (s.Equals("true")) return true;
			if (WheneverCode.traceLevel > 3) Console.WriteLine("Bool Eval: " + s);

			// Check for numerical comparisons
			int less = s.IndexOf("<");
			int greater = s.IndexOf(">");
			int equals = s.IndexOf("==");
			int notequals = s.IndexOf("!=");
			int i = -1;
			if (less >= 0 && (i < 0 || less < i)) i = less;
			if (greater >= 0 && (i < 0 || greater < i)) i = greater;
			if (equals >= 0 && (i < 0 || equals < i)) i = equals;
			if (notequals >= 0 && (i < 0 || notequals < i)) i = notequals;

			// I have no idea what this does, I'm just hoping this works
			if (i >= 0)
			{
				int lside = Math.Max(s.Substring(0, i).LastIndexOf("&&"), s.Substring(0, i).LastIndexOf("||"));
				int rside1 = s.IndexOf("&&", 1);
				int rside2 = s.IndexOf("||", i);
				int rside = -1;
				if (rside1 >= 0 && rside2 >= 0)
					rside = Math.Min(rside1, rside2);
				if (rside1 >= 0 && rside2 < 0)
					rside = rside1;
				if (rside1 < 0 && rside2 >= 0)
					rside = rside2;
				string lstring = "";
				string rstring = "";
				string lcomp;
				string rcomp;
				string op = s.Substring(i, i + 2 - i);
				// extract left comparison operand and left boolean leftover string if necessary
				if (lside >= 0)
				{
					lside += 2;
					lstring = s.Substring(0, lside);
					lcomp = s.Substring(lside, i - lside).Trim();
				}
				else
					lcomp = s.Substring(0, i).Trim();
				// add different amount to index to extract right operand for different length operators
				if (op.Equals("<=") || op.Equals(">=") || op.Equals("==") || op.Equals("!="))
					i += 2;
				else // simple < or >
					i++;
				// extract right comparison operand and right boolean leftover string if necessary
				if (rside >= 0)
				{
					rstring = s.Substring(rside);
					rcomp = s.Substring(i, rside - i).Trim();
				}
				else
					rcomp = s.Substring(i).Trim();
				bool comp;
				// TODO figure out what evaluate integer does
				if (op.Equals("<=")) comp = evaluateInt(lcomp) <= evaluateInt(rcomp);
				else if (op.Equals(">=")) comp = evaluateInt(lcomp) >= evaluateInt(rcomp);
				else if (op.Equals("==")) comp = evaluateInt(lcomp) == evaluateInt(rcomp);
				else if (op.Equals("!=")) comp = evaluateInt(lcomp) != evaluateInt(rcomp);
				// https://stackoverflow.com/questions/3581741/c-sharp-equivalent-to-javas-charat
				else if (op[0] == '<') comp = evaluateInt(lcomp) < evaluateInt(rcomp);
				else comp = evaluateInt(lcomp) > evaluateInt(rcomp); // >
																	 // I hope the comp.ToString() works :)
				return evaluateBool(lstring + comp.ToString() + rstring);
			}

			i = s.IndexOf("||");
			if (i >= 0)
				return evaluateBool(s.Substring(0, i).Trim()) || evaluateBool(s.Substring(i + 2).Trim());
			// we must have an and operator
			i = s.IndexOf("&&");
			if (i >= 0) return evaluateBool(s.Substring(0, i).Trim()) && evaluateBool(s.Substring(i + 2).Trim());
			// exhausted all boolean operations, must have an integer expression which is evaluated according to to-do list
			return WheneverCode.code!.getN(evaluateInt(s));
		}


		private void action()
		{
			if (actionString.IndexOf(print) == 0) doPrint(actionString.Substring(print.Length).Trim());
			else doLines(actionString);
			return; // TODO Fix CS0162 :)
		}

		private void doLines(string s)
		{
			// Why do you need a string tokeniser then the only delimiter is , ?
			IEnumerable<string> lines = s.Split(",");
			foreach (var line in lines)
			{
				doLine(line);
			}
		}

		private void doLine(string s)
		{
			int numTimes = 1;
			int lineNumber = 0;
			int i;
			if ((i = s.IndexOf("#")) >= 0)
			{
				numTimes = evaluateInt(s.Substring(i + 1));
				lineNumber = evaluateInt(s.Substring(0, i));
			}
			else
				lineNumber = evaluateInt(s);
			if (WheneverCode.traceLevel >= 2)
				Console.WriteLine($"Adding line {lineNumber}, {numTimes} times");
			WheneverCode.code!.changeNumToDo(lineNumber, numTimes);
		}

		private int evaluateInt(string s)
		{
			Console.WriteLine($"Eval Int: {s}");
			try
			{
				return int.Parse(s);
			}
			catch (FormatException e)
			{
				if (WheneverCode.traceLevel > 4)
				{
					Console.WriteLine($"Int Eval Exception: {s}");
				}
				int i = 0, j = 0;
				if ((i = s.IndexOf("N(")) >= 0)
				{
					Console.WriteLine($"i = {i}, j = {j}");
					j = i + 1;
					int depth = 1;
					try
					{
						while (depth > 0)
						{
							j++;
							if (s[j] == '(') depth++;
							if (s[j] == ')') depth--;
						}
					}
					catch (IndexOutOfRangeException)
					{
						throw new WheneverException("[Error] Mismatched parentheses in arithmetic expression function call");
					}
					if (WheneverCode.traceLevel > 3)
					{
						Console.WriteLine("N() argument: " + s.Substring(i + 2, j - (i + 2)));
						Console.WriteLine("N() substituted: " + s.Substring(0, i) + WheneverCode.code!.getNumToDo(evaluateInt(s.Substring(i + 2, j - (i + 2)))) + s.Substring(j + 1));
					}
					return evaluateInt(s.Substring(0, i) + WheneverCode.code!.getNumToDo(evaluateInt(s.Substring(i + 2, j - (i + 2)))) + s.Substring(j + 1));
				}
				else if ((i = s.IndexOf("(")) >= 0)
				{
					Console.WriteLine($"i = {i}, j = {j}");
					j = i;
					int depth = 1;
					try
					{
						while (depth > 0)
						{
							j++;
							if (s[j] == '(')
								depth++;
							if (s[j] == ')')
								depth--;
						}
					}
					catch (IndexOutOfRangeException)
					{
						throw new WheneverException("[Error] Mismatched parentheses in arithmetic expression");
					}
					if (WheneverCode.traceLevel > 3)
					{
						Console.WriteLine("() argument: " + s.Substring(i + 1, j - (i+1)));
						Console.WriteLine("() substituted: " + s.Substring(0, i) + evaluateInt(s.Substring(i + 1, j - (i+1))) + s.Substring(j + 1));
					}
					return evaluateInt(s.Substring(0, i) + evaluateInt(s.Substring(i + 1, j - (i + 1))) + s.Substring(j + 1));
				}
				else if (s.IndexOf("+") >= 0 || s.IndexOf("-") >= 0)
				{
					throw new NotImplementedException("Yea nah I'm doing this later");
				}
				else
				{
					return multiply(s);
				}
			}
		}

		/*
		 * Evaluates the argument string as an integer expression.
		 * Argument string may only contain * and / binary operators and unary minuses.
		 */
		private int multiply(string s)
		{
			int i = s.IndexOf("*");
			int j = s.IndexOf("/");
			if (i >= 0 && (j < 0 || i < j))
			{
				if (WheneverCode.traceLevel > 3) Console.WriteLine($"Multiply (*) arguments: {s}");
				return evaluateInt(s.Substring(0, i).Trim()) * evaluateInt(s.Substring(i + 1).Trim());
			}
			else
			{
				if (WheneverCode.traceLevel > 3) Console.WriteLine($"Divide (/) arguments: {s}");
				return (int)(evaluateInt(s.Substring(0, j).Trim()) / evaluateInt(s.Substring(j + 1).Trim()));
			}
		}

		private void doPrint(string s)
		{
			// Wow look at me using fancy C# things like s[^1], I'm so smart
			if (s[0] != '(' || s[s.Length - 1] != ')') throw new WheneverException("[Error] Print statement must be in parentheses");
			s = s.Substring(1, s.Length - 2).Trim();
			string outString = "";

			while (s.Length > 0)
			{
				// if item begins with a string literal it begins with a double quote
				if (s[0] == '"')
				{
					int i = s.IndexOf('"', 1);
					if (i < 0) throw new WheneverException("[Error] String must be delimited by double quotes");

					outString += s.Substring(1, i - 1);

					if (i + 1 == s.Length)
					{
						s = "";
					}
					else
					{
						s = s.Substring(i + 1).Trim();

						if (s[0] != '+') throw new WheneverException("Illegal string concatenation, + expected");
						// remove + sign
						s = s.Substring(1).Trim();
					}
				}
				else
				{
					// item does not begin with a string literal
					// get index of opening double quote
					int i = s.IndexOf('"');
					if (i < 0)
					{ // there are no more string literals - all stuff is an integer expression
						outString += evaluateInt(s);
						s = "";
					}
					else
					{ // there is a concatenated string literal
					  // check for concatenation operator
						if (s[s.Substring(0, i).Trim().Length - 1] != '+') throw new WheneverException("Illegal string concatenation, + expected");
						outString += evaluateInt(s.Substring(0, s.Substring(0, i).Trim().Length - 1).Trim());
						s = s.Substring(i);
					}
				}
			}
			Console.WriteLine(outString);
		}
	}
}
