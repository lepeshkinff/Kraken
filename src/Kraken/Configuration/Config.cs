namespace Kraken.Configuration
{
	internal class Config
	{
		public string Environment { get; set; }
		public string SolutionFolder { get; set; }
		public string OctopusEndpoint { get; set; }
		public string OctopusApiKey { get; set; }
		public string[] ConfigurationPath { get; set; }
	}
}