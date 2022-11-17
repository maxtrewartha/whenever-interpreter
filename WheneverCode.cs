using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whenever_in_C_Sharp
{
	class WheneverCode
	{
		private static readonly string defer = "defer";
		private static readonly string again = "again";
		// TODO Implement CodeTable
		public static CodeList? list { get; set; }
		public static int traceLevel { get; private set; } = 0;

		// Constructor for code and shit
		public WheneverCode(string source)
		{
			// I think this works?
			new WheneverCode(source, "0");
		}

		public WheneverCode(string source, string trace)
		{
			// I'm porting this properly, so everything will bt tried and caught (hopefully)
			try
			{
				traceLevel = int.Parse(trace);
			}
			catch (FormatException)
			{
				Console.WriteLine("[Error] Illegal value used in trace level, must be an integer");
				Environment.Exit(1);
			}

			list = new CodeList();
			try
			{
				using (StreamReader file = new StreamReader(source))
				{
					int counter = 0;
					string ln;

					while ((ln = file.ReadLine()) != null)
					{
						// TODO Implement Command Parsing
						parse(ln.Trim());
						counter++;
					}
					Whenever.debug($"Sucessfully parsed {counter} line{(counter == 1 ? "" : "s")}");
					file.Close();
				}
			}
			catch
			{
				Console.WriteLine($"[Error] Program source file unavailable: {source}");
				Environment.Exit(1);
			}
		}

		private void parse(string source)
		{
			// Make sure the line actually starts with a line number
			int space = source.IndexOf(' ');
			int tab = source.IndexOf('\t');
			if (space < 0 && tab < 0) throw new WheneverException("[Error] Missing line number");

			// Gets line number

			int? lineNumber;
			int whitespace = tab < 0 || tab > space ? space : tab;
			try
			{
				lineNumber = int.Parse(source.Substring(0, whitespace));
				Whenever.debug($"Parsing Line Number {lineNumber}", 7);
			}
			catch (FormatException)
			{
				Console.WriteLine($"[Error] Invalid line number format: {source}");
				Environment.Exit(1);
			}

			// Get line contents

			source = source.Substring(whitespace);
			Command command = new Command();
			int endDefer = 0;
			int endAgain = 0;

			int i = source.IndexOf(defer);

			// There is a defer clause
			if (i >= 0)
			{
				endDefer = source.IndexOf('(', i);
				i = endDefer;
				int nest = 1;

				try
				{
					while (nest > 0)
					{
						if (source[++endDefer] == ')') nest--;
						if (source[endDefer] == '(') nest++;
					}
				}
				catch (IndexOutOfRangeException)
				{
					throw new WheneverException("[Error] Defer clause doesn't have matched parentheses");
				}
			}

		}

		public void run()
		{
			while (list!.totalToDo > 0)
			{
				int instance = (int)(new Random().NextDouble() * list!.totalToDo) + 1;
				list.doCommand(instance);
			}
		}
	}
}
