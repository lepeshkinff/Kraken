using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Kraken.Engine
{
	public class ConfigurationsProvider
	{
		private string[] configurationPath;

		public ConfigurationsProvider(string[] configurationPath)
		{
			this.configurationPath = configurationPath ?? throw new ArgumentNullException(nameof(configurationPath), "не задан путь к файлам конфигураций");
		}

		public IReadOnlyCollection<FileConfiguration> GetConfigurations()
		{
			var list = new List<FileConfiguration>();
			foreach(var path in configurationPath)
			{
				var fileName = Path.GetFileName(path);
				var dir = path.Replace(fileName, "");

				if(string.IsNullOrWhiteSpace(dir) || !Path.IsPathRooted(dir))
				{
					var root = Assembly.GetExecutingAssembly().Location;
					dir = Path.Combine(root.Replace(Path.GetFileName(root), ""), dir);
				}

				foreach(var file in Directory.GetFiles(dir, fileName, SearchOption.AllDirectories))
				{
					try
					{
						list.AddRange(JsonConvert.DeserializeObject<List<FileConfiguration>>(File.ReadAllText(file)));
					}
					catch { }
				}
			}
			return list;
		}
	}
}
