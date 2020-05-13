using FluentAssertions;
using Kraken.Configuration;
using System.IO;
using System.Reflection;
using Xunit;

namespace Kraken.Test
{
	public class ConfigurationLoaderShould
	{
		[Fact]
		public void LoadJsonConfig()
		{
			var root = Assembly.GetExecutingAssembly().Location;
			root = root.Replace(Path.GetFileName(root), "");
			var json = File.ReadAllText(Path.Combine(root, "AppConfiguration.json.test"));

			var config = ConfigurationLoader.Load(new[] { json });

			config.Should().NotBeNull();
			config.Should().BeEquivalentTo(new Config
			{
				SolutionFolder = "1",
				OctopusEndpoint = "2",
				OctopusApiKey = "3",
				ConfigurationPath = new[] { "4" },
				Environment = "5"
			});
		}

		[Fact]
		public void LoadArgsConfig()
		{
			var config = ConfigurationLoader.Load(new[]
			{
				"-SolutionFolder","1",
				"-OctopusEndpoint", "2",
				"-OctopusApiKey", "3",
				"-ConfigurationPath", "4;7",
				"-Environment", "5"
			});

			config.Should().NotBeNull();
			config.Should().BeEquivalentTo(new Config
			{
				SolutionFolder = "1",
				OctopusEndpoint = "2",
				OctopusApiKey = "3",
				ConfigurationPath = new[] { "4", "7" },
				Environment = "5"
			});
		}
	}
}
