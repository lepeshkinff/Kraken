using Octopus.Client;
using Octopus.Client.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kraken.Engine
{
	/// <summary>
	/// Получает окружения из Октопус
	/// </summary>
	public class EnvironmentsProvider
	{
		private readonly string octopusEndpoint;
		private readonly string octopusApiKey;

		public EnvironmentsProvider(string octopusApiKey, string octopusEndpoint)
		{
			this.octopusEndpoint = octopusEndpoint;
			this.octopusApiKey = octopusApiKey;
		}

		public async Task<string[]> GetEnvironments()
		{
			var endpoint = new OctopusServerEndpoint(octopusEndpoint, octopusApiKey);
			var client = await OctopusAsyncClient.Create(endpoint);
			OctopusAsyncRepository repository = new OctopusAsyncRepository(client);

			var environments = await repository.Environments.FindAll();
			return environments.Select(x => x.Name).ToArray();
		}
	}
}
