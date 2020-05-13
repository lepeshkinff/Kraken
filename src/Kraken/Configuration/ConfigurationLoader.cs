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

            if (startingArgs?.Length == 1)
            {
                config = JsonConvert.DeserializeObject<Config>(startingArgs[0]);
            }
            else if(startingArgs?.Length > 1)
            {
                config = LoadArgs(startingArgs);
            }
            config = config ?? new Config();

            if (string.IsNullOrWhiteSpace(config.OctopusApiKey))
            {
                config.OctopusApiKey = ConfigurationManager.AppSettings[nameof(config.OctopusApiKey)] ??
                    Environment.GetEnvironmentVariable(nameof(config.OctopusApiKey));

                if (string.IsNullOrWhiteSpace(config.OctopusApiKey))
                {
                    throw new ArgumentException(nameof(config.OctopusApiKey)+" должен быть указан");
                }
            }

            if (string.IsNullOrWhiteSpace(config.OctopusEndpoint))
            {
                config.OctopusEndpoint = ConfigurationManager.AppSettings[nameof(config.OctopusEndpoint)] ??
                    Environment.GetEnvironmentVariable(nameof(config.OctopusEndpoint));

                if (string.IsNullOrWhiteSpace(config.OctopusEndpoint))
                {
                    throw new ArgumentException(nameof(config.OctopusEndpoint)+" должен быть указан");
                }
            }

            if (config.ConfigurationPath?.Any() != true)
            {
               var str = ConfigurationManager.AppSettings[nameof(config.ConfigurationPath)] ??
                    Environment.GetEnvironmentVariable(nameof(config.ConfigurationPath)) ??
                    Path.Combine(Assembly.GetExecutingAssembly().Location, "configuration.json");

               config.ConfigurationPath = str.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (string.IsNullOrWhiteSpace(config.Environment))
            {
                config.Environment = ConfigurationManager.AppSettings[nameof(config.Environment)] ??
                    Environment.GetEnvironmentVariable(nameof(config.Environment));
            }

            if (string.IsNullOrWhiteSpace(config.SolutionFolder))
            {
                config.SolutionFolder = ConfigurationManager.AppSettings[nameof(config.SolutionFolder)] ??
                    Environment.GetEnvironmentVariable(nameof(config.SolutionFolder));
            }

            return config;
        }

        private static Config LoadArgs(string[] startingArgs)
        {
            Config config = new Config();
            for (var i = 0; i < startingArgs.Length; i++)
            {
                switch (startingArgs[i].ToUpperInvariant())
                {
                    case "-ENVIRONMENT":
                    case "-E":
                        i++;
                        config.Environment = startingArgs[i];
                        break;
                    case "-SOLUTIONFOLDER":
                    case "-S":
                        i++;
                        config.SolutionFolder = startingArgs[i]
                            .Replace("\"", ""); //студия, будь она не ладна, сюда пихает кавычки (((
                        break;
                    case "-OCTOPUSENDPOINT":
                    case "-OE":
                        i++;
                        config.OctopusEndpoint = startingArgs[i];
                        break;
                    case "-OCTOPUSAPIKEY":
                    case "-OK":
                        i++;
                        config.OctopusApiKey = startingArgs[i];
                        break;
                    case "-CONFIGURATIONPATH":
                    case "-C":
                        i++;
                        config.ConfigurationPath = startingArgs[i].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                  break;
                }
            }

            return config;
        }
    }
}
