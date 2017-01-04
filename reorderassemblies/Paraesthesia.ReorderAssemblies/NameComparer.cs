using System;
using System.Collections.Generic;

namespace Paraesthesia.ReorderAssemblies
{
	public class NameComparer : IComparer<string>
	{
		public int Compare(string x, string y)
		{
			bool xIsFramework = IsFramework(x);
			bool yIsFramework = IsFramework(y);
			if (xIsFramework)
			{
				if (yIsFramework)
				{
					// Both are framework assemblies.
					return StringComparer.OrdinalIgnoreCase.Compare(x, y);
				}
				// Only X is a framework assembly.
				return -1;
			}
			if (yIsFramework)
			{
				// Only Y is a framework assembly.
				return 1;
			}

			// Neither is a framework assembly.
			return StringComparer.OrdinalIgnoreCase.Compare(x, y);
		}

		public static bool IsFramework(string name)
		{
			return name != null &&
				(name.StartsWith("mscorlib", StringComparison.OrdinalIgnoreCase) ||
				name.Equals("System", StringComparison.OrdinalIgnoreCase) ||
				name.StartsWith("System.", StringComparison.OrdinalIgnoreCase));
		}
	}
}
