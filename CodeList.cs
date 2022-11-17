using org.matheval.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Whenever_in_C_Sharp
{
	class CodeList: Dictionary<int, Command>
	{
		// Key is line number
		// Value is how many more times it has to do it
		public int totalToDo { get; set; } = 0;


		// returns null if the key/value pair was added to the dictionary; if the key already exists, it returns the old value and replaces the value for that key.
		// its basically the same as Java's Hashtable put() method
		public Command? Put (int lineNumber, Command value)
		{
			if (base.ContainsKey(lineNumber))
			{
				Command old = base[lineNumber];
				base[lineNumber] = value;
				return old;
			}
			else
			{
				base.Add(lineNumber, value);
				return null;
			}
		}

		public void doCommand(int instance)
		{
			Whenever.debug($"Do command called: {instance}");
			IEnumerator commands = this.Keys.GetEnumerator();
			int lineNumber = (int)commands.Current;
			Command command = this[lineNumber];
			while (instance > command.numToDo)
			{
				instance -= command.numToDo;
				commands.MoveNext();
				lineNumber = (int)commands.Current;
				command = this[lineNumber];
			}
			Whenever.debug($"Attempting line {lineNumber}", 5);
		}

	}
}
