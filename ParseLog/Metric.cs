using System;

namespace ParseLog
{
	public class Metric
	{
		public string Party { get; set; }
		public string Type { get; set; }
		public double Value { get; set; }
		public string What { get; set; }
		public DateTime When { get; set; }
	}
}