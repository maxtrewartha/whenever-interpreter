using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace Whenever_in_C_Sharp
{
	class WheneverCode
	{
		private static readonly string defer = "defer";
		private static readonly string again = "again";
		public static CodeTable? code;
		public static int traceLevel { get; set; } = 0;

		public WheneverCode(string infile, string t)
		{
			if (string.IsNullOrWhiteSpace(t)) t = "0";
			try
			{
				traceLevel = int.Parse(t);
			} 
			catch (FormatException e)
			{
				Console.WriteLine("Invalid trace level, use an integer");
				Environment.Exit(1);
			}
			code = new CodeTable();
			try
			{
				using (StreamReader file = new StreamReader(infile))
				{
					int counter = 0;
					string ln;

					while ((ln = file.ReadLine()) != null)
					{
						parse(ln.Trim());
						counter++;
					}
					Console.WriteLine($"Sucessfully parsed {counter} lines");
					file.Close();
				}
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine("[Error] Program source file unavailable: " + infile);
			}
		}

		private void parse(string line)
		{
			int space = line.IndexOf(" ");
			int tab = line.IndexOf("\t");
			if (space < 0 && tab < 0) throw new WheneverException("[Error] Missing line number");
			int? lineNumber;
			try
			{
				var lineNo = line.Substring(0, (tab < 0 || tab > space) ? space : tab);
				lineNumber = int.Parse(lineNo);
			}
			catch (FormatException e)
			{
				throw new WheneverException("[Error] Bad line number format");
			}
			line = line.Substring((tab < 0 || tab > space) ? space : tab).Trim();
			Command command = new Command();
			int endDefer = 0, endAgain = 0;
			int i = line.IndexOf(defer);
			if (i >= 0)
			{
				endDefer = i = line.IndexOf('(', i);
				int nest = 1;
				try
				{
					while(nest > 0)
					{
						if (line[++endDefer] == ')') nest--;
						if (line[endDefer] == '(') nest++;
					}
				}
				catch (IndexOutOfRangeException e)
				{
					throw new WheneverException("Defer clause required matched parenthesesesesesesese");
				}
				command.deferString = line.Substring(i + 1, endDefer++ - (i+1));
			}
			line = line.Substring(Math.Max(endDefer, endAgain)).Trim();
			if (line[line.Length - 1] != ';') throw new WheneverException("Line requires semi-colon termination");
			command.actionString = line.Substring(0, line.Length - 1);
			code.Add(lineNumber, command);

		}

		public void run()
		{
			while (code.totalToDo > 0)
			{
				int totalToDo = code.totalToDo;
				int instanceToDo = (int)new Random().NextDouble() * totalToDo + 1;
				code.doCommand(instanceToDo);
			}
		}

	}
}
