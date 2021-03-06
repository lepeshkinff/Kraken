using FluentAssertions;
using Kraken.Engine;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Kraken.Test
{
	public class ConfigurationsProviderShould
	{
		[Fact]
		public async Task LoadFilesByUrl()
		{
			var httpClient = new Mock<IHttpClient>();
			httpClient.Setup(x => x.GetStringAsync(It.IsAny<Uri>()))
				.ReturnsAsync(
					@"{
						""Version"": 1,
						""Configurations"" : 
						[
							{
								""Id"": ""8682B238-65D8-43E7-98E2-E24BB739E195"",
								""ComponentName"": ""OctopusComponentName"",
								""PathToFile"": ""Path/To/File/Local/appsettings.json"",
								""OctopusProject"": ""Some Octopus Project"",
								""OctopusArtifactName"": ""SomeArtifact.json"",
								""IsSubstitutionsOnly"": false,
								""Substitutions"": {}
							}
						]
					}");

			var provider = new ConfigurationsProvider(
				new[]
				{
					"http://some.url.com",
				},
				httpClient.Object);

			var configs = await provider.GetConfigurations();

			configs.Should().NotBeNull();
			configs.Configurations.Should().NotBeNullOrEmpty();
			configs.Configurations.Should().HaveCount(1);
			configs.Configurations.First().Id.Should().Be(Guid.Parse("8682B238-65D8-43E7-98E2-E24BB739E195"));
		}

		[Fact]
		public async Task LoadFilesByPattern()
		{
			var httpClient = new Mock<IHttpClient>();
			var provider = new ConfigurationsProvider(
				new[]
				{
					"*.json",
				},
				httpClient.Object);

			var configs = await provider.GetConfigurations();
			
			configs.Should().NotBeNull();
			configs.Configurations.Should().NotBeNullOrEmpty();
			configs.Configurations.Should().HaveCount(4);
		}

		[Fact]
		public async Task LoadFilesByStrongName()
		{
			var httpClient = new Mock<IHttpClient>();
			var root = Assembly.GetExecutingAssembly().Location;
			root = root.Replace(Path.GetFileName(root), "");

			var provider = new ConfigurationsProvider(
				new[]
				{
				  Path.Combine(root,  "FolderForTest\\Subfolder\\json1.json"),
				},
				httpClient.Object);

			var configs = await provider.GetConfigurations();

			configs.Should().NotBeNull();
			configs.Configurations.Should().NotBeNullOrEmpty();
			configs.Configurations.Should().HaveCount(2);
		}

		[Fact]
		public async Task LoadFilesByStrongNameWithRelativePath()
		{
			var httpClient = new Mock<IHttpClient>();
			var provider = new ConfigurationsProvider(
				new[]
				{
					"FolderForTest\\Subfolder\\json1.json"
				},
				httpClient.Object);

			var configs = await provider.GetConfigurations();

			configs.Should().NotBeNull();
			configs.Configurations.Should().NotBeNullOrEmpty();
			configs.Configurations.Should().HaveCount(2);
		}
		
		[Theory]
		[InlineData("[]")] //backward compatible content
		[InlineData("AI will do the trick")] //backward compatibility broken
		public async Task FailToLoadUriWithHigherVersion(string configurations)
		{
			var httpClient = new Mock<IHttpClient>();
			httpClient.Setup(x => x.GetStringAsync(It.IsAny<Uri>()))
				.ReturnsAsync(
					@$"{{
						""Version"": 2,
						""Configurations"" : ""{configurations}""
					}}");

			var provider = new ConfigurationsProvider(
				new[]
				{
					"http://some.url.com",
				},
				httpClient.Object);

			var configs = await provider.GetConfigurations();
			configs.Should().NotBeNull();
			configs.Errors.Should().HaveCount(1);
			configs.Errors.Single().Should().StartWith("Ваш Kraken не дорос");
		}

	}
}
