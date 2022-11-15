using System.Collections;

namespace Whenever_in_C_Sharp
{
	class CodeTable: Hashtable
	{
		public int totalToDo { get; private set; } = 0;


		public override void Add(object lineNumber, object? value)
		{
			// Not me realising there was supposed to be more shit here.
			//base.Add(key, value);
			Command command = (Command)value!;

			/**
			 * From Java:
			 * Return Value: If an existing key is passed then the previous value gets returned. If a new pair is passed, then NULL is returned.
			 * So if a key already exists, replace the key and return the old key, else return null and add the item
			 */
			object? old = this[lineNumber];
			//base.Add(lineNumber, value);
			if (old == null)
			{
				base.Add(lineNumber, value);
			}
			else
			{
				base[lineNumber] = value;
			}

			if (old == null)
			{
				totalToDo += command.numToDo;
				return;
			}
			else
			{
				Command oldCommand = (Command)old;
				totalToDo += command.numToDo = oldCommand.numToDo;
				return;
			}
		}

		public bool getN(int lineNumber)
		{
			Command command = (Command)this[lineNumber]!;
			if (command == null)
				return false;
			return command.numToDo > 0;
		}

		public int getNumToDo(int lineNumber)
		{
			Command command = (Command)this[lineNumber]!;
			if (command == null) return 0;
			return command.numToDo;
		}

		public int changeNumToDo(int lineNum, int change)
		{
			if(lineNum < 0)
			{
				lineNum = -lineNum;
				change = -change;
			}
			Command command = (Command)this[lineNum]!;
			if (command == null) return 0; // TODO FROM SOURCE: need to do something here to implement adding extra line numbers
			command.numToDo += change;
			totalToDo += change;
			if(command.numToDo < 0)
			{
				totalToDo -= command.numToDo;
				command.numToDo = 0;
			}
			this.Add(lineNum, command);
			return command.numToDo;
		}

		public void doCommand(int instance)
		{
			Console.WriteLine($"Doing command {instance}");
			IEnumerator enumerator = this.Keys.GetEnumerator();
			enumerator.MoveNext();
			int lineNumber = (int)enumerator.Current;
			enumerator.MoveNext();
			Command command = (Command)this[lineNumber];

			while (instance > command.numToDo)
			{
				instance -= command.numToDo;
				// TODO Make sure this actually works
				lineNumber = (int)enumerator.Current;
				enumerator.MoveNext();
				command = (Command)this[lineNumber];
			}
			if(WheneverCode.traceLevel >= 1) Console.WriteLine("Attempting line " + lineNumber);
			command.execute(lineNumber);

		}
	}
}
