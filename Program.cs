// See https://aka.ms/new-console-template for more information
namespace Whenever_in_C_Sharp
{
	public class Whenever
	{
		private static WheneverCode code;
		static void Main(string[] args)
		{
			if (args.Length == 2)
			{
				code = new WheneverCode(args[0], args[1]);
				code.run();
			}
			else if (args.Length == 1)
			{
				code = new WheneverCode(args[0]);
				code.run();
			}
			else
			{
				Console.WriteLine("Usage: whenever.exe <sourcefile> [tracelevel]");
				Environment.Exit(0);
			}
		}

		public static void debug(string message, int level = 0)
		{
			// There is no pattern to the trace levels, you may as well just leave it on 7
			if (WheneverCode.traceLevel >= level)
			{
				// string concat or format strings? it doesn't really matter, the code is slow enough anyway
				Console.WriteLine($"[DEBUG] {message}");
			}
		}
	}
}