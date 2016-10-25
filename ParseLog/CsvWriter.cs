using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ParseLog
{
	internal class CsvWriter
	{
		private static readonly char[] EncodedValues = {'"', ',', ';', '\n'};

		public static void Write(IEnumerable<IList<string>> data, string path, char delimeter, Encoding encoding)
		{
			var lines = GetCsvLines(data, delimeter);
			File.WriteAllLines(path, lines, encoding);
		}

		private static IEnumerable<string> GetCsvLines(IEnumerable<IList<string>> data, char delimeter)
		{
			var listData = data.ToList();
			var lineItemsCount = listData.Select(x => x.Count).Max();

			foreach (var row in listData)
			{
				while (row.Count < lineItemsCount)
					row.Add(string.Empty);

				yield return string.Join(delimeter.ToString(CultureInfo.InvariantCulture),
					row.Select(x => x != null && x.IndexOfAny(EncodedValues) != -1
						? string.Format(CultureInfo.InvariantCulture, "\"{0}\"", x.Replace("\"", "\"\""))
						: x ?? ""));
			}
		}
	}
}