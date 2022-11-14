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
				Console.WriteLine("[" + Environment.ProcessPath + "] Invalid trace level, use an integer");
				Environment.Exit(1);
			}
			code = new CodeTable();
			try
			{



			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine("[" + Environment.ProcessPath + "] Program source file unavailable: " + infile);
			}
		}

		private void parse(string line)
		{
			int space = line.IndexOf(" ");
			int tab = line.IndexOf("\t");
			if (space < 0 && tab < 0) throw new SyntaxErrorException("Missing line number");
			int? lineNumber;
			try
			{
				var lineNo = line.Substring(0, (tab < 0 || tab > space) ? space : tab);
				lineNumber = int.Parse(lineNo);
			}

		}

	}
}
