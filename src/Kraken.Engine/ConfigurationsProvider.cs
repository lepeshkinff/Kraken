using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Kraken.Engine
{
	public class ConfigurationsProvider
	{
		//Версия файлов, с котрыми умеет работать тул. Увеличить при breaking changes
		public const int currentToolVersion = 1;
		private readonly string[] configurationPath;
		private readonly IHttpClient httpClient;

		public ConfigurationsProvider(string[] configurationPath, IHttpClient httpClient)
		{
			this.configurationPath = configurationPath ?? throw new ArgumentNullException(nameof(configurationPath), "не задан путь к файлам конфигураций");
			this.httpClient = httpClient;
		}

		public async Task<IReadOnlyCollection<FileConfiguration>> GetConfigurations()
		{
			var list = new List<FileConfiguration>();
			foreach(var path in configurationPath)
			{
				if(!Uri.TryCreate(path, UriKind.Absolute, out var uriResult) ||
					(!Uri.UriSchemeHttp.Equals(uriResult?.Scheme, StringComparison.OrdinalIgnoreCase)
					&& !Uri.UriSchemeHttps.Equals(uriResult?.Scheme, StringComparison.OrdinalIgnoreCase)))
				{
					LoadFile(list, path);
				}
				else
				{
					await LoadFileUri(list, uriResult);
				}
			}
			return list;
		}

		private async Task LoadFileUri(List<FileConfiguration> list, Uri uri)
		{
			try
			{
				var json = await httpClient.GetStringAsync(uri);
				list.AddRange(LoadConfigurationsFromFile(json));
			}
			catch (VersionMismatchException) { throw; }
			catch {}
		}

		private static void LoadFile(List<FileConfiguration> list, string path)
		{
			var fileName = Path.GetFileName(path);
			var dir =  path.Replace(fileName, "");

			if (string.IsNullOrWhiteSpace(dir) || !Path.IsPathRooted(dir))
			{
				var root = Assembly.GetExecutingAssembly().Location;
				dir = Path.Combine(root.Replace(Path.GetFileName(root), ""), dir);
			}

			foreach (var file in Directory.GetFiles(dir, fileName, SearchOption.AllDirectories))
			{
				try
				{
					list.AddRange(LoadConfigurationsFromFile(File.ReadAllText(file)));
				}
				catch (VersionMismatchException) { throw; }
				catch {}
			}
		}

		private static List<FileConfiguration> LoadConfigurationsFromFile(string jsonSting)
		{
			var fileWithVersion = JsonConvert.DeserializeObject<FileWithVersion>(jsonSting);
			if (fileWithVersion.Version.Value > currentToolVersion)
			{
				//TODO: вообще хорошо бы не валить все конфиги из-за одного. а еще сообщать другие проблемы с загрузкой конфигов.
				throw new VersionMismatchException(fileWithVersion.Version.Value, currentToolVersion);
			}

			var krakenFile = JsonConvert.DeserializeObject<KrakenFile>(jsonSting);
			return krakenFile.Configurations;
		}
		
	}
}
