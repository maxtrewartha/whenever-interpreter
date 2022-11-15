using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whenever_in_C_Sharp
{
	class WheneverException: Exception
	{
		// https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions
		public WheneverException(): base(){}
		public WheneverException(string message):base(message){}
		public WheneverException(string message, Exception innerException):base(message, innerException){}
	}
}
