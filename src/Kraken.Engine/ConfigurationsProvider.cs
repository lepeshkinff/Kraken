using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Kraken.Engine
{
	public class ConfigurationsProvider
	{
		private string configurationPath;

		public ConfigurationsProvider(string configurationPath)
		{
			this.configurationPath = configurationPath;
		}

		public IReadOnlyCollection<FileConfiguration> GetConfigurations() =>
			JsonConvert.DeserializeObject<List<FileConfiguration>>(File.ReadAllText(configurationPath));
	}
}
