using System;
using System.Text;

namespace Paraesthesia.ReorderAssemblies
{
	public static class ByteArrayExtensions
	{
		public static string ToHexString(this byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
			{
				return "";
			}

			var builder = new StringBuilder();
			foreach (var b in bytes)
			{
				builder.AppendFormat("{0:x}", b);
			}
			return builder.ToString();
		}
	}
}
