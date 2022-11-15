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
				code = new WheneverCode(args[0], "0");
				code.run();
			}
			else
			{
				Console.WriteLine("Usage: whenever.exe <sourcefile> [tracelevel]\n");
				Environment.Exit(0);
			}
		}
	}
}