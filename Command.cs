using org.matheval;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Whenever_in_C_Sharp
{
	class Command
	{
		private static readonly string print = "print";
		// I'm not writing 3 functions to do what 21 characters can do
		public string? deferString { private get; set; } = null;
		public string? againString { private get; set; } = null;
		public string? actionString { private get; set; } = null;
		public int numToDo { get; set; } = 1;

		// TODO Unary not still needs implementing
		private bool evalBool(string source)
		{
			Whenever.debug($"Eval Bool Input: {source}", 7);
			// Literally just booleans
			if(source == null || source.ToLower().Equals("false")) return false;
			if(source.ToLower().Equals("true")) return true;

			// Original interpreter in java used lovely spaghetti code to evaluate booleans, I'm using a library, because.
			Expression expr = new Expression(source);
			return expr.Eval<bool>();

		}

		private int evalInt(string source)
		{
			Whenever.debug($"Eval Int Input: {source}", 7);
			try
			{
				// First try parsing the integer
				return int.Parse(source);
			}
			catch (FormatException)
			{
				Whenever.debug($"Eval Int Caught: {source}", 3);
				// Unfortunately I can't just use the same code from evalBool()

				int i, j;

				// If source has builtin N()
				// N() takes an integer argument and returns the number of times that line number is in the current to-do list. 
				if ((i = source.IndexOf("N(")) >= 0)
				{
					j = i + 1;
					int depth = 1;
					try
					{
						while (depth > 0)
						{
							j++;
							if (source[j] == '(') depth++;
							if (source[j] == ')') depth--;
						}
					}
					catch (IndexOutOfRangeException)
					{
						throw new WheneverException("[Error] Mismatched parentheses in N() function.");
					}
					Whenever.debug($"N() Argument: {source}", 3);
					Whenever.debug($"N() Substituted: {source.Substring(0, i)} {WheneverCode.list[evalInt(source.Substring(i+2, j-(i+2)))]} {source.Substring(j+1)}", 3);
					throw new NotImplementedException();
				}
				throw new NotImplementedException();
			}
		}
	}
}
