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
	/// Получает конфигурационные файлы и прочие артефакты из октопус
	/// </summary>
	public class ArtifactsProvider
	{
		private readonly string octopusEndpoint;
		private readonly string octopusApiKey;

		public ArtifactsProvider(string octopusApiKey, string octopusEndpoint)
		{
			this.octopusEndpoint = octopusEndpoint;
			this.octopusApiKey = octopusApiKey;
		}

		public async Task<Dictionary<string, string>> GetArtifacts(string environmentName, string projectName, params string[] artifactNames)
		{
			var endpoint = new OctopusServerEndpoint(octopusEndpoint, octopusApiKey);
			var client = await OctopusAsyncClient.Create(endpoint);
			OctopusAsyncRepository repository = new OctopusAsyncRepository(client);

			var artifacts = await GetArtifactsList(repository, environmentName, projectName);

			var artifactsByName = new Dictionary<string, string>();
			foreach (var artifactName in artifactNames)
			{
				var matchingArtifact = artifacts.Items.Single(a => a.Filename == artifactName);
				if (matchingArtifact == null)
				{
					throw new ArtifactsProviderException(
						 $"У деплоя проекта {projectName} на среду {environmentName} отсутствует артефакт {artifactName}. " +
						 "Проверьте свежесть и полноту деплоя.");
				}
				using (var stream = await repository.Artifacts.GetContent(matchingArtifact))
				using (var streamReader = new StreamReader(stream))
				{
					artifactsByName[artifactName] = await streamReader.ReadToEndAsync();
				}
			}

			return artifactsByName;
		}

		private static async Task<ResourceCollection<ArtifactResource>> GetArtifactsList(
			 OctopusAsyncRepository repository,
			 string environmentName,
			 string projectName)
		{
			var environment = await repository.Environments.FindOne(x => x.Name.Equals(environmentName, System.StringComparison.OrdinalIgnoreCase));
			if (environment == null)
			{
				throw new ArtifactsProviderException($"Среда {environmentName} не найдена. Проверьте написание среды (case sensetive)");
			}

			var project = await repository.Projects.FindOne(x => x.Name.Equals(projectName, System.StringComparison.OrdinalIgnoreCase));
			if (project == null)
			{
				throw new ArtifactsProviderException($"Проект {projectName} не найден в октопусе. Проверьте конфигурацию");
			}

			var deployments = await repository.Deployments.FindBy(new[] { project.Id }, new[] { environment.Id }, take: 1);
			var deployment = deployments.Items.SingleOrDefault();
			if (deployment == null)
			{
				throw new ArtifactsProviderException($"Не удалось найти деплой проекта {projectName} на среду {environmentName}");
			}

			return await repository.Artifacts.FindRegarding(new DeploymentResource { Id = deployment.Id });
		}

		public async Task<IDictionary<string, string>> GetVariables(string environmentName, string projectName)
		{
			var endpoint = new OctopusServerEndpoint(octopusEndpoint, octopusApiKey);
			var client = await OctopusAsyncClient.Create(endpoint);
			var repository = new OctopusAsyncRepository(client);
			var project = await repository.Projects.FindByName(projectName);

			var sets = new ConcurrentDictionary<string, string>();

			await Task.WhenAll(
				 project.IncludedLibraryVariableSetIds
				 .Select(async x =>
					  {
						  try
						  {
							  var set = await repository.LibraryVariableSets.Get(x);
							  var vars = await repository.VariableSets.Get(set.VariableSetId);

							  var environment = vars.ScopeValues.Environments.FirstOrDefault(e => e.Name.Equals(environmentName, System.StringComparison.OrdinalIgnoreCase));
							  if (!string.IsNullOrWhiteSpace(environment?.Id))
							  {
								  var envVars = vars.Variables
											.Where(vs =>
												 (vs.Scope.TryGetValue(ScopeField.Environment, out var vs1) &&
												 vs1.Any(e => e == environment.Id)));

								  foreach (var s in envVars)
								  {
									  sets.TryAdd(s.Name, s.Value);
								  }

								  envVars = vars.Variables
											.Where(vs => !vs.Scope.ContainsKey(ScopeField.Environment));

								  foreach (var s in envVars)
								  {
									  sets.TryAdd(s.Name, s.Value);
								  }
							  }

							  sets.TryAdd("Octopus.Environment.Name", environment.Name);
						  }
						  catch { }
					  })
				 .ToArray());

			//меняем вложенные переменные
			for (var i = 0; i < 10; i++)
			{
				bool hasChanges = false;
				foreach (var set in sets)
				{
					foreach (var sub in sets)
					{
						if (sub.Key == set.Key)
						{
							continue;
						}

						var replace = $"#{{{set.Key}}}";
						if (sets[sub.Key]?.Contains(replace) == true)
						{
							hasChanges = true;
							sets[sub.Key] = sets[sub.Key]?.Replace(replace, set.Value);
						}
					}
				}

				if (!hasChanges)
				{
					break;
				}
			}

			return sets;
		}
	}
}
