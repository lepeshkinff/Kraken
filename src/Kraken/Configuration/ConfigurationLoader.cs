using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Kraken.Configuration
{
	internal static class ConfigurationLoader
	{
		public static Config Load(string[] startingArgs)
		{
			Config config = null;

			if (startingArgs?.Any() == true)
			{
				config = JsonConvert.DeserializeObject<Config>(startingArgs[0]);
			}

			config = config ?? new Config();

			if (string.IsNullOrWhiteSpace(config.OctopusApiKey))
			{
				config.OctopusApiKey = ConfigurationManager.AppSettings["OctopusApiKey"] ??
					Environment.GetEnvironmentVariable("OctopusApiKey");

				if(string.IsNullOrWhiteSpace(config.OctopusApiKey))
				{
					throw new ArgumentException("OctopusApiKey должен быть указан");
				}
			}

			if (string.IsNullOrWhiteSpace(config.OctopusEndpoint))
			{
				config.OctopusApiKey = ConfigurationManager.AppSettings["OctopusEndpoint"] ??
					Environment.GetEnvironmentVariable("OctopusEndpoint");

				if (string.IsNullOrWhiteSpace(config.OctopusEndpoint))
				{
					throw new ArgumentException("OctopusEndpoint должен быть указан");
				}
			}

			if (string.IsNullOrWhiteSpace(config.ConfigurationPath))
			{
				config.ConfigurationPath = ConfigurationManager.AppSettings["configurationPath"] ??
					Environment.GetEnvironmentVariable("ConfigurationPath") ??
					Path.Combine(Assembly.GetExecutingAssembly().Location, "configuration.json");
			}

			if (string.IsNullOrWhiteSpace(config.Environment))
			{
				config.Environment = ConfigurationManager.AppSettings["octopusEnvironment"] ??
					Environment.GetEnvironmentVariable("OctopusEnvironment");
			}

			if (string.IsNullOrWhiteSpace(config.SolutionFolder))
			{
				config.Environment = ConfigurationManager.AppSettings["SolutionFolder"] ??
					Environment.GetEnvironmentVariable("SolutionFolder");
			}

			return config;
		}
	}
}
