using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ParseLog
{
	public class LogParser
	{
		private readonly string _directory;

		public LogParser(string directory)
		{
			_directory = directory;
		}

		public IList<LogInfo> Parse()
		{
			var result = new List<LogInfo>();
			foreach (var file in Directory.GetFiles(_directory, "*_s.txt", SearchOption.AllDirectories))
			{
				var lines = File.ReadLines(file, Encoding.UTF8).ToArray();
				if (lines.Length == 0 || !lines[0].Contains("200 OK")) continue;

				var json = lines.Last();

				var jobject = JObject.Parse(json);

				var jmetrics = jobject["Log"]?["Metrics"];

				if (jmetrics != null)
				{
					var info = new LogInfo
					{
						Number = Path.GetFileNameWithoutExtension(file),
						Metrics = new List<Metric>()
					};
					foreach (var jmetric in jmetrics)
					{
						info.Metrics.Add(jmetric.ToObject<Metric>());
					}
					result.Add(info);
				}
			}

			return result;
		}

	}
}