using System.Collections;

namespace Whenever_in_C_Sharp
{
	class CodeTable: Hashtable
	{
		public int totalToDo { get; private set; } = 0;

		public override void Add(object key, object? value)
		{
			// Not me realising there was supposed to be more shit here.
			base.Add(key, value);
		}

		public bool getN(int lineNumber)
		{
			Command command = (Command)this[lineNumber]!;
			if (command == null)
				return false;
			return command.numToDo > 0;
		}
	}
}
