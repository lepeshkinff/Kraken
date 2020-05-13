using FluentAssertions;
using Kraken.Engine;
using System.IO;
using System.Reflection;
using Xunit;

namespace Kraken.Test
{
	public class ConfigurationsProviderShould
	{
		public ConfigurationsProviderShould()
		{

		}

		[Fact]
		public void LoadFilesByPattern()
		{
			var provider = new ConfigurationsProvider(
				new[]
				{
					"*.json",
				});

			var configs = provider.GetConfigurations();

			configs.Should().NotBeNull();
			configs.Should().NotBeEmpty();
			configs.Should().HaveCount(4);
		}

		[Fact]
		public void LoadFilesByStrongName()
		{
			var root = Assembly.GetExecutingAssembly().Location;
			root = root.Replace(Path.GetFileName(root), "");

			var provider = new ConfigurationsProvider(
				new[]
				{
				  Path.Combine(root,  "FolderForTest\\Subfolder\\json1.json"),
				});

			var configs = provider.GetConfigurations();

			configs.Should().NotBeNull();
			configs.Should().NotBeEmpty();
			configs.Should().HaveCount(2);
		}

		[Fact]
		public void LoadFilesByStrongNameWithRelativePath()
		{
			var provider = new ConfigurationsProvider(
				new[]
				{
					"FolderForTest\\Subfolder\\json1.json"
				});

			var configs = provider.GetConfigurations();

			configs.Should().NotBeNull();
			configs.Should().NotBeEmpty();
			configs.Should().HaveCount(2);
		}
	}
}
