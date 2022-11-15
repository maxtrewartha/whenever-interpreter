using System.Collections;

namespace Whenever_in_C_Sharp
{
	class CodeTable: Hashtable
	{
		public int totalToDo { get; private set; } = 0;


		public Command AddCommand(object lineNumber, object? value)
		{
			// Not me realising there was supposed to be more shit here.
			//base.Add(key, value);
			Command command = (Command)value;

			/**
			 * From Java:
			 * Return Value: If an existing key is passed then the previous value gets returned. If a new pair is passed, then NULL is returned.
			 * So if a key already exists, replace the key and return the old key, else return null and add the item
			 */
			object? old = this[lineNumber];
			base.Add(lineNumber, value);

			if (old == null)
			{
				totalToDo += command.numToDo;
				return null;
			}
			else
			{
				Command oldCommand = (Command)old;
				totalToDo += command.numToDo = oldCommand.numToDo;
				return oldCommand;
			}
		}

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
			base.Add(lineNumber, value);

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
	}
}
