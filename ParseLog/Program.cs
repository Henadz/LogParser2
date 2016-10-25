using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseLog
{
	class Program
	{
		static void Main(string[] args)
		{
			var directory = @"d:\Test\RawLog\";
			var cssTitles = new[]
			{
				"GoldfireWebApiCallTime", "CitationsControllerRepository.Post", "PostProcessingResult.Facets",
				"PostProcessingResult.Full", "PostProcessingResult.Citations", "EWBWebApiCallTime"
			};

			var parser = new LogParser(directory);

			var items = parser.Parse();

			var cssData = new List<List<string>>();
			var row = new List<string>
			{
				"File Number"
			};
			row.AddRange(cssTitles);
			cssData.Add(row);

			foreach (var logInfo in items)
			{
				row = new List<string>
				{
					logInfo.Number
				};
				row.AddRange
					(
						cssTitles
						.Select(title => logInfo.Metrics.FirstOrDefault(x => x.What.Equals(title, StringComparison.Ordinal)))
						.Select(value => value?.Value.ToString(CultureInfo.CurrentCulture) ?? "")
					);
				cssData.Add(row);
			}

			CsvWriter.Write(cssData, @"d:\Test\metrics.csv", ';', Encoding.UTF8);
		}
	}
}
