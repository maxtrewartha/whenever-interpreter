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
		private string? deferString { get; set; }
		private string? againString { get; set; }
		private string? actionString { get; set; }
		public int numToDo = 1;

		public void execute(int lineNumber)
		{
			try
			{

			} catch (SyntaxErrorException e)
			{
				Console.WriteLine("[" + Environment.ProcessPath + "] Syntax error at line " + lineNumber.ToString());
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
			if(i >= 0)
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
				string op = s.Substring(i, i + 2);
				// extract left comparison operand and left boolean leftover string if necessary
				if (lside >= 0)
				{
					lside += 2;
					lstring = s.Substring(0, lside);
					lcomp = s.Substring(lside, i).Trim();
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
					rcomp = s.Substring(i, rside).Trim();
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
				return evaluateBool(lstring + String.valueOf(comp) + rstring);
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
			if (actionString.IndexOf(print) == 0) throw new NotImplementedException("doPrint(actionString.SubString(print.length)).Trim()");
			else throw new NotImplementedException("doLines(actionString)");
			return; // TODO Fix CS0162 :)
		}



		private int evaluateInt(string s)
		{
			try
			{
				return int.Parse(s);
			} catch (FormatException e)
			{
				if(WheneverCode.traceLevel > 4)
				{
					Console.WriteLine("[" + Environment.ProcessPath + " Format exception, passed: " + s);
				}
				throw new NotImplementedException("Yea nah I'm doing this later");
			}
		}

		/*
		 * Evaluates the argument string as an integer expression.
		 * Argument string may only contain * and / binary operators and unary minuses.
		 */
		private int multiply(String s)
		{
		int i = s.IndexOf("*");
		int j = s.IndexOf("/");
		if (i >= 0 && (j< 0 || i<j))
		{
			if (WheneverCode.traceLevel > 3) Console.WriteLine("[" + Environment.ProcessPath +"] [* arguments: " + s.Substring(0, i).Trim() + ", " + s.Substring(i+1).Trim() + "]");
			return evaluateInt(s.Substring(0, i).Trim()) * evaluateInt(s.Substring(i+1).Trim());
		}
		else
		{
			if (WheneverCode.traceLevel > 3) Console.WriteLine("[/ arguments: " + s.Substring(0, j).Trim() + ", " + s.Substring(j+1).Trim() + "]");
			return (int) (evaluateInt(s.Substring(0,j).Trim()) / evaluateInt(s.Substring(j+1).Trim()));
		}
	}
	}
}
